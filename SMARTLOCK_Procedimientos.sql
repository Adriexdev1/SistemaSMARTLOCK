USE SMARTLOCK_BD;
GO
IF OBJECT_ID('SP_REGISTRARROL', 'P') IS NOT NULL DROP PROCEDURE SP_REGISTRARROL;
GO
CREATE PROCEDURE SP_REGISTRARROL
    @nombre_rol VARCHAR(50),
    @descripcion_rol VARCHAR(150),
    @estado_rol INT,
    @permiso_rol INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO roles (nombre_rol, descripcion_rol, estado_rol, permiso_rol)
    VALUES (@nombre_rol, @descripcion_rol, @estado_rol, @permiso_rol);
END
GO

IF OBJECT_ID('SP_EDITARROL', 'P') IS NOT NULL DROP PROCEDURE SP_EDITARROL;
GO
CREATE PROCEDURE SP_EDITARROL
    @id_rol INT,
    @nombre_rol VARCHAR(50),
    @descripcion_rol VARCHAR(150),
    @estado_rol INT,
    @permiso_rol INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE roles
    SET nombre_rol = @nombre_rol,
        descripcion_rol = @descripcion_rol,
        estado_rol = @estado_rol,
        permiso_rol = @permiso_rol
    WHERE id_rol = @id_rol;
END
GO

IF OBJECT_ID('SP_ELIMINARROL', 'P') IS NOT NULL DROP PROCEDURE SP_ELIMINARROL;
GO
CREATE PROCEDURE SP_ELIMINARROL
    @id_rol INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM roles WHERE id_rol = @id_rol;
END
GO


IF OBJECT_ID('SP_REGISTRARUSUARIO', 'P') IS NOT NULL DROP PROCEDURE SP_REGISTRARUSUARIO;
GO
CREATE PROCEDURE SP_REGISTRARUSUARIO
    @nombre_usuario VARCHAR(200),
    @telefono_usuario VARCHAR(25),
    @correo_usuario VARCHAR(100),
    @id_rol INT,
    @contrasena_usuario VARCHAR(100),
    @nacimiento_usuario DATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO usuarios (nombre_usuario, telefono_usuario, correo_usuario, id_rol, contrasena_usuario, nacimiento_usuario)
    VALUES (@nombre_usuario, @telefono_usuario, @correo_usuario, @id_rol, @contrasena_usuario, @nacimiento_usuario);
END
GO

IF OBJECT_ID('SP_EDITARUSUARIO', 'P') IS NOT NULL DROP PROCEDURE SP_EDITARUSUARIO;
GO
CREATE PROCEDURE SP_EDITARUSUARIO
    @id_usuario INT,
    @nombre_usuario VARCHAR(200),
    @telefono_usuario VARCHAR(25),
    @correo_usuario VARCHAR(100),
    @id_rol INT,
    @estado_usuario INT,
    @nacimiento_usuario DATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE usuarios
    SET nombre_usuario = @nombre_usuario,
        telefono_usuario = @telefono_usuario,
        correo_usuario = @correo_usuario,
        id_rol = @id_rol,
        estado_usuario = @estado_usuario,
        nacimiento_usuario = @nacimiento_usuario
    WHERE id_usuario = @id_usuario;
END
GO

IF OBJECT_ID('SP_ELIMINARUSUARIO', 'P') IS NOT NULL DROP PROCEDURE SP_ELIMINARUSUARIO;
GO
CREATE PROCEDURE SP_ELIMINARUSUARIO
    @id_usuario INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM usuarios WHERE id_usuario = @id_usuario;
END
GO


IF OBJECT_ID('SP_REGISTRAREVENTO', 'P') IS NOT NULL DROP PROCEDURE SP_REGISTRAREVENTO;
GO
CREATE PROCEDURE SP_REGISTRAREVENTO
    @nombre_evento VARCHAR(100),
    @fechaprogramada_evento DATETIME,
    @descripcion_evento VARCHAR(150),
    @estado_evento INT,
    @fechalimite_evento DATETIME
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO eventos (nombre_evento, fechaprogramada_evento, descripcion_evento, estado_evento, fechalimite_evento)
    VALUES (@nombre_evento, @fechaprogramada_evento, @descripcion_evento, @estado_evento, @fechalimite_evento);
END
GO

IF OBJECT_ID('SP_EDITAREVENTO', 'P') IS NOT NULL DROP PROCEDURE SP_EDITAREVENTO;
GO
CREATE PROCEDURE SP_EDITAREVENTO
    @id_evento INT,
    @nombre_evento VARCHAR(100),
    @fechaprogramada_evento DATETIME,
    @descripcion_evento VARCHAR(150),
    @estado_evento INT,
    @fechalimite_evento DATETIME
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE eventos
    SET nombre_evento = @nombre_evento,
        fechaprogramada_evento = @fechaprogramada_evento,
        descripcion_evento = @descripcion_evento,
        estado_evento = @estado_evento,
        fechalimite_evento = @fechalimite_evento
    WHERE id_evento = @id_evento;
END
GO

IF OBJECT_ID('SP_ELIMINAREVENTO', 'P') IS NOT NULL DROP PROCEDURE SP_ELIMINAREVENTO;
GO
CREATE PROCEDURE SP_ELIMINAREVENTO
    @id_evento INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM eventos WHERE id_evento = @id_evento;
END
GO

IF OBJECT_ID('SP_REGISTRAR_DETALLE_EVENTO', 'P') IS NOT NULL DROP PROCEDURE SP_REGISTRAR_DETALLE_EVENTO;
GO
CREATE PROCEDURE SP_REGISTRAR_DETALLE_EVENTO
    @id_usuario INT,
    @id_evento INT,
    @rol_en_evento VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO detalle_eventos (id_usuario, id_evento, rol_en_evento)
    VALUES (@id_usuario, @id_evento, @rol_en_evento);
END
GO

IF OBJECT_ID('SP_ELIMINAR_DETALLE_EVENTO', 'P') IS NOT NULL DROP PROCEDURE SP_ELIMINAR_DETALLE_EVENTO;
GO
CREATE PROCEDURE SP_ELIMINAR_DETALLE_EVENTO
    @id_detalle_evento INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM detalle_eventos WHERE id_detalle_evento = @id_detalle_evento;
END
GO