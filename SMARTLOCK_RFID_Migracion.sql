USE [SMARTLOCK_BD];
GO

SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF OBJECT_ID(N'dbo.usuarios', N'U') IS NULL
BEGIN
    ;THROW 51000, 'No existe dbo.usuarios. Selecciona la base SMARTLOCK_BD correcta.', 1;
END;

IF COL_LENGTH(N'dbo.usuarios', N'pin_usuario') IS NULL
BEGIN
    ;THROW 51001, 'No existe usuarios.pin_usuario.', 1;
END;

IF EXISTS (
    SELECT pin_usuario
    FROM dbo.usuarios
    WHERE pin_usuario IS NOT NULL
    GROUP BY pin_usuario
    HAVING COUNT(*) > 1
)
BEGIN
    ;THROW 51004, 'Hay valores de PIN duplicados; resuélvelos antes de crear el índice de tarjetas.', 1;
END;

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.usuarios')
      AND name = N'UX_usuarios_pin_usuario'
)
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX UX_usuarios_pin_usuario
        ON dbo.usuarios(pin_usuario)
        WHERE pin_usuario IS NOT NULL;
END;

IF OBJECT_ID(N'dbo.eventos', N'U') IS NULL
BEGIN
    ;THROW 51002, 'No existe dbo.eventos. Selecciona la base SMARTLOCK_BD correcta.', 1;
END;

IF OBJECT_ID(N'dbo.detalle_eventos', N'U') IS NULL
BEGIN
    ;THROW 51005, 'No existe dbo.detalle_eventos.', 1;
END;

IF (
    SELECT COUNT(*)
    FROM dbo.eventos
    WHERE descripcion_evento = 'SMARTLOCK:RFID:PERMANENT_ACCESS:v1'
) > 1
BEGIN
    ;THROW 51003, 'Hay más de un evento reservado para RFID; corrige los duplicados antes de continuar.', 1;
END;

IF EXISTS (
    SELECT 1
    FROM dbo.eventos
    WHERE descripcion_evento = 'SMARTLOCK:RFID:PERMANENT_ACCESS:v1'
)
BEGIN
    UPDATE dbo.eventos
    SET nombre_evento = 'Acceso permanente RFID',
        fechaprogramada_evento = NULL,
        fechalimite_evento = NULL,
        estado_evento = 1
    WHERE descripcion_evento = 'SMARTLOCK:RFID:PERMANENT_ACCESS:v1';
END
ELSE
BEGIN
    INSERT INTO dbo.eventos
        (nombre_evento, fechaprogramada_evento, descripcion_evento,
         estado_evento, fechalimite_evento)
    VALUES
        ('Acceso permanente RFID', NULL, 'SMARTLOCK:RFID:PERMANENT_ACCESS:v1',
         1, NULL);
END;

IF OBJECT_ID(N'dbo.smartlock_rfid_sync', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.smartlock_rfid_sync
    (
        registro_id uniqueidentifier NOT NULL
            CONSTRAINT PK_smartlock_rfid_sync PRIMARY KEY,
        id_detalle_evento int NOT NULL,
        sincronizado_utc datetime2(0) NOT NULL
            CONSTRAINT DF_smartlock_rfid_sync_sincronizado_utc
            DEFAULT SYSUTCDATETIME()
    );
END;

COMMIT TRANSACTION;
GO
