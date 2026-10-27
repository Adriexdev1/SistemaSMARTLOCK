USE SL_NEW;
GO

IF COL_LENGTH('dbo.usuarios', 'pin_usuario') IS NULL
BEGIN
    ALTER TABLE dbo.usuarios
    ADD pin_usuario VARCHAR(100) NULL;
END
GO

IF OBJECT_ID('dbo.SP_OBTENER_PIN_USUARIO', 'P') IS NOT NULL
    DROP PROCEDURE dbo.SP_OBTENER_PIN_USUARIO;
GO
CREATE PROCEDURE dbo.SP_OBTENER_PIN_USUARIO
    @id_usuario INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT pin_usuario
    FROM dbo.usuarios
    WHERE id_usuario = @id_usuario;
END
GO

IF OBJECT_ID('dbo.SP_GUARDAR_PIN_USUARIO', 'P') IS NOT NULL
    DROP PROCEDURE dbo.SP_GUARDAR_PIN_USUARIO;
GO
CREATE PROCEDURE dbo.SP_GUARDAR_PIN_USUARIO
    @id_usuario INT,
    @pin_hash VARCHAR(100),
    @resultado BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.usuarios
    SET pin_usuario = @pin_hash
    WHERE id_usuario = @id_usuario AND estado_usuario = 1;

    SET @resultado = CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END;
END
GO

IF OBJECT_ID('dbo.SP_ELIMINAR_PIN_USUARIO', 'P') IS NOT NULL
    DROP PROCEDURE dbo.SP_ELIMINAR_PIN_USUARIO;
GO
CREATE PROCEDURE dbo.SP_ELIMINAR_PIN_USUARIO
    @id_usuario INT,
    @resultado BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.usuarios
    SET pin_usuario = NULL
    WHERE id_usuario = @id_usuario AND estado_usuario = 1;

    SET @resultado = CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END;
END
GO