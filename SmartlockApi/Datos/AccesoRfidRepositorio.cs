using System.Data;
using Microsoft.Data.SqlClient;

namespace SmartlockApi.Datos;

public sealed record AccesoRfidRegistrado(
    int IdDetalleEvento,
    int IdUsuario,
    string NombreUsuario,
    DateTime FechaAcceso);

public sealed class AccesoRfidRepositorio(IConfiguration configuration)
{
    private const string MarcadorEventoPermanente = "SMARTLOCK:RFID:PERMANENT_ACCESS:v1";
    private readonly string _cadenaConexion = configuration.GetConnectionString("SmartlockDb")
        ?? throw new InvalidOperationException("Falta la cadena de conexion 'SmartlockDb' en appsettings.json");

    public async Task<AccesoRfidRegistrado?> RegistrarAsync(
        string hashTarjeta,
        CancellationToken cancellationToken)
    {
        const string consulta = """
            SET XACT_ABORT ON;
            BEGIN TRANSACTION;

            DECLARE @IdEvento int;
            SELECT @IdEvento = id_evento
            FROM dbo.eventos WITH (UPDLOCK, HOLDLOCK)
            WHERE descripcion_evento = @MarcadorEvento;

            IF @IdEvento IS NULL
                THROW 51010, 'No se encontró el evento permanente RFID.', 1;

            IF NOT EXISTS (
                SELECT 1
                FROM dbo.eventos
                WHERE id_evento = @IdEvento
                  AND estado_evento = 1
                  AND fechaprogramada_evento IS NULL
                  AND fechalimite_evento IS NULL
            )
                THROW 51011, 'El evento permanente RFID no está activo.', 1;

            DECLARE @IdUsuario int;
            DECLARE @NombreUsuario varchar(200);
            DECLARE @EstadoUsuario int;
            DECLARE @FechaAcceso datetime = GETDATE();
            DECLARE @DetalleCreado TABLE (id_detalle_evento int);

            SELECT TOP (1)
                @IdUsuario = id_usuario,
                @NombreUsuario = nombre_usuario,
                @EstadoUsuario = estado_usuario
            FROM dbo.usuarios WITH (UPDLOCK, HOLDLOCK)
            WHERE pin_usuario = @HashTarjeta;

            IF @IdUsuario IS NOT NULL AND @EstadoUsuario = 1
            BEGIN
                INSERT INTO dbo.detalle_eventos
                    (id_usuario, id_evento, acceso_detalle_evento, metodo_acceso)
                OUTPUT INSERTED.id_detalle_evento INTO @DetalleCreado
                VALUES (@IdUsuario, @IdEvento, @FechaAcceso, 'RFID');
            END;
            ELSE
            BEGIN
                INSERT INTO dbo.smartlock_accesos_rechazados
                    (registro_id, id_usuario, metodo_acceso, credencial_hash,
                     motivo, fecha_acceso)
                VALUES
                    (NEWID(), @IdUsuario, 'RFID', @HashTarjeta,
                     CASE WHEN @IdUsuario IS NULL
                          THEN 'tarjeta_no_registrada'
                          ELSE 'usuario_inactivo'
                     END,
                     @FechaAcceso);
            END;

            COMMIT TRANSACTION;

            SELECT
                d.id_detalle_evento,
                @IdUsuario AS id_usuario,
                @NombreUsuario AS nombre_usuario,
                @FechaAcceso AS fecha_acceso
            FROM @DetalleCreado d;
            """;

        await using var conexion = new SqlConnection(_cadenaConexion);
        await using var comando = new SqlCommand(consulta, conexion);
        comando.Parameters.Add("@MarcadorEvento", SqlDbType.VarChar, 150).Value = MarcadorEventoPermanente;
        comando.Parameters.Add("@HashTarjeta", SqlDbType.VarChar, 100).Value = hashTarjeta;

        await conexion.OpenAsync(cancellationToken);
        await using var lector = await comando.ExecuteReaderAsync(cancellationToken);
        if (!await lector.ReadAsync(cancellationToken))
            return null;

        return new AccesoRfidRegistrado(
            lector.GetInt32(lector.GetOrdinal("id_detalle_evento")),
            lector.GetInt32(lector.GetOrdinal("id_usuario")),
            lector.GetString(lector.GetOrdinal("nombre_usuario")),
            lector.GetDateTime(lector.GetOrdinal("fecha_acceso")));
    }
}
