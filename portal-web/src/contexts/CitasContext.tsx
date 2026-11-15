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

export type ParticipanteCita = {
  idDetalleEvento: number
  idUsuario: number
  nombre: string
  qr: EstadoQr
  qrToken: string
  historial: AccesoCita[]
}

export type Cita = {
  id: number
  fecha: string
  hora: string
  horaFin: string
  tipo: string
  estado: EstadoCita
  participantes: ParticipanteCita[]
}

export type UsuarioAcceso = { id: number; nombre: string }

type EventoApi = {
  idEvento: number
  tipo: string
  fecha: string
  hora: string
  horaFin: string
  estado: EstadoCita
  participantes: {
    idDetalleEvento: number
    idUsuario: number
    nombreUsuario: string
    estadoQr: EstadoQr
    codigoQr: string | null
    historial: AccesoCita[]
  }[]
}

type UsuarioApi = { idUsuario: number; nombreUsuario: string; estadoUsuario: number }

type CitasContextValue = {
  citas: Cita[]
  usuarios: UsuarioAcceso[]
  cargando: boolean
  error: string
  agregarCita: (cita: Omit<Cita, 'id' | 'participantes'>, idUsuarios: number[]) => Promise<void>
  actualizarCita: (citaActualizada: Cita) => Promise<void>
  eliminarCita: (id: number) => Promise<void>
  agregarParticipante: (idEvento: number, idUsuario: number) => Promise<void>
  eliminarParticipante: (idEvento: number, idDetalleEvento: number) => Promise<void>
  generarQr: (idDetalleEvento: number) => Promise<string>
}

const apiUrl = import.meta.env.VITE_API_URL || 'http://localhost:5000'

function horaConSegundos(hora: string) {
  return hora.length === 5 ? `${hora}:00` : hora
}

async function leerError(respuesta: Response, mensaje: string) {
  try {
    const cuerpo = await respuesta.json() as { mensaje?: string; title?: string; errors?: Record<string, string[]> }
    const detalleValidacion = cuerpo.errors
      ? Object.values(cuerpo.errors).flat().join(' ')
      : ''
    return cuerpo.mensaje ?? (detalleValidacion || cuerpo.title || `${mensaje} (HTTP ${respuesta.status})`)
  } catch {
    return `${mensaje} (HTTP ${respuesta.status})`
  }
}

function adaptarEvento(evento: EventoApi): Cita {
  return {
    id: evento.idEvento,
    fecha: evento.fecha,
    hora: evento.hora,
    horaFin: evento.horaFin,
    tipo: evento.tipo,
    estado: evento.estado,
    participantes: evento.participantes.map((participante) => ({
      idDetalleEvento: participante.idDetalleEvento,
      idUsuario: participante.idUsuario,
      nombre: participante.nombreUsuario,
      qr: participante.estadoQr,
      qrToken: participante.codigoQr ?? '',
      historial: participante.historial ?? [],
    })),
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

  async function agregarCita(nuevaCita: Omit<Cita, 'id' | 'participantes'>, idUsuarios: number[]) {
    const respuesta = await fetch(`${apiUrl}/api/eventos`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        idUsuarios,
        tipo: nuevaCita.tipo,
        fecha: nuevaCita.fecha,
        horaInicio: horaConSegundos(nuevaCita.hora),
        horaFin: horaConSegundos(nuevaCita.horaFin),
      }),
    })
    if (!respuesta.ok) throw new Error(await leerError(respuesta, 'No se pudo guardar el evento.'))
    const evento = await respuesta.json() as EventoApi
    setCitas((actuales) => [adaptarEvento(evento), ...actuales])
  }

  async function actualizarCita(citaActualizada: Cita) {
    const respuesta = await fetch(`${apiUrl}/api/eventos/${citaActualizada.id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        tipo: citaActualizada.tipo,
        fecha: citaActualizada.fecha,
        horaInicio: horaConSegundos(citaActualizada.hora),
        horaFin: horaConSegundos(citaActualizada.horaFin),
        estado: citaActualizada.estado,
      }),
    })
    if (!respuesta.ok) throw new Error(await leerError(respuesta, 'No se pudo actualizar el evento.'))
    setCitas((actuales) => actuales.map((cita) => cita.id === citaActualizada.id
      ? citaActualizada
      : cita))
  }

  async function eliminarCita(id: number) {
    const respuesta = await fetch(`${apiUrl}/api/eventos/${id}`, { method: 'DELETE' })
    if (!respuesta.ok) throw new Error(await leerError(respuesta, 'No se pudo eliminar el acceso.'))
    setCitas((actuales) => actuales.filter((cita) => cita.id !== id))
  }

  async function agregarParticipante(idEvento: number, idUsuario: number) {
    const respuesta = await fetch(`${apiUrl}/api/eventos/${idEvento}/participantes`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ idUsuario }),
    })
    if (!respuesta.ok) throw new Error(await leerError(respuesta, 'No se pudo agregar al participante.'))
    const evento = await respuesta.json() as EventoApi
    setCitas((actuales) => actuales.map((cita) => cita.id === idEvento ? adaptarEvento(evento) : cita))
  }

  async function eliminarParticipante(idEvento: number, idDetalleEvento: number) {
    const respuesta = await fetch(`${apiUrl}/api/eventos/${idEvento}/participantes/${idDetalleEvento}`, {
      method: 'DELETE',
    })
    if (!respuesta.ok) throw new Error(await leerError(respuesta, 'No se pudo quitar al participante.'))
    setCitas((actuales) => actuales.map((cita) => cita.id === idEvento
      ? { ...cita, participantes: cita.participantes.filter((participante) => participante.idDetalleEvento !== idDetalleEvento) }
      : cita))
  }

  async function generarQr(idDetalleEvento: number): Promise<string> {
    const respuesta = await fetch(`${apiUrl}/api/acceso/generar-qr`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ idDetalleEvento }),
    })
  if (!respuesta.ok) throw new Error(await leerError(respuesta, 'No se pudo generar el código QR.'))
  const resultado = await respuesta.json() as { codigoQr: string }
    setCitas((actuales) => actuales.map((cita) => ({
      ...cita,
      participantes: cita.participantes.map((participante) => participante.idDetalleEvento === idDetalleEvento
        ? { ...participante, qr: 'Generado', qrToken: resultado.codigoQr }
        : participante),
    })))
  return resultado.codigoQr
  }

  return (
    <CitasContext.Provider value={{ citas, usuarios, cargando, error, agregarCita, actualizarCita, eliminarCita, agregarParticipante, eliminarParticipante, generarQr }}>
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