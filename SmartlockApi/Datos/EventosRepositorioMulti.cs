using System.Data;
using Microsoft.Data.SqlClient;
using SmartlockApi.Modelos;

namespace SmartlockApi.Datos;

public sealed class EventosRepositorioMulti(IConfiguration configuration)
{
    private readonly string _cadenaConexion = configuration.GetConnectionString("SmartlockDb")
        ?? throw new InvalidOperationException("Falta la cadena de conexion 'SmartlockDb' en appsettings.json");

    private const string ConsultaEventos = """
        SELECT e.id_evento, e.nombre_evento, e.fechaprogramada_evento,
               e.fechalimite_evento, e.estado_evento,
               de.id_detalle_evento, de.id_usuario, u.nombre_usuario,
               de.acceso_detalle_evento, de.metodo_acceso, de.codigo_qr, de.qr_usado
        FROM dbo.eventos e
        LEFT JOIN dbo.detalle_eventos de ON de.id_evento = e.id_evento
        LEFT JOIN dbo.usuarios u ON u.id_usuario = de.id_usuario
        """;

    public async Task<IReadOnlyList<EventoResponse>> ListarAsync(CancellationToken cancellationToken)
    {
        var eventos = new Dictionary<int, EventoAcumulado>();
        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(ConsultaEventos + " ORDER BY e.fechaprogramada_evento DESC;", conexion);
        await conexion.OpenAsync(cancellationToken);
        await using var lector = await comando.ExecuteReaderAsync(cancellationToken);

        while (await lector.ReadAsync(cancellationToken))
            AgregarFila(eventos, lector);

        return eventos.Values.Select(evento => evento.ToResponse()).ToArray();
    }

    public async Task<EventoResponse?> ObtenerAsync(int idEvento, CancellationToken cancellationToken)
    {
        var eventos = new Dictionary<int, EventoAcumulado>();
        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(ConsultaEventos + " WHERE e.id_evento = @IdEvento;", conexion);
        comando.Parameters.Add("@IdEvento", SqlDbType.Int).Value = idEvento;
        await conexion.OpenAsync(cancellationToken);
        await using var lector = await comando.ExecuteReaderAsync(cancellationToken);

        while (await lector.ReadAsync(cancellationToken))
            AgregarFila(eventos, lector);

        return eventos.TryGetValue(idEvento, out var evento) ? evento.ToResponse() : null;
    }

    public async Task<int> DesactivarVencidosAsync(CancellationToken cancellationToken)
    {
        const string consulta = """
            UPDATE dbo.eventos
            SET estado_evento = 0
            WHERE estado_evento = 1
              AND fechalimite_evento IS NOT NULL
              AND fechalimite_evento <= GETDATE();

            SELECT @@ROWCOUNT;
            """;

        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        await conexion.OpenAsync(cancellationToken);

        object? resultado = await comando.ExecuteScalarAsync(cancellationToken);
        return resultado is null || resultado is DBNull ? 0 : Convert.ToInt32(resultado);
    }

