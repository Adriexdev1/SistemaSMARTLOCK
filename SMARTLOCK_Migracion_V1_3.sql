/*=====================================================================
  SMARTLOCK_BD - Migracion V1.3
  Objetivo: reordenar el flujo QR -> PIN -> consumir, para que
  coincida con el diagrama de flujo de validacion de acceso
  definido por el equipo (el QR solo se invalida DESPUES de que el
  PIN es correcto, no antes).

  Reemplaza SP_VALIDAR_Y_CONSUMIR_QR (que validaba y consumia en un
  solo paso) por dos procedimientos separados:
    - SP_VALIDAR_QR: solo lectura, no modifica nada.
    - SP_CONSUMIR_QR: atomico, marca el QR como usado (se llama
      unicamente despues de que el PIN fue verificado correcto).

  Ejecutar DESPUES de SMARTLOCK_Migracion_V1_2.sql
=====================================================================*/

USE SMARTLOCK_BD;
GO

IF OBJECT_ID('SP_VALIDAR_Y_CONSUMIR_QR', 'P') IS NOT NULL
    DROP PROCEDURE SP_VALIDAR_Y_CONSUMIR_QR;
GO

/*====================
  SP: VALIDAR QR (SOLO LECTURA)
  Revisa que el QR exista, no este usado, el evento este activo y
  no haya pasado la fecha limite. NO modifica nada. Se llama antes
  de pedir el PIN.
====================*/
IF OBJECT_ID('SP_VALIDAR_QR', 'P') IS NOT NULL
    DROP PROCEDURE SP_VALIDAR_QR;
GO
CREATE PROCEDURE SP_VALIDAR_QR
    @codigo_qr VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        de.id_detalle_evento,
        de.id_usuario,
        de.id_evento,
        u.nombre_usuario,
        e.nombre_evento
    FROM detalle_eventos de
        INNER JOIN eventos e ON e.id_evento = de.id_evento
        INNER JOIN usuarios u ON u.id_usuario = de.id_usuario
    WHERE de.codigo_qr = @codigo_qr
      AND de.qr_usado = 0
      AND e.estado_evento = 1
      AND (e.fechalimite_evento IS NULL OR e.fechalimite_evento > GETDATE());
END
GO

/*====================
  SP: CONSUMIR QR (ATOMICO)
  Marca el QR como usado, revalidando las mismas condiciones en
  este mismo instante (para evitar que dos solicitudes al mismo
  tiempo consuman el mismo QR). Se llama SOLO despues de que el
  PIN fue verificado correcto (o si el usuario no tiene PIN
  configurado).
====================*/
IF OBJECT_ID('SP_CONSUMIR_QR', 'P') IS NOT NULL
    DROP PROCEDURE SP_CONSUMIR_QR;
GO
CREATE PROCEDURE SP_CONSUMIR_QR
    @codigo_qr VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE de
    SET de.qr_usado = 1,
        de.acceso_detalle_evento = GETDATE()
    OUTPUT inserted.id_detalle_evento
    FROM detalle_eventos de
        INNER JOIN eventos e ON e.id_evento = de.id_evento
    WHERE de.codigo_qr = @codigo_qr
      AND de.qr_usado = 0
      AND e.estado_evento = 1
      AND (e.fechalimite_evento IS NULL OR e.fechalimite_evento > GETDATE());
END
GO
