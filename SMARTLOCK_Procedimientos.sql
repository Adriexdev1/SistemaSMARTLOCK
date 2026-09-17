/*=====================================================================
  SMARTLOCK_BD - Procedimientos almacenados (CORREGIDOS)

  Estos procedimientos fueron reescritos para coincidir EXACTAMENTE
  con lo que CapaDatos espera (nombres de parametros, cantidad, y
  parametros OUTPUT), verificado contra:
    - CD_Usuario.cs
    - CD_Rol.cs
    - CD_Evento.cs
    - CD_DetalleEvento.cs

  NOTA: CD_DetalleEvento.cs usa una convencion de nombres distinta
  (minusculas con guion bajo) a los otros tres (PascalCase). Esto es
  asi en el codigo original de la app, se respeta tal cual.

  Ejecutar DESPUES de SMARTLOCK_BD_Simplificado.sql. Reemplaza por
  completo la version anterior de este archivo.
=====================================================================*/

USE SMARTLOCK_BD;
GO

/*====================
  ROLES
====================*/
IF OBJECT_ID('SP_REGISTRARROL', 'P') IS NOT NULL DROP PROCEDURE SP_REGISTRARROL;
GO
CREATE PROCEDURE SP_REGISTRARROL
    @NombreRol VARCHAR(50),
    @DescripcionRol VARCHAR(150),
    @EstadoRol INT,
    @PermisoRol INT,
    @IdRolResultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO roles (nombre_rol, descripcion_rol, estado_rol, permiso_rol)
        VALUES (@NombreRol, @DescripcionRol, @EstadoRol, @PermisoRol);

        SET @IdRolResultado = CAST(SCOPE_IDENTITY() AS INT);
        SET @Mensaje = 'Rol registrado correctamente.';
    END TRY
    BEGIN CATCH
        SET @IdRolResultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END
GO

IF OBJECT_ID('SP_EDITARROL', 'P') IS NOT NULL DROP PROCEDURE SP_EDITARROL;
GO
CREATE PROCEDURE SP_EDITARROL
    @IdRol INT,
    @NombreRol VARCHAR(50),
    @DescripcionRol VARCHAR(150),
    @EstadoRol INT,
    @PermisoRol INT,
    @Respuesta BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE roles
        SET nombre_rol = @NombreRol,
            descripcion_rol = @DescripcionRol,
            estado_rol = @EstadoRol,
            permiso_rol = @PermisoRol
        WHERE id_rol = @IdRol;

        IF @@ROWCOUNT > 0
        BEGIN
            SET @Respuesta = 1;
            SET @Mensaje = 'Rol actualizado correctamente.';
        END
        ELSE
        BEGIN
            SET @Respuesta = 0;
            SET @Mensaje = 'No se encontro el rol indicado.';
        END
    END TRY
    BEGIN CATCH
        SET @Respuesta = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END
GO

IF OBJECT_ID('SP_ELIMINARROL', 'P') IS NOT NULL DROP PROCEDURE SP_ELIMINARROL;
GO
CREATE PROCEDURE SP_ELIMINARROL
    @IdRol INT,
    @Respuesta BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DELETE FROM roles WHERE id_rol = @IdRol;

        IF @@ROWCOUNT > 0
        BEGIN
            SET @Respuesta = 1;
            SET @Mensaje = 'Rol eliminado correctamente.';
        END
        ELSE
        BEGIN
            SET @Respuesta = 0;
            SET @Mensaje = 'No se encontro el rol indicado.';
        END
    END TRY
    BEGIN CATCH
        SET @Respuesta = 0;
        SET @Mensaje = 'No se pudo eliminar (posiblemente tiene usuarios asignados): ' + ERROR_MESSAGE();
    END CATCH
END
GO

