USE SL_NEW;
GO

IF COL_LENGTH('dbo.detalle_eventos', 'codigo_qr') IS NULL
BEGIN
    ALTER TABLE dbo.detalle_eventos
    ADD codigo_qr VARCHAR(100) NULL;
END
GO

IF COL_LENGTH('dbo.detalle_eventos', 'qr_usado') IS NULL
BEGIN
    ALTER TABLE dbo.detalle_eventos
    ADD qr_usado BIT NOT NULL
        CONSTRAINT DF_detalle_eventos_qr_usado DEFAULT 0;
END
GO

IF COL_LENGTH('dbo.detalle_eventos', 'metodo_acceso') IS NULL
BEGIN
    ALTER TABLE dbo.detalle_eventos
    ADD metodo_acceso VARCHAR(20) NULL;
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = 'UX_detalle_eventos_codigo_qr'
      AND object_id = OBJECT_ID('dbo.detalle_eventos')
)
BEGIN
    CREATE UNIQUE INDEX UX_detalle_eventos_codigo_qr
        ON dbo.detalle_eventos(codigo_qr)
        WHERE codigo_qr IS NOT NULL;
END
GO

IF OBJECT_ID('dbo.SP_GENERAR_QR_DETALLE', 'P') IS NOT NULL
    DROP PROCEDURE dbo.SP_GENERAR_QR_DETALLE;
GO
CREATE PROCEDURE dbo.SP_GENERAR_QR_DETALLE
    @id_detalle_evento INT,
    @codigo_qr VARCHAR(100) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @codigo_qr = CONVERT(VARCHAR(36), NEWID());

    UPDATE dbo.detalle_eventos
    SET codigo_qr = @codigo_qr,
        qr_usado = 0
    WHERE id_detalle_evento = @id_detalle_evento;

    IF @@ROWCOUNT = 0
        SET @codigo_qr = NULL;
END
GO

IF OBJECT_ID('dbo.SP_DIAGNOSTICO_QR', 'P') IS NOT NULL
    DROP PROCEDURE dbo.SP_DIAGNOSTICO_QR;
GO
CREATE PROCEDURE dbo.SP_DIAGNOSTICO_QR
    @codigo_qr VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CASE
        WHEN de.id_detalle_evento IS NULL THEN 'QR_INEXISTENTE'
        WHEN de.qr_usado = 1 THEN 'QR_YA_USADO'
        WHEN e.estado_evento <> 1 THEN 'EVENTO_INACTIVO'
        WHEN e.fechalimite_evento IS NOT NULL AND e.fechalimite_evento <= GETDATE() THEN 'QR_EXPIRADO'
        ELSE 'QR_VALIDO'
    END AS motivo
    FROM (SELECT @codigo_qr AS codigo_qr) entrada
    LEFT JOIN dbo.detalle_eventos de ON de.codigo_qr = entrada.codigo_qr
    LEFT JOIN dbo.eventos e ON e.id_evento = de.id_evento;
END
GO

IF OBJECT_ID('dbo.SP_VALIDAR_QR', 'P') IS NOT NULL
    DROP PROCEDURE dbo.SP_VALIDAR_QR;
GO
CREATE PROCEDURE dbo.SP_VALIDAR_QR
    @codigo_qr VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT de.id_detalle_evento, de.id_usuario, de.id_evento,
           u.nombre_usuario, e.nombre_evento
    FROM dbo.detalle_eventos de
    INNER JOIN dbo.eventos e ON e.id_evento = de.id_evento
    INNER JOIN dbo.usuarios u ON u.id_usuario = de.id_usuario
    WHERE de.codigo_qr = @codigo_qr
      AND de.qr_usado = 0
      AND e.estado_evento = 1
      AND (e.fechalimite_evento IS NULL OR e.fechalimite_evento > GETDATE());
END
GO

IF OBJECT_ID('dbo.SP_CONSUMIR_QR', 'P') IS NOT NULL
    DROP PROCEDURE dbo.SP_CONSUMIR_QR;
GO
CREATE PROCEDURE dbo.SP_CONSUMIR_QR
    @codigo_qr VARCHAR(100),
    @metodo_acceso VARCHAR(20) = 'Solo QR'
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE de
    SET de.qr_usado = 1,
        de.acceso_detalle_evento = GETDATE(),
        de.metodo_acceso = @metodo_acceso
    OUTPUT inserted.id_detalle_evento
    FROM dbo.detalle_eventos de
    INNER JOIN dbo.eventos e ON e.id_evento = de.id_evento
    WHERE de.codigo_qr = @codigo_qr
      AND de.qr_usado = 0
      AND e.estado_evento = 1
      AND (e.fechalimite_evento IS NULL OR e.fechalimite_evento > GETDATE());
END
GO