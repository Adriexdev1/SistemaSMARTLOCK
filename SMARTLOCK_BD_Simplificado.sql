
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
    pin_usuario VARCHAR(100) NULL,
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
    acceso_detalle_evento DATETIME,
    codigo_qr VARCHAR(100) NULL,
    qr_usado BIT NOT NULL DEFAULT 0,
    metodo_acceso VARCHAR(20) NULL
);
GO

CREATE TABLE incidentes (
    id_incidente INT IDENTITY(1,1) PRIMARY KEY,
    tipo_incidente VARCHAR(80) NOT NULL,
    descripcion_incidente VARCHAR(500) NOT NULL,
    prioridad_incidente VARCHAR(20) NOT NULL
        CHECK (prioridad_incidente IN ('Crítica', 'Alta', 'Media', 'Baja')),
    estado_incidente VARCHAR(20) NOT NULL
        CHECK (estado_incidente IN ('Pendiente', 'En progreso', 'Resuelto')),
    fecha_incidente DATE NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    hora_incidente TIME NOT NULL DEFAULT CONVERT(TIME, GETDATE()),
    id_usuario_registro INT NULL,
    id_evento INT NULL,
    id_usuario_asignado INT NULL,
    fecha_creacion DATETIME NOT NULL DEFAULT GETDATE(),
    fecha_actualizacion DATETIME NULL,
    observacion_cierre VARCHAR(500) NULL
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

ALTER TABLE incidentes
ADD CONSTRAINT FK_incidentes_usuario_registro
FOREIGN KEY (id_usuario_registro) REFERENCES usuarios(id_usuario);

ALTER TABLE incidentes
ADD CONSTRAINT FK_incidentes_evento
FOREIGN KEY (id_evento) REFERENCES eventos(id_evento);

ALTER TABLE incidentes
ADD CONSTRAINT FK_incidentes_usuario_asignado
FOREIGN KEY (id_usuario_asignado) REFERENCES usuarios(id_usuario);
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
