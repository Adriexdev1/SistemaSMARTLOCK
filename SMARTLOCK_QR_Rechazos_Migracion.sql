IF OBJECT_ID(N'dbo.smartlock_qr_rechazos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.smartlock_qr_rechazos
    (
        id_rechazo BIGINT IDENTITY(1,1) NOT NULL,
        registro_id UNIQUEIDENTIFIER NOT NULL,
        id_detalle_evento INT NULL,
        codigo_qr_hash VARCHAR(128) NOT NULL,
        motivo VARCHAR(80) NOT NULL,
        fecha_intento DATETIME2(0) NOT NULL,
        CONSTRAINT PK_smartlock_qr_rechazos
            PRIMARY KEY CLUSTERED (id_rechazo),
        CONSTRAINT UQ_smartlock_qr_rechazos_registro_id
            UNIQUE (registro_id)
    );

    CREATE INDEX IX_smartlock_qr_rechazos_fecha
        ON dbo.smartlock_qr_rechazos (fecha_intento DESC);
END;
GO