/*====================
  USUARIOS
====================*/
IF OBJECT_ID('SP_REGISTRARUSUARIO', 'P') IS NOT NULL DROP PROCEDURE SP_REGISTRARUSUARIO;
GO
CREATE PROCEDURE SP_REGISTRARUSUARIO
    @NombreUsuario VARCHAR(200),
    @TelefonoUsuario VARCHAR(25),
    @CorreoUsuario VARCHAR(100),
    @ContrasenaUsuario VARCHAR(100),
    @IdRol INT,
    @EstadoUsuario INT,
    @NacimientoUsuario DATE,
    @IdUsuarioResultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO usuarios (nombre_usuario, telefono_usuario, correo_usuario, contrasena_usuario, id_rol, estado_usuario, nacimiento_usuario)
        VALUES (@NombreUsuario, @TelefonoUsuario, @CorreoUsuario, @ContrasenaUsuario, @IdRol, @EstadoUsuario, @NacimientoUsuario);

        SET @IdUsuarioResultado = CAST(SCOPE_IDENTITY() AS INT);
        SET @Mensaje = 'Usuario registrado correctamente.';
    END TRY
    BEGIN CATCH
        SET @IdUsuarioResultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END
GO

IF OBJECT_ID('SP_EDITARUSUARIO', 'P') IS NOT NULL DROP PROCEDURE SP_EDITARUSUARIO;
GO
CREATE PROCEDURE SP_EDITARUSUARIO
    @IdUsuario INT,
    @NombreUsuario VARCHAR(200),
    @TelefonoUsuario VARCHAR(25),
    @CorreoUsuario VARCHAR(100),
    @ContrasenaUsuario VARCHAR(100),
    @IdRol INT,
    @EstadoUsuario INT,
    @NacimientoUsuario DATE,
    @Respuesta BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE usuarios
        SET nombre_usuario = @NombreUsuario,
            telefono_usuario = @TelefonoUsuario,
            correo_usuario = @CorreoUsuario,
            contrasena_usuario = @ContrasenaUsuario,
            id_rol = @IdRol,
            estado_usuario = @EstadoUsuario,
            nacimiento_usuario = @NacimientoUsuario
        WHERE id_usuario = @IdUsuario;

        IF @@ROWCOUNT > 0
        BEGIN
            SET @Respuesta = 1;
            SET @Mensaje = 'Usuario actualizado correctamente.';
        END
        ELSE
        BEGIN
            SET @Respuesta = 0;
            SET @Mensaje = 'No se encontro el usuario indicado.';
        END
    END TRY
    BEGIN CATCH
        SET @Respuesta = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END
GO

IF OBJECT_ID('SP_ELIMINARUSUARIO', 'P') IS NOT NULL DROP PROCEDURE SP_ELIMINARUSUARIO;
GO
CREATE PROCEDURE SP_ELIMINARUSUARIO
    @IdUsuario INT,
    @Respuesta BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DELETE FROM usuarios WHERE id_usuario = @IdUsuario;

        IF @@ROWCOUNT > 0
        BEGIN
            SET @Respuesta = 1;
            SET @Mensaje = 'Usuario eliminado correctamente.';
        END
        ELSE
        BEGIN
            SET @Respuesta = 0;
            SET @Mensaje = 'No se encontro el usuario indicado.';
        END
    END TRY
    BEGIN CATCH
        SET @Respuesta = 0;
        SET @Mensaje = 'No se pudo eliminar (posiblemente tiene citas asignadas): ' + ERROR_MESSAGE();
    END CATCH
END
GO

/*====================
  EVENTOS
====================*/
IF OBJECT_ID('SP_REGISTRAREVENTO', 'P') IS NOT NULL DROP PROCEDURE SP_REGISTRAREVENTO;
GO
CREATE PROCEDURE SP_REGISTRAREVENTO
    @NombreEvento VARCHAR(100),
    @FechaProgramadaEvento DATETIME,
    @FechaLimiteEvento DATETIME,
    @DescripcionEvento VARCHAR(150),
    @EstadoEvento INT,
    @IdEventoResultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO eventos (nombre_evento, fechaprogramada_evento, descripcion_evento, estado_evento, fechalimite_evento)
        VALUES (@NombreEvento, @FechaProgramadaEvento, @DescripcionEvento, @EstadoEvento, @FechaLimiteEvento);

        SET @IdEventoResultado = CAST(SCOPE_IDENTITY() AS INT);
        SET @Mensaje = 'Evento registrado correctamente.';
    END TRY
    BEGIN CATCH
        SET @IdEventoResultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END
GO

