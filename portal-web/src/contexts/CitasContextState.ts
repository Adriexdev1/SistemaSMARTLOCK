import { createContext, useContext } from 'react'

export type EstadoCita = 'Activo' | 'Inactivo'
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
  fechaFin: string
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
  fechaFin: string
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

export type CitasContextValue = {
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

export const CitasContext = createContext<CitasContextValue | null>(null)

export function useCitas() {
  const contexto = useContext(CitasContext)
  if (!contexto) {
    throw new Error('useCitas debe usarse dentro de CitasProvider')
  }
  return contexto
}

export type { EventoApi, UsuarioApi }
