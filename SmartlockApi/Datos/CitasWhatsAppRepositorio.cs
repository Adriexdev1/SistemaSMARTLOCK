using System.Data;
using Microsoft.Data.SqlClient;
using SmartlockApi.Modelos;

namespace SmartlockApi.Datos;

public sealed class CitasWhatsAppRepositorio(IConfiguration configuration)
{
    private readonly string _cadenaConexion = configuration.GetConnectionString("SmartlockDb")
        ?? throw new InvalidOperationException("Falta la cadena de conexion 'SmartlockDb' en appsettings.json");

    public async Task<CrearCitaWhatsAppResponse> CrearAsync(
        CrearCitaWhatsAppRequest request,
        CancellationToken cancellationToken)
    {
        DateOnly fecha = request.Fecha
            ?? throw new ArgumentException("La fecha es requerida.");
        TimeOnly horaInicio = request.HoraInicio
            ?? throw new ArgumentException("La hora de inicio es requerida.");
        TimeOnly horaFin = request.HoraFin
            ?? throw new ArgumentException("La hora de fin es requerida.");

        if (horaFin <= horaInicio)
            throw new ArgumentException("La hora de fin debe ser posterior a la hora de inicio.");

        string telefono = NormalizarTelefono(request.TelefonoWhatsApp);
        string correo = request.CorreoUsuario.Trim().ToLowerInvariant();

        if (telefono.Length is < 8 or > 15)
            throw new ArgumentException("El teléfono de WhatsApp no tiene un formato válido.");

        DateTime inicio = fecha.ToDateTime(horaInicio);
        DateTime fin = fecha.ToDateTime(horaFin);

        await using var conexion = new SqlConnection(_cadenaConexion);
        await conexion.OpenAsync(cancellationToken);

        await using var transaccion = (SqlTransaction)await conexion.BeginTransactionAsync(
            IsolationLevel.Serializable,
            cancellationToken);

        try
        {
            int idUsuario = await ObtenerOCrearUsuarioAsync(
                conexion,
                transaccion,
                request,
                correo,
                telefono,
                cancellationToken);

            const string buscarConflicto = """
                SELECT TOP (1) 1
                FROM dbo.eventos WITH (UPDLOCK, HOLDLOCK)
                WHERE estado_evento = 1
                  AND fechaprogramada_evento < @Fin
                  AND fechalimite_evento > @Inicio;
                """;

            await using (var comando = new SqlCommand(buscarConflicto, conexion, transaccion))
            {
                comando.Parameters.Add("@Inicio", SqlDbType.DateTime).Value = inicio;
                comando.Parameters.Add("@Fin", SqlDbType.DateTime).Value = fin;

                if (await comando.ExecuteScalarAsync(cancellationToken) is not null)
                    throw new CitaWhatsAppConflictoException("Ese horario ya no está disponible.");
            }

            const string insertarEvento = """
                INSERT INTO dbo.eventos
                    (nombre_evento, fechaprogramada_evento, fechalimite_evento,
                     descripcion_evento, estado_evento, fecha_evento)
                OUTPUT INSERTED.id_evento
                VALUES ('Cita Dental', @Inicio, @Fin, @Motivo, 1, GETDATE());
                """;

            int idEvento;
            await using (var comando = new SqlCommand(insertarEvento, conexion, transaccion))
            {
                comando.Parameters.Add("@Inicio", SqlDbType.DateTime).Value = inicio;
                comando.Parameters.Add("@Fin", SqlDbType.DateTime).Value = fin;
                comando.Parameters.Add("@Motivo", SqlDbType.VarChar, 150).Value = request.Motivo.Trim();
                idEvento = Convert.ToInt32(await comando.ExecuteScalarAsync(cancellationToken));
            }

            const string insertarDetalle = """
                INSERT INTO dbo.detalle_eventos
                    (id_usuario, id_evento, fecha_detalle_evento, rol_en_evento)
                OUTPUT INSERTED.id_detalle_evento
                VALUES (@IdUsuario, @IdEvento, GETDATE(), 'Paciente');
                """;

            int idDetalleEvento;
            await using (var comando = new SqlCommand(insertarDetalle, conexion, transaccion))
            {
                comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
                comando.Parameters.Add("@IdEvento", SqlDbType.Int).Value = idEvento;
                idDetalleEvento = Convert.ToInt32(await comando.ExecuteScalarAsync(cancellationToken));
            }

            string codigoQr = await GenerarQrAsync(
                conexion,
                transaccion,
                idDetalleEvento,
                cancellationToken);

            await transaccion.CommitAsync(cancellationToken);

            return new CrearCitaWhatsAppResponse(
                idUsuario,
                idEvento,
                idDetalleEvento,
                codigoQr,
                fecha,
                horaInicio,
                horaFin);
        }
        catch
        {
            await transaccion.RollbackAsync(CancellationToken.None);
            throw;
        }
    }