IF OBJECT_ID('SP_EDITAREVENTO', 'P') IS NOT NULL DROP PROCEDURE SP_EDITAREVENTO;
GO
CREATE PROCEDURE SP_EDITAREVENTO
    @IdEvento INT,
    @NombreEvento VARCHAR(100),
    @FechaProgramadaEvento DATETIME,
    @FechaLimiteEvento DATETIME,
    @DescripcionEvento VARCHAR(150),
    @EstadoEvento INT,
    @Respuesta BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE eventos
        SET nombre_evento = @NombreEvento,
            fechaprogramada_evento = @FechaProgramadaEvento,
            fechalimite_evento = @FechaLimiteEvento,
            descripcion_evento = @DescripcionEvento,
            estado_evento = @EstadoEvento
        WHERE id_evento = @IdEvento;

        IF @@ROWCOUNT > 0
        BEGIN
            SET @Respuesta = 1;
            SET @Mensaje = 'Evento actualizado correctamente.';
        END
        ELSE
        BEGIN
            SET @Respuesta = 0;
            SET @Mensaje = 'No se encontro el evento indicado.';
        END
    END TRY
    BEGIN CATCH
        SET @Respuesta = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END
GO

IF OBJECT_ID('SP_ELIMINAREVENTO', 'P') IS NOT NULL DROP PROCEDURE SP_ELIMINAREVENTO;
GO
CREATE PROCEDURE SP_ELIMINAREVENTO
    @IdEvento INT,
    @Respuesta BIT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DELETE FROM eventos WHERE id_evento = @IdEvento;

        IF @@ROWCOUNT > 0
        BEGIN
            SET @Respuesta = 1;
            SET @Mensaje = 'Evento eliminado correctamente.';
        END
        ELSE
        BEGIN
            SET @Respuesta = 0;
            SET @Mensaje = 'No se encontro el evento indicado.';
        END
    END TRY
    BEGIN CATCH
        SET @Respuesta = 0;
        SET @Mensaje = 'No se pudo eliminar (posiblemente tiene asistentes registrados): ' + ERROR_MESSAGE();
    END CATCH
END
GO

/*====================
  DETALLE_EVENTOS
  (CD_DetalleEvento.cs usa una convencion de nombres distinta:
   minusculas con guion bajo, se respeta tal cual)
====================*/
IF OBJECT_ID('SP_REGISTRAR_DETALLE_EVENTO', 'P') IS NOT NULL DROP PROCEDURE SP_REGISTRAR_DETALLE_EVENTO;
GO
CREATE PROCEDURE SP_REGISTRAR_DETALLE_EVENTO
    @IdUsuario INT,
    @IdEvento INT,
    @Rol VARCHAR(50),
    @IdResultado INT OUTPUT,
    @Mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO detalle_eventos (id_usuario, id_evento, rol_en_evento)
        VALUES (@IdUsuario, @IdEvento, @Rol);

        SET @IdResultado = CAST(SCOPE_IDENTITY() AS INT);
        SET @Mensaje = 'Detalle de evento registrado correctamente.';
    END TRY
    BEGIN CATCH
        SET @IdResultado = 0;
        SET @Mensaje = ERROR_MESSAGE();
    END CATCH
END
GO

IF OBJECT_ID('SP_ELIMINAR_DETALLE_EVENTO', 'P') IS NOT NULL DROP PROCEDURE SP_ELIMINAR_DETALLE_EVENTO;
GO
CREATE PROCEDURE SP_ELIMINAR_DETALLE_EVENTO
    @id_detalle_evento INT,
    @resultado BIT OUTPUT,
    @mensaje VARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DELETE FROM detalle_eventos WHERE id_detalle_evento = @id_detalle_evento;

        IF @@ROWCOUNT > 0
        BEGIN
            SET @resultado = 1;
            SET @mensaje = 'Detalle de evento eliminado correctamente.';
        END
        ELSE
        BEGIN
            SET @resultado = 0;
            SET @mensaje = 'No se encontro el detalle de evento indicado.';
        END
    END TRY
    BEGIN CATCH
        SET @resultado = 0;
        SET @mensaje = 'No se pudo eliminar: ' + ERROR_MESSAGE();
    END CATCH
END
GO
