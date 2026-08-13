
CREATE DATABASE SMARTLOCK_BD;
GO
USE SMARTLOCK_BD;
GO

/*====================
TABLAS
====================*/

CREATE TABLE roles(
    id_rol INT IDENTITY(1,1) PRIMARY KEY,
    nombre_rol VARCHAR(50) NOT NULL,
    descripcion_rol VARCHAR(150),
    fecha_rol DATETIME DEFAULT GETDATE(),
    estado_rol INT,
    permiso_rol INT
);
GO

CREATE TABLE usuarios(
    id_usuario INT IDENTITY(1,1) PRIMARY KEY,
    nombre_usuario VARCHAR(200) NOT NULL,
    telefono_usuario VARCHAR(25),
    correo_usuario VARCHAR(100) NOT NULL UNIQUE,
    id_rol INT,
    estado_usuario INT NOT NULL DEFAULT 1,
    fecha_usuario DATETIME DEFAULT GETDATE(),
    contrasena_usuario VARCHAR(100),
    nacimiento_usuario DATE
);
GO

CREATE TABLE eventos(
    id_evento INT IDENTITY(1,1) PRIMARY KEY,
    nombre_evento VARCHAR(100) NOT NULL,
    fechaprogramada_evento DATETIME,
    descripcion_evento VARCHAR(150),
    estado_evento INT NOT NULL,
    fecha_evento DATETIME DEFAULT GETDATE(),
    fechalimite_evento DATETIME
);
GO

CREATE TABLE detalle_eventos(
    id_detalle_evento INT IDENTITY(1,1) PRIMARY KEY,
    id_usuario INT,
    id_evento INT,
    fecha_detalle_evento DATETIME DEFAULT GETDATE(),
    rol_en_evento VARCHAR(50),
    acceso_detalle_evento DATETIME
);
GO

/*====================
RELACIONES
====================*/

ALTER TABLE usuarios
ADD CONSTRAINT FK_usuarios_roles
FOREIGN KEY(id_rol) REFERENCES roles(id_rol);
GO

ALTER TABLE detalle_eventos
ADD CONSTRAINT FK_detalle_usuario
FOREIGN KEY(id_usuario) REFERENCES usuarios(id_usuario);
GO

ALTER TABLE detalle_eventos
ADD CONSTRAINT FK_detalle_evento
FOREIGN KEY(id_evento) REFERENCES eventos(id_evento);
GO

/***********************
PROCEDIMIENTOS INCLUIDOS
************************
SP_REGISTRARROL
SP_EDITARROL
SP_ELIMINARROL

SP_REGISTRARUSUARIO
SP_EDITARUSUARIO
SP_ELIMINARUSUARIO

SP_REGISTRAREVENTO
SP_EDITAREVENTO
SP_ELIMINAREVENTO

SP_REGISTRAR_DETALLE_EVENTO
SP_ELIMINAR_DETALLE_EVENTO
************************/