    public async Task<int> CrearAsync(CrearEventoRequest request, CancellationToken cancellationToken)
    {
        const string insertarEvento = """
            INSERT INTO dbo.eventos
                (nombre_evento, fechaprogramada_evento, fechalimite_evento,
                 descripcion_evento, estado_evento)
            OUTPUT INSERTED.id_evento
            VALUES (@Tipo, @Inicio, @Fin, @Tipo, 1);
            """;
        const string insertarDetalle = """
            INSERT INTO dbo.detalle_eventos (id_usuario, id_evento, rol_en_evento)
            VALUES (@IdUsuario, @IdEvento, 'Visitante');
            """;

        await using var conexion = new SqlConnection(_cadenaConexion);
        await conexion.OpenAsync(cancellationToken);
        await using var transaccion = (SqlTransaction)await conexion.BeginTransactionAsync(cancellationToken);
        try
        {
            await using var comandoEvento = new SqlCommand(insertarEvento, conexion, transaccion);
            comandoEvento.Parameters.Add("@Tipo", SqlDbType.VarChar, 100).Value = request.Tipo.Trim();
            comandoEvento.Parameters.Add("@Inicio", SqlDbType.DateTime).Value = request.Fecha.ToDateTime(request.HoraInicio);
            comandoEvento.Parameters.Add("@Fin", SqlDbType.DateTime).Value = request.Fecha.ToDateTime(request.HoraFin);
            int idEvento = Convert.ToInt32(await comandoEvento.ExecuteScalarAsync(cancellationToken));

            foreach (int idUsuario in request.IdUsuarios.Distinct())
            {
                await using var comandoDetalle = new SqlCommand(insertarDetalle, conexion, transaccion);
                comandoDetalle.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
                comandoDetalle.Parameters.Add("@IdEvento", SqlDbType.Int).Value = idEvento;
                await comandoDetalle.ExecuteNonQueryAsync(cancellationToken);
            }

            await transaccion.CommitAsync(cancellationToken);
            return idEvento;
        }
        catch
        {
            await transaccion.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<bool> ActualizarAsync(
        int idEvento,
        ActualizarEventoRequest request,
        CancellationToken cancellationToken)
    {
        const string consulta = """
            UPDATE dbo.eventos
            SET nombre_evento = @Tipo,
                descripcion_evento = @Tipo,
                fechaprogramada_evento = @Inicio,
                fechalimite_evento = @Fin,
                estado_evento = @Estado
            WHERE id_evento = @IdEvento;
            """;

        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@Tipo", SqlDbType.VarChar, 100).Value = request.Tipo.Trim();
        comando.Parameters.Add("@Inicio", SqlDbType.DateTime).Value = request.Fecha.ToDateTime(request.HoraInicio);
        comando.Parameters.Add("@Fin", SqlDbType.DateTime).Value = request.Fecha.ToDateTime(request.HoraFin);
        comando.Parameters.Add("@Estado", SqlDbType.Int).Value = request.Estado == "Activo" ? 1 : 0;
        comando.Parameters.Add("@IdEvento", SqlDbType.Int).Value = idEvento;

        await conexion.OpenAsync(cancellationToken);
        return await comando.ExecuteNonQueryAsync(cancellationToken) > 0;
    }

    public async Task<int> AgregarParticipanteAsync(int idEvento, int idUsuario, CancellationToken cancellationToken)
    {
        const string consulta = """
            IF NOT EXISTS (SELECT 1 FROM dbo.eventos WHERE id_evento = @IdEvento)
                RETURN;

            IF NOT EXISTS (
                SELECT 1 FROM dbo.detalle_eventos
                WHERE id_evento = @IdEvento AND id_usuario = @IdUsuario
            )
            BEGIN
                INSERT INTO dbo.detalle_eventos (id_usuario, id_evento, rol_en_evento)
                OUTPUT INSERTED.id_detalle_evento
                VALUES (@IdUsuario, @IdEvento, 'Visitante');
            END
            """;

        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@IdEvento", SqlDbType.Int).Value = idEvento;
        comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
        await conexion.OpenAsync(cancellationToken);
        object? resultado = await comando.ExecuteScalarAsync(cancellationToken);
        return resultado is null || resultado is DBNull ? 0 : Convert.ToInt32(resultado);
    }

    public async Task<bool> EliminarParticipanteAsync(
        int idEvento,
        int idDetalleEvento,
        CancellationToken cancellationToken)
    {
        const string consulta = """
            DELETE FROM dbo.detalle_eventos
            WHERE id_evento = @IdEvento AND id_detalle_evento = @IdDetalleEvento;
            """;

        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@IdEvento", SqlDbType.Int).Value = idEvento;
        comando.Parameters.Add("@IdDetalleEvento", SqlDbType.Int).Value = idDetalleEvento;
        await conexion.OpenAsync(cancellationToken);
        return await comando.ExecuteNonQueryAsync(cancellationToken) > 0;
    }

    public async Task<bool> EliminarAsync(int idEvento, CancellationToken cancellationToken)
    {
        const string consulta = """
            DELETE FROM dbo.detalle_eventos WHERE id_evento = @IdEvento;
            DELETE FROM dbo.eventos WHERE id_evento = @IdEvento;
            """;

        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@IdEvento", SqlDbType.Int).Value = idEvento;
        await conexion.OpenAsync(cancellationToken);
        return await comando.ExecuteNonQueryAsync(cancellationToken) > 0;
    }

    private static void AgregarFila(Dictionary<int, EventoAcumulado> eventos, SqlDataReader lector)
    {
        int idEvento = lector.GetInt32(lector.GetOrdinal("id_evento"));
        if (!eventos.TryGetValue(idEvento, out var evento))
        {
            int ordinalInicio = lector.GetOrdinal("fechaprogramada_evento");
            int ordinalFin = lector.GetOrdinal("fechalimite_evento");
            evento = new EventoAcumulado(
                idEvento,
                lector.GetString(lector.GetOrdinal("nombre_evento")),
                lector.IsDBNull(ordinalInicio) ? null : lector.GetDateTime(ordinalInicio),
                lector.IsDBNull(ordinalFin) ? null : lector.GetDateTime(ordinalFin),
                lector.GetInt32(lector.GetOrdinal("estado_evento")));
            eventos.Add(idEvento, evento);
        }

        int ordinalDetalle = lector.GetOrdinal("id_detalle_evento");
        int ordinalUsuario = lector.GetOrdinal("id_usuario");
        if (lector.IsDBNull(ordinalDetalle) || lector.IsDBNull(ordinalUsuario))
            return;

        int idDetalle = lector.GetInt32(ordinalDetalle);
        int idUsuario = lector.GetInt32(ordinalUsuario);
        int ordinalNombre = lector.GetOrdinal("nombre_usuario");
        int ordinalQr = lector.GetOrdinal("codigo_qr");
        int ordinalAcceso = lector.GetOrdinal("acceso_detalle_evento");
        string? codigoQr = lector.IsDBNull(ordinalQr) ? null : lector.GetString(ordinalQr);
        IReadOnlyList<RegistroAccesoResponse> historial = lector.IsDBNull(ordinalAcceso)
            ? Array.Empty<RegistroAccesoResponse>()
            : new[]
            {
                new RegistroAccesoResponse(
                    idDetalle,
                    lector.GetDateTime(ordinalAcceso).ToString("yyyy-MM-dd"),
                    lector.GetDateTime(ordinalAcceso).ToString("HH:mm:ss"),
                    lector.IsDBNull(lector.GetOrdinal("metodo_acceso")) ? "No registrado" : lector.GetString(lector.GetOrdinal("metodo_acceso")))
            };

        evento.Participantes.Add(new ParticipanteEventoResponse(
            idDetalle,
            idUsuario,
            lector.IsDBNull(ordinalNombre) ? "Usuario" : lector.GetString(ordinalNombre),
            codigoQr is null ? "Pendiente" : lector.GetBoolean(lector.GetOrdinal("qr_usado")) ? "Utilizado" : "Generado",
            codigoQr,
            historial));
    }

    private sealed class EventoAcumulado(int idEvento, string tipo, DateTime? inicio, DateTime? fin, int estado)
    {
        public List<ParticipanteEventoResponse> Participantes { get; } = [];

        public EventoResponse ToResponse() => new(
            idEvento,
            tipo,
            inicio?.ToString("yyyy-MM-dd") ?? string.Empty,
            inicio?.ToString("HH:mm") ?? string.Empty,
            fin?.ToString("HH:mm") ?? string.Empty,
            estado == 1 ? "Activo" : "Completado",
            Participantes);
    }
}