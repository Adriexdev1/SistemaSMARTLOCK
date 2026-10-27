using System.Data;
using Microsoft.Data.SqlClient;
using SmartlockApi.Modelos;

namespace SmartlockApi.Datos;

public sealed class EventosRepositorio(IConfiguration configuration)
{
    private readonly string _cadenaConexion = configuration.GetConnectionString("SmartlockDb")
        ?? throw new InvalidOperationException("Falta la cadena de conexion 'SmartlockDb' en appsettings.json");

    public async Task<IReadOnlyList<EventoResponse>> ListarAsync(CancellationToken cancellationToken)
    {
        const string consulta = """
            SELECT e.id_evento, de.id_detalle_evento, u.id_usuario, u.nombre_usuario,
                   e.nombre_evento, e.fechaprogramada_evento, e.fechalimite_evento,
                   e.estado_evento, de.acceso_detalle_evento, de.metodo_acceso, de.codigo_qr, de.qr_usado
            FROM dbo.detalle_eventos de
            INNER JOIN dbo.eventos e ON e.id_evento = de.id_evento
            INNER JOIN dbo.usuarios u ON u.id_usuario = de.id_usuario
            ORDER BY e.fechaprogramada_evento DESC;
            """;

        var accesos = new List<EventoResponse>();
        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        await conexion.OpenAsync(cancellationToken);
        await using var lector = await comando.ExecuteReaderAsync(cancellationToken);

        while (await lector.ReadAsync(cancellationToken))
            accesos.Add(Mapear(lector));

        return accesos;
    }

