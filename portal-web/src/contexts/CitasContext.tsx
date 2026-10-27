import { createContext, useContext, useEffect, useState } from 'react'
import { Outlet } from 'react-router-dom'

export type EstadoCita = 'Activo' | 'Completado'
export type EstadoQr = 'Generado' | 'Pendiente' | 'Utilizado'

export type AccesoCita = {
  id: number
  fecha: string
  hora: string
  resultado: string
}

export type Cita = {
  id: number
  idUsuario: number
  fecha: string
  hora: string
  horaFin: string
  visitante: string
  tipo: string
  estado: EstadoCita
  qr: EstadoQr
  qrToken: string
  historial: AccesoCita[]
}

export type UsuarioAcceso = { id: number; nombre: string }

type EventoApi = {
  id: number
  idDetalleEvento: number
  idUsuario: number
  visitante: string
  tipo: string
  fecha: string
  hora: string
  horaFin: string
  estado: EstadoCita
  estadoQr: EstadoQr
  codigoQr: string | null
  historial: AccesoCita[]
}

type UsuarioApi = { idUsuario: number; nombreUsuario: string; estadoUsuario: number }

type CitasContextValue = {
  citas: Cita[]
  usuarios: UsuarioAcceso[]
  cargando: boolean
  error: string
  agregarCita: (cita: Omit<Cita, 'id' | 'qr' | 'qrToken' | 'historial'>) => Promise<void>
  actualizarCita: (citaActualizada: Cita) => Promise<void>
  eliminarCita: (id: number) => Promise<void>
    generarQr: (id: number) => Promise<string>
}

const apiUrl = import.meta.env.VITE_API_URL || 'http://localhost:5000'

function adaptarEvento(evento: EventoApi): Cita {
  return {
    id: evento.idDetalleEvento,
    idUsuario: evento.idUsuario,
    fecha: evento.fecha,
    hora: evento.hora,
    horaFin: evento.horaFin,
    visitante: evento.visitante,
    tipo: evento.tipo,
    estado: evento.estado,
    qr: evento.estadoQr,
    qrToken: evento.codigoQr ?? '',
    historial: evento.historial ?? [],
  }
}

const CitasContext = createContext<CitasContextValue | null>(null)

export function CitasProvider() {
  const [citas, setCitas] = useState<Cita[]>([])
  const [usuarios, setUsuarios] = useState<UsuarioAcceso[]>([])
  const [cargando, setCargando] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    async function cargarDatos() {
      setCargando(true)
      setError('')
      try {
        const [respuestaEventos, respuestaUsuarios] = await Promise.all([
          fetch(`${apiUrl}/api/eventos`),
          fetch(`${apiUrl}/api/usuarios`),
        ])
        if (!respuestaEventos.ok || !respuestaUsuarios.ok) {
          throw new Error('No se pudieron cargar los accesos y usuarios.')
        }

        const [eventos, usuariosApi] = await Promise.all([
          respuestaEventos.json() as Promise<EventoApi[]>,
          respuestaUsuarios.json() as Promise<UsuarioApi[]>,
        ])
        setCitas(eventos.map(adaptarEvento))
        setUsuarios(usuariosApi
          .filter((usuario) => usuario.estadoUsuario === 1)
          .map((usuario) => ({ id: usuario.idUsuario, nombre: usuario.nombreUsuario })))
      } catch (errorCarga) {
        setError(errorCarga instanceof Error ? errorCarga.message : 'No se pudo conectar con la API.')
      } finally {
        setCargando(false)
      }
    }

    void cargarDatos()
  }, [])

  async function agregarCita(nuevaCita: Omit<Cita, 'id' | 'qr' | 'qrToken' | 'historial'>) {
    const respuesta = await fetch(`${apiUrl}/api/eventos`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        idUsuario: nuevaCita.idUsuario,
        tipo: nuevaCita.tipo,
        fecha: nuevaCita.fecha,
        horaInicio: nuevaCita.hora,
        horaFin: nuevaCita.horaFin,
      }),
    })
    if (!respuesta.ok) throw new Error('No se pudo guardar el acceso.')
    const evento = await respuesta.json() as EventoApi
    setCitas((actuales) => [adaptarEvento(evento), ...actuales])
  }

  async function actualizarCita(citaActualizada: Cita) {
    const respuesta = await fetch(`${apiUrl}/api/eventos/${citaActualizada.id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        idUsuario: citaActualizada.idUsuario,
        tipo: citaActualizada.tipo,
        fecha: citaActualizada.fecha,
        horaInicio: citaActualizada.hora,
        horaFin: citaActualizada.horaFin,
        estado: citaActualizada.estado,
      }),
    })
    if (!respuesta.ok) throw new Error('No se pudo actualizar el acceso.')
    setCitas((actuales) => actuales.map((cita) => cita.id === citaActualizada.id
      ? { ...citaActualizada, qr: 'Pendiente', qrToken: '' }
      : cita))
  }

  async function eliminarCita(id: number) {
    const respuesta = await fetch(`${apiUrl}/api/eventos/${id}`, { method: 'DELETE' })
    if (!respuesta.ok) throw new Error('No se pudo eliminar el acceso.')
    setCitas((actuales) => actuales.filter((cita) => cita.id !== id))
  }

  async function generarQr(id: number) {
    const respuesta = await fetch(`${apiUrl}/api/acceso/generar-qr`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ idDetalleEvento: id }),
    })
  if (!respuesta.ok) throw new Error('No se pudo generar el código QR.')
  const resultado = await respuesta.json() as { codigoQr: string }
    setCitas((actuales) => actuales.map((cita) => cita.id === id
      ? { ...cita, qr: 'Generado', qrToken: resultado.codigoQr }
      : cita))
  return resultado.codigoQr
  }

  return (
    <CitasContext.Provider value={{ citas, usuarios, cargando, error, agregarCita, actualizarCita, eliminarCita, generarQr }}>
      <Outlet />
    </CitasContext.Provider>
  )
}

export function useCitas() {
  const contexto = useContext(CitasContext)
  if (!contexto) {
    throw new Error('useCitas debe usarse dentro de CitasProvider')
  }
  return contexto
}