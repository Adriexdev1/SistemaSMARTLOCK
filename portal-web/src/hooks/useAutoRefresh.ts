import { useEffect, useRef } from 'react'

export function useAutoRefresh(
  actualizar: () => Promise<void> | void,
  intervaloMs = 10_000,
  habilitado = true,
) {
  const actualizarRef = useRef(actualizar)
  const actualizacionEnCurso = useRef(false)

  useEffect(() => {
    actualizarRef.current = actualizar
  }, [actualizar])

  useEffect(() => {
    if (!habilitado) return

    let activo = true
    async function ejecutarActualizacion() {
      if (!activo || document.visibilityState === 'hidden' || actualizacionEnCurso.current)
        return

      actualizacionEnCurso.current = true
      try {
        await actualizarRef.current()
      } catch (error) {
        console.error('Falló la actualización automática de datos.', error)
      } finally {
        actualizacionEnCurso.current = false
      }
    }

    function actualizarAlVolver() {
      if (document.visibilityState === 'visible')
        void ejecutarActualizacion()
    }

    void ejecutarActualizacion()
    const temporizador = window.setInterval(() => {
      void ejecutarActualizacion()
    }, intervaloMs)
    document.addEventListener('visibilitychange', actualizarAlVolver)

    return () => {
      activo = false
      window.clearInterval(temporizador)
      document.removeEventListener('visibilitychange', actualizarAlVolver)
    }
  }, [habilitado, intervaloMs])
}
