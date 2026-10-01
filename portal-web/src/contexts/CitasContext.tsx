import { createContext, useContext, useState } from 'react'
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

type CitasContextValue = {
  citas: Cita[]
  agregarCita: (cita: Omit<Cita, 'id'>) => void
  actualizarCita: (citaActualizada: Cita) => void
  eliminarCita: (id: number) => void
}

const citasIniciales: Cita[] = [
  {
    id: 1,
    fecha: '2026-08-28',
    hora: '08:00',
    horaFin: '16:00',
    visitante: 'Carlos Mendoza Ruiz',
    tipo: 'Turno matutino',
    estado: 'Activo',
    qr: 'Generado',
    qrToken: 'SMARTLOCK-DEMO-ACCESS-1',
    historial: [],
  },
  {
    id: 2,
    fecha: '2026-08-28',
    hora: '09:30',
    horaFin: '12:00',
    visitante: 'Grupo Logística NL',
    tipo: 'Visita proveedor',
    estado: 'Activo',
    qr: 'Pendiente',
    qrToken: 'SMARTLOCK-DEMO-ACCESS-2',
    historial: [],
  },
]

const CitasContext = createContext<CitasContextValue | null>(null)

export function CitasProvider() {
  const [citas, setCitas] = useState(citasIniciales)

  function agregarCita(nuevaCita: Omit<Cita, 'id'>){
    setCitas((actuales) => [
        ...actuales,
        {
            ...nuevaCita,
            id: Math.max(0, ...actuales.map((cita) => cita.id)) + 1,
        },
    ])
  }

  function actualizarCita(citaActualizada: Cita) {
    setCitas((actuales) => actuales.map((cita) =>
      cita.id === citaActualizada.id ? citaActualizada : cita
    ))
  }

  function eliminarCita(id: number) {
    setCitas((actuales) => actuales.filter((cita) => cita.id !== id))
  }

  return (
    <CitasContext.Provider value={{ citas, agregarCita, actualizarCita, eliminarCita }}>
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