//Importacion de componentes internos reutilizables
import CartaEstadistica from '../components/componentesPaginas/CartaEstadistica'
import ProximasVisitas from '../components/componentesPaginas/ProximasVisitas'
import ActividadReciente from '../components/componentesPaginas/ActividadReciente'
import AccesoRapido from '../components/componentesPaginas/AccesoRapido'
import { AlertTriangle, CalendarDays, CircleCheck, Users } from 'lucide-react'
import { obtenerUsuarioSesion } from '../utils/usuarioSesion'
import { useEffect, useState } from 'react'

type UsuarioInicioApi = { estadoUsuario: number }
type EventoInicioApi = {
  id: number
  fecha: string
  hora: string
  horaFin: string
  visitante: string
  tipo: string
  estado: string
}
type HistorialInicioApi = { fecha: string; hora: string; usuario: string; resultado: string }
type IncidenteInicioApi = { fecha: string; hora: string; tipo: string; estado: string; descripcion: string }
type Actividad = { texto: string; hora: string; fechaHora: string; color: string }

const apiUrl = import.meta.env.VITE_API_URL || 'http://localhost:5000'

export default function Inicio() {
  const usuario = obtenerUsuarioSesion()
  const nombreVisible = usuario?.nombreUsuario.split(/\s+/)[0] ?? 'Usuario'
  const [totalUsuarios, setTotalUsuarios] = useState(0)
  const [accesosHoy, setAccesosHoy] = useState(0)
  const [autorizadosHoy, setAutorizadosHoy] = useState(0)
  const [incidentesPendientes, setIncidentesPendientes] = useState(0)
  const [visitas, setVisitas] = useState<{ hora: string; nombre: string; detalle: string; estado: string }[]>([])
  const [actividades, setActividades] = useState<Actividad[]>([])
  const [cargando, setCargando] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    async function cargarResumen() {
      setCargando(true)
      setError('')
      try {
        const [respuestaUsuarios, respuestaEventos, respuestaHistorial, respuestaIncidentes] = await Promise.all([
          fetch(`${apiUrl}/api/usuarios`),
          fetch(`${apiUrl}/api/eventos`),
          fetch(`${apiUrl}/api/historial`),
          fetch(`${apiUrl}/api/incidentes`),
        ])
        if ([respuestaUsuarios, respuestaEventos, respuestaHistorial, respuestaIncidentes].some((respuesta) => !respuesta.ok)) {
          throw new Error('No se pudo cargar el resumen del portal.')
        }

        const [usuarios, eventos, historial, incidentes] = await Promise.all([
          respuestaUsuarios.json() as Promise<UsuarioInicioApi[]>,
          respuestaEventos.json() as Promise<EventoInicioApi[]>,
          respuestaHistorial.json() as Promise<HistorialInicioApi[]>,
          respuestaIncidentes.json() as Promise<IncidenteInicioApi[]>,
        ])

        const hoy = new Date().toISOString().slice(0, 10)
        setTotalUsuarios(usuarios.filter((registro) => registro.estadoUsuario === 1).length)
        setAccesosHoy(eventos.filter((registro) => registro.fecha === hoy && registro.estado === 'Activo').length)
        setAutorizadosHoy(historial.filter((registro) => registro.fecha === hoy && registro.resultado === 'Autorizado').length)
        setIncidentesPendientes(incidentes.filter((registro) => registro.estado === 'Pendiente').length)

        setVisitas(eventos
          .filter((registro) => registro.fecha >= hoy && registro.estado === 'Activo')
          .sort((a, b) => `${a.fecha}T${a.hora}`.localeCompare(`${b.fecha}T${b.hora}`))
          .slice(0, 4)
          .map((registro) => ({
            hora: registro.fecha === hoy ? registro.hora : `${registro.fecha} ${registro.hora}`,
            nombre: registro.visitante,
            detalle: registro.tipo,
            estado: registro.estado,
          })))

        const actividadHistorial = historial.map((registro) => ({
          texto: `Acceso ${registro.resultado.toLowerCase()} - ${registro.usuario}`,
          hora: registro.hora,
          fechaHora: `${registro.fecha}T${registro.hora}`,
          color: 'bg-emerald-500',
        }))
        const actividadIncidentes = incidentes.map((registro) => ({
          texto: `Incidente ${registro.estado.toLowerCase()} - ${registro.tipo}: ${registro.descripcion}`,
          hora: registro.hora,
          fechaHora: `${registro.fecha}T${registro.hora}`,
          color: 'bg-orange-500',
        }))
        setActividades([...actividadHistorial, ...actividadIncidentes]
          .sort((a, b) => b.fechaHora.localeCompare(a.fechaHora))
          .slice(0, 5))
      } catch (errorCarga) {
        setError(errorCarga instanceof Error ? errorCarga.message : 'No se pudo conectar con la API.')
      } finally {
        setCargando(false)
      }
    }

    void cargarResumen()
  }, [])

  return (
    <div>
      {/*Seccion superior*/}
      <section>
        <h1 className="text-lg font-semibold text-slate-800">Bienvenido, {nombreVisible}</h1>
        <p className="text-sm text-slate-400 sm:text-base">{usuario?.rol ?? 'Sin rol asignado'} · {usuario?.correoUsuario ?? ''}</p>
      </section>

      {/*Seccion de tarjetas con informacion relevante*/}
      {/*Las tarjetas pasan de una columna en móvil a cuatro en pantallas amplias*/}
      {error && <p role="alert" className="mt-4 rounded-md border border-rose-200 bg-rose-50 px-4 py-3 text-sm text-rose-700">{error}</p>}
      {cargando && <p role="status" className="py-4 text-sm text-slate-500">Actualizando resumen...</p>}
      <section className="grid grid-cols-1 gap-4 py-5 sm:grid-cols-2 xl:grid-cols-4">
        <CartaEstadistica icon={Users} iconClassName="bg-blue-500" title="Empleados" value={String(totalUsuarios)} subtitle="Activos"/>
        <CartaEstadistica icon={CalendarDays} iconClassName="bg-emerald-500" title="Accesos hoy" value={String(accesosHoy)} subtitle="Programados"/>
        <CartaEstadistica icon={CircleCheck} iconClassName="bg-amber-500" title="Autorizados" value={String(autorizadosHoy)} subtitle="Hoy"/>
        <CartaEstadistica icon={AlertTriangle} iconClassName="bg-rose-500" title="Incidentes" value={String(incidentesPendientes)} subtitle="Pendientes"/>
      </section>

      {/*Los paneles se apilan en teléfono y quedan lado a lado desde xl*/}
      <section className="grid grid-cols-1 gap-4 xl:grid-cols-2">
        <ActividadReciente actividades={actividades} cargando={cargando}/>
        <ProximasVisitas visitas={visitas} cargando={cargando}/>
      </section>
      <AccesoRapido/>
    </div>
  )
}