    public async Task<EventoResponse?> ObtenerAsync(int idEvento, CancellationToken cancellationToken)
    {
        const string consulta = """
            SELECT e.id_evento, de.id_detalle_evento, u.id_usuario, u.nombre_usuario,
                   e.nombre_evento, e.fechaprogramada_evento, e.fechalimite_evento,
                   e.estado_evento, de.acceso_detalle_evento, de.metodo_acceso, de.codigo_qr, de.qr_usado
            FROM dbo.detalle_eventos de
            INNER JOIN dbo.eventos e ON e.id_evento = de.id_evento
            INNER JOIN dbo.usuarios u ON u.id_usuario = de.id_usuario
            WHERE de.id_detalle_evento = @IdDetalleEvento;
            """;

        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@IdDetalleEvento", SqlDbType.Int).Value = idEvento;
        await conexion.OpenAsync(cancellationToken);
        await using var lector = await comando.ExecuteReaderAsync(cancellationToken);

        return await lector.ReadAsync(cancellationToken) ? Mapear(lector) : null;
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
            OUTPUT INSERTED.id_detalle_evento
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

            await using var comandoDetalle = new SqlCommand(insertarDetalle, conexion, transaccion);
            comandoDetalle.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = request.IdUsuario;
            comandoDetalle.Parameters.Add("@IdEvento", SqlDbType.Int).Value = idEvento;
            int idDetalleEvento = Convert.ToInt32(await comandoDetalle.ExecuteScalarAsync(cancellationToken));

            await transaccion.CommitAsync(cancellationToken);
            return idDetalleEvento;
        }
        catch
        {
            await transaccion.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<bool> ActualizarAsync(
        int idDetalleEvento,
        ActualizarEventoRequest request,
        CancellationToken cancellationToken)
    {
        const string consulta = """
            UPDATE e
            SET e.nombre_evento = @Tipo,
                e.descripcion_evento = @Tipo,
                e.fechaprogramada_evento = @Inicio,
                e.fechalimite_evento = @Fin,
                e.estado_evento = @Estado
            FROM dbo.eventos e
            INNER JOIN dbo.detalle_eventos de ON de.id_evento = e.id_evento
            WHERE de.id_detalle_evento = @IdDetalleEvento;

            UPDATE dbo.detalle_eventos
            SET id_usuario = @IdUsuario,
                codigo_qr = NULL,
                qr_usado = 0
            WHERE id_detalle_evento = @IdDetalleEvento;
            """;

        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@Tipo", SqlDbType.VarChar, 100).Value = request.Tipo.Trim();
        comando.Parameters.Add("@Inicio", SqlDbType.DateTime).Value = request.Fecha.ToDateTime(request.HoraInicio);
        comando.Parameters.Add("@Fin", SqlDbType.DateTime).Value = request.Fecha.ToDateTime(request.HoraFin);
        comando.Parameters.Add("@Estado", SqlDbType.Int).Value = request.Estado == "Activo" ? 1 : 0;
        comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = request.IdUsuario;
        comando.Parameters.Add("@IdDetalleEvento", SqlDbType.Int).Value = idDetalleEvento;

        await conexion.OpenAsync(cancellationToken);
        return await comando.ExecuteNonQueryAsync(cancellationToken) > 0;
    }

    public async Task<bool> EliminarAsync(int idDetalleEvento, CancellationToken cancellationToken)
    {
        const string consulta = """
            DECLARE @IdEvento INT;
            SELECT @IdEvento = id_evento
            FROM dbo.detalle_eventos
            WHERE id_detalle_evento = @IdDetalleEvento;

            IF @IdEvento IS NULL RETURN;

            DELETE FROM dbo.detalle_eventos WHERE id_detalle_evento = @IdDetalleEvento;

            IF NOT EXISTS (SELECT 1 FROM dbo.detalle_eventos WHERE id_evento = @IdEvento)
                DELETE FROM dbo.eventos WHERE id_evento = @IdEvento;
            """;

        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@IdDetalleEvento", SqlDbType.Int).Value = idDetalleEvento;
        await conexion.OpenAsync(cancellationToken);
        return await comando.ExecuteNonQueryAsync(cancellationToken) > 0;
    }

    private static EventoResponse Mapear(SqlDataReader lector)
    {
        int ordinalInicio = lector.GetOrdinal("fechaprogramada_evento");
        int ordinalFin = lector.GetOrdinal("fechalimite_evento");
        int ordinalAcceso = lector.GetOrdinal("acceso_detalle_evento");
        int ordinalQr = lector.GetOrdinal("codigo_qr");
        bool qrUsado = lector.GetBoolean(lector.GetOrdinal("qr_usado"));
        string? codigoQr = lector.IsDBNull(ordinalQr) ? null : lector.GetString(ordinalQr);
        var historial = lector.IsDBNull(ordinalAcceso)
            ? Array.Empty<RegistroAccesoResponse>()
            : new[]
            {
                new RegistroAccesoResponse(
                    lector.GetInt32(lector.GetOrdinal("id_detalle_evento")),
                    lector.GetDateTime(ordinalAcceso).ToString("yyyy-MM-dd"),
                    lector.GetDateTime(ordinalAcceso).ToString("HH:mm:ss"),
                    lector.IsDBNull(lector.GetOrdinal("metodo_acceso")) ? "No registrado" : lector.GetString(lector.GetOrdinal("metodo_acceso")))
            };

        return new EventoResponse(
            lector.GetInt32(lector.GetOrdinal("id_evento")),
            lector.GetInt32(lector.GetOrdinal("id_detalle_evento")),
            lector.GetInt32(lector.GetOrdinal("id_usuario")),
            lector.GetString(lector.GetOrdinal("nombre_usuario")),
            lector.GetString(lector.GetOrdinal("nombre_evento")),
            lector.IsDBNull(ordinalInicio) ? string.Empty : lector.GetDateTime(ordinalInicio).ToString("yyyy-MM-dd"),
            lector.IsDBNull(ordinalInicio) ? string.Empty : lector.GetDateTime(ordinalInicio).ToString("HH:mm"),
            lector.IsDBNull(ordinalFin) ? string.Empty : lector.GetDateTime(ordinalFin).ToString("HH:mm"),
            lector.GetInt32(lector.GetOrdinal("estado_evento")) == 1 ? "Activo" : "Completado",
            codigoQr is null ? "Pendiente" : qrUsado ? "Utilizado" : "Generado",
            codigoQr,
            historial);
    }
}