    private static async Task<int> ObtenerOCrearUsuarioAsync(
        SqlConnection conexion,
        SqlTransaction transaccion,
        CrearCitaWhatsAppRequest request,
        string correo,
        string telefono,
        CancellationToken cancellationToken)
    {
        const string buscarPorCorreo = """
            SELECT TOP (1) id_usuario, telefono_usuario, estado_usuario
            FROM dbo.usuarios WITH (UPDLOCK, HOLDLOCK)
            WHERE LOWER(LTRIM(RTRIM(correo_usuario))) = @Correo;
            """;

        int? idUsuario = null;
        string? telefonoRegistrado = null;
        int estadoUsuario = 0;

        await using (var comando = new SqlCommand(buscarPorCorreo, conexion, transaccion))
        {
            comando.Parameters.Add("@Correo", SqlDbType.VarChar, 100).Value = correo;

            await using var lector = await comando.ExecuteReaderAsync(cancellationToken);
            if (await lector.ReadAsync(cancellationToken))
            {
                idUsuario = lector.GetInt32(lector.GetOrdinal("id_usuario"));
                int ordinalTelefono = lector.GetOrdinal("telefono_usuario");
                telefonoRegistrado = lector.IsDBNull(ordinalTelefono)
                    ? null
                    : lector.GetString(ordinalTelefono);
                estadoUsuario = lector.GetInt32(lector.GetOrdinal("estado_usuario"));
            }
        }

        if (idUsuario.HasValue)
        {
            if (estadoUsuario != 1 ||
                NormalizarTelefono(telefonoRegistrado) != telefono)
            {
                throw new CitaWhatsAppConflictoException(
                    "No se pudo verificar el paciente con ese correo y teléfono.");
            }

            return idUsuario.Value;
        }

        if (string.IsNullOrWhiteSpace(request.NombreUsuario) ||
            request.FechaNacimiento is not DateOnly nacimiento)
        {
            throw new ArgumentException(
                "Para registrar un paciente nuevo se requiere nombre y fecha de nacimiento.");
        }

        const string buscarTelefono = """
            SELECT TOP (1) id_usuario
            FROM dbo.usuarios WITH (UPDLOCK, HOLDLOCK)
            WHERE telefono_usuario IS NOT NULL
              AND REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(telefono_usuario, '+', ''), ' ', ''), '-', ''), '(', ''), ')', '') = @Telefono;
            """;

        await using (var comando = new SqlCommand(buscarTelefono, conexion, transaccion))
        {
            comando.Parameters.Add("@Telefono", SqlDbType.VarChar, 25).Value = telefono;

            if (await comando.ExecuteScalarAsync(cancellationToken) is not null)
                throw new CitaWhatsAppConflictoException(
                    "Ese teléfono ya está asociado a otra cuenta.");
        }

        const string insertarUsuario = """
            INSERT INTO dbo.usuarios
                (nombre_usuario, telefono_usuario, correo_usuario, contrasena_usuario,
                 nacimiento_usuario, id_rol, estado_usuario, fecha_usuario)
            OUTPUT INSERTED.id_usuario
            VALUES (@Nombre, @Telefono, @Correo, NULL, @Nacimiento, NULL, 1, GETDATE());
            """;

        await using (var comando = new SqlCommand(insertarUsuario, conexion, transaccion))
        {
            comando.Parameters.Add("@Nombre", SqlDbType.VarChar, 200).Value = request.NombreUsuario.Trim();
            comando.Parameters.Add("@Telefono", SqlDbType.VarChar, 25).Value = $"+{telefono}";
            comando.Parameters.Add("@Correo", SqlDbType.VarChar, 100).Value = correo;
            comando.Parameters.Add("@Nacimiento", SqlDbType.Date).Value =
                nacimiento.ToDateTime(TimeOnly.MinValue);

            return Convert.ToInt32(await comando.ExecuteScalarAsync(cancellationToken));
        }
    }

    private static async Task<string> GenerarQrAsync(
        SqlConnection conexion,
        SqlTransaction transaccion,
        int idDetalleEvento,
        CancellationToken cancellationToken)
    {
        await using var comando = new SqlCommand(
            "dbo.SP_GENERAR_QR_DETALLE",
            conexion,
            transaccion)
        {
            CommandType = CommandType.StoredProcedure
        };

        comando.Parameters.Add("@id_detalle_evento", SqlDbType.Int).Value = idDetalleEvento;
        var parametroQr = comando.Parameters.Add("@codigo_qr", SqlDbType.VarChar, 100);
        parametroQr.Direction = ParameterDirection.Output;

        await comando.ExecuteNonQueryAsync(cancellationToken);

        if (parametroQr.Value is not string codigoQr || string.IsNullOrWhiteSpace(codigoQr))
            throw new InvalidOperationException("No se pudo generar el código QR para la cita.");

        return codigoQr;
    }

    private static string NormalizarTelefono(string? telefono) =>
        string.IsNullOrWhiteSpace(telefono)
            ? string.Empty
            : new string(telefono.Where(char.IsDigit).ToArray());
}

public sealed class CitaWhatsAppConflictoException(string mensaje) : Exception(mensaje);