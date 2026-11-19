USE [SMARTLOCK_BD];
GO

SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF OBJECT_ID(N'dbo.incidentes', N'U') IS NULL
BEGIN
    ;THROW 51020, 'No existe dbo.incidentes. Selecciona la base SMARTLOCK_BD correcta.', 1;
END;

IF OBJECT_ID(N'dbo.smartlock_accesos_rechazados', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.smartlock_accesos_rechazados
    (
        id_acceso_rechazado bigint IDENTITY(1,1) NOT NULL
            CONSTRAINT PK_smartlock_accesos_rechazados PRIMARY KEY,
        registro_id uniqueidentifier NOT NULL
            CONSTRAINT UQ_smartlock_accesos_rechazados_registro UNIQUE,
        id_usuario int NULL,
        metodo_acceso varchar(20) NOT NULL,
        credencial_hash varchar(128) NOT NULL,
        motivo varchar(80) NOT NULL,
        fecha_acceso datetime2(0) NOT NULL
            CONSTRAINT DF_smartlock_accesos_rechazados_fecha
            DEFAULT GETDATE()
    );

    CREATE NONCLUSTERED INDEX IX_smartlock_accesos_rechazados_fecha
        ON dbo.smartlock_accesos_rechazados(fecha_acceso DESC);
END;

IF OBJECT_ID(N'dbo.smartlock_reconexion_sync', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.smartlock_reconexion_sync
    (
        registro_id uniqueidentifier NOT NULL
            CONSTRAINT PK_smartlock_reconexion_sync PRIMARY KEY,
        id_incidente int NOT NULL,
        sincronizado_utc datetime2(0) NOT NULL
            CONSTRAINT DF_smartlock_reconexion_sync_sincronizado_utc
            DEFAULT SYSUTCDATETIME()
    );
END;

COMMIT TRANSACTION;
GO
