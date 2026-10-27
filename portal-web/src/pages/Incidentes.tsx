import { useEffect, useState, type FormEvent } from 'react'
import { Hammer, CircleCheck, Clock3, Eye, Plus, RefreshCw, Siren, X } from 'lucide-react'
import CartaEstadistica from '../components/componentesPaginas/CartaEstadistica'
import DataTable from '../components/TablaBase'
import Paginacion from '../components/PieTabla'
import { obtenerUsuarioSesion } from '../utils/usuarioSesion'

type EstadoIncidente = 'Pendiente' | 'En progreso' | 'Resuelto'
type PrioridadIncidente = 'Crítica' | 'Alta' | 'Media' | 'Baja'
type FiltroIncidentes = 'Todos' | EstadoIncidente

type Incidente = {
    id: number
    fecha: string
    hora: string
    tipo: string
    descripcion: string
    prioridad: PrioridadIncidente
    estado: EstadoIncidente
}

type IncidenteApi = {
    idIncidente: number
    tipo: string
    descripcion: string
    prioridad: PrioridadIncidente
    estado: EstadoIncidente
    fecha: string
    hora: string
}

const apiUrl = import.meta.env.VITE_API_URL || 'http://localhost:5000'

const clasesPrioridad: Record<PrioridadIncidente, string> = {
    Crítica: 'bg-red-600 text-white',
    Alta: 'bg-rose-100 text-rose-700',
    Media: 'bg-amber-100 text-amber-700',
    Baja: 'bg-slate-100 text-slate-600',
}

const clasesEstado: Record<EstadoIncidente, string> = {
    Pendiente: 'bg-amber-100 text-amber-700',
    'En progreso': 'bg-blue-100 text-blue-700',
    Resuelto: 'bg-emerald-100 text-emerald-700',
}

const filtros: FiltroIncidentes[] = ['Todos', 'Pendiente', 'En progreso', 'Resuelto']

function fechaVisible(fecha: string) {
    const [anio, mes, dia] = fecha.split('-')
    return `${dia}/${mes}/${anio}`
}

export default function Incidentes() {
    const [incidentes, setIncidentes] = useState<Incidente[]>([])
    const [cargando, setCargando] = useState(true)
    const [guardando, setGuardando] = useState(false)
    const [errorApi, setErrorApi] = useState('')
    const [filtro, setFiltro] = useState<FiltroIncidentes>('Todos')
    const [paginaActual, setPaginaActual] = useState(1)
    const [modalRegistroAbierto, setModalRegistroAbierto] = useState(false)
    const [incidenteSeleccionado, setIncidenteSeleccionado] = useState<Incidente | null>(null)
    const [nuevoIncidente, setNuevoIncidente] = useState({
        tipo: 'QR inválido',
        descripcion: '',
        prioridad: 'Media' as PrioridadIncidente,
    })

    useEffect(() => {
        async function cargarIncidentes() {
            setCargando(true)
            setErrorApi('')
            try {
                const respuesta = await fetch(`${apiUrl}/api/incidentes`)
                if (!respuesta.ok) throw new Error('No se pudieron cargar los incidentes.')
                const datos = await respuesta.json() as IncidenteApi[]
                setIncidentes(datos.map((incidente) => ({
                    id: incidente.idIncidente,
                    fecha: incidente.fecha,
                    hora: incidente.hora,
                    tipo: incidente.tipo,
                    descripcion: incidente.descripcion,
                    prioridad: incidente.prioridad,
                    estado: incidente.estado,
                })))
            } catch (error) {
                setErrorApi(error instanceof Error ? error.message : 'No se pudo conectar con la API.')
            } finally {
                setCargando(false)
            }
        }

        void cargarIncidentes()
    }, [])

    const tamanoPagina = 5
    const incidentesFiltrados = filtro === 'Todos'
        ? incidentes
        : incidentes.filter((incidente) => incidente.estado === filtro)
    const totalPaginas = Math.max(1, Math.ceil(incidentesFiltrados.length / tamanoPagina))
    const pagina = Math.min(paginaActual, totalPaginas)
    const inicio = (pagina - 1) * tamanoPagina
    const incidentesPagina = incidentesFiltrados.slice(inicio, inicio + tamanoPagina)
    const pendientes = incidentes.filter((incidente) => incidente.estado === 'Pendiente').length
    const enProgreso = incidentes.filter((incidente) => incidente.estado === 'En progreso').length
    const resueltos = incidentes.filter((incidente) => incidente.estado === 'Resuelto').length

    async function avanzarEstado(incidenteId: number) {
        const incidente = incidentes.find((actual) => actual.id === incidenteId)
        if (!incidente) return

        const siguienteEstado: EstadoIncidente = incidente.estado === 'Pendiente'
            ? 'En progreso'
            : 'Resuelto'

        setGuardando(true)
        setErrorApi('')
        try {
            const respuesta = await fetch(`${apiUrl}/api/incidentes/${incidenteId}/estado`, {
                method: 'PATCH',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ estado: siguienteEstado }),
            })
            if (!respuesta.ok) throw new Error('No se pudo actualizar el estado del incidente.')

            setIncidentes((actuales) => actuales.map((actual) =>
                actual.id === incidenteId ? { ...actual, estado: siguienteEstado } : actual
            ))
            setIncidenteSeleccionado((actual) => actual?.id === incidenteId
                ? { ...actual, estado: siguienteEstado }
                : actual)
        } catch (error) {
            setErrorApi(error instanceof Error ? error.message : 'No se pudo conectar con la API.')
        } finally {
            setGuardando(false)
        }
    }

    async function registrarIncidente(evento: FormEvent<HTMLFormElement>) {
        evento.preventDefault()
        const usuario = obtenerUsuarioSesion()
        if (!usuario) {
            setErrorApi('Tu sesión terminó. Inicia sesión nuevamente.')
            return
        }

        setGuardando(true)
        setErrorApi('')
        try {
            const respuesta = await fetch(`${apiUrl}/api/incidentes`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    tipo: nuevoIncidente.tipo,
                    descripcion: nuevoIncidente.descripcion.trim(),
                    prioridad: nuevoIncidente.prioridad,
                    idUsuarioRegistro: usuario.idUsuario,
                }),
            })

            if (!respuesta.ok) throw new Error('No se pudo registrar el incidente.')
            const creado = await respuesta.json() as IncidenteApi
            setIncidentes((actuales) => [{
                id: creado.idIncidente,
                fecha: creado.fecha,
                hora: creado.hora,
                tipo: creado.tipo,
                descripcion: creado.descripcion,
                prioridad: creado.prioridad,
                estado: creado.estado,
            }, ...actuales])
            setFiltro('Todos')
            setPaginaActual(1)
            setNuevoIncidente({ tipo: 'QR inválido', descripcion: '', prioridad: 'Media' })
            setModalRegistroAbierto(false)
        } catch (error) {
            setErrorApi(error instanceof Error ? error.message : 'No se pudo conectar con la API.')
        } finally {
            setGuardando(false)
        }
    }

    const columnasIncidentes = [
        {
            header: 'Fecha',
            width: '12%',
            render: (incidente: Incidente) => (
                <span className="whitespace-nowrap font-mono text-slate-500">{fechaVisible(incidente.fecha)}</span>
            ),
        },
        {
            header: 'Hora',
            width: '8%',
            render: (incidente: Incidente) => (
                <span className="whitespace-nowrap font-mono font-semibold text-slate-700">{incidente.hora}</span>
            ),
        },
        {
            header: 'Tipo',
            width: '13%',
            render: (incidente: Incidente) => <span className="font-medium text-slate-800">{incidente.tipo}</span>,
        },
        {
            header: 'Descripción',
            width: '30%',
            render: (incidente: Incidente) => <span className="block truncate text-slate-600" title={incidente.descripcion}>{incidente.descripcion}</span>,
        },
        {
            header: 'Prioridad',
            width: '12%',
            render: (incidente: Incidente) => (
                <span className={`whitespace-nowrap rounded-full px-2.5 py-1 text-xs font-semibold ${clasesPrioridad[incidente.prioridad]}`}>
                    {incidente.prioridad}
                </span>
            ),
        },
        {
            header: 'Estado',
            width: '13%',
            render: (incidente: Incidente) => (
                <span className={`whitespace-nowrap rounded-full px-2.5 py-1 text-xs font-semibold ${clasesEstado[incidente.estado]}`}>
                    {incidente.estado}
                </span>
            ),
        },
        {
            header: 'Acciones',
            width: '12%',
            render: (incidente: Incidente) => (
                <div className="flex items-center gap-1">
                    <button
                        type="button"
                        aria-label={`Ver incidente ${incidente.id}`}
                        onClick={() => setIncidenteSeleccionado(incidente)}
                        className="flex h-8 w-8 items-center justify-center rounded-md text-slate-500 transition-colors hover:bg-slate-100 hover:text-slate-900 focus-visible:outline-2 focus-visible:outline-slate-400"
                    >
                        <Eye size={16} />
                    </button>
                    {incidente.estado !== 'Resuelto' && (
                        <button
                            type="button"
                            aria-label={`Actualizar estado del incidente ${incidente.id}`}
                            disabled={guardando}
                            onClick={() => void avanzarEstado(incidente.id)}
                            className="flex h-8 w-8 items-center justify-center rounded-md text-slate-500 transition-colors hover:bg-blue-100 hover:text-blue-700 focus-visible:outline-2 focus-visible:outline-blue-400 disabled:cursor-wait disabled:opacity-50"
                        >
                            <RefreshCw size={15} />
                        </button>
                    )}
                </div>
            ),
        },
    ]

    return (
        <div className="mx-auto w-full max-w-[1600px]">
            <section className="mb-5 flex flex-wrap items-center justify-between gap-4">
                <div>
                    <h2 className="text-xl font-semibold text-slate-900">Incidentes</h2>
                    <p className="mt-1 text-sm text-slate-500">Registra y da seguimiento a incidentes del sistema</p>
                </div>
                <button
                    type="button"
                    onClick={() => {
                        setErrorApi('')
                        setModalRegistroAbierto(true)
                    }}
                    disabled={cargando}
                    className="inline-flex items-center gap-2 rounded-lg bg-rose-500 px-4 py-2.5 text-sm font-semibold text-white transition-colors hover:bg-rose-600 disabled:cursor-wait disabled:opacity-60"
                >
                    <Plus size={18} aria-hidden="true" />
                    Registrar incidente
                </button>
            </section>

            {errorApi && <p role="alert" className="mb-4 rounded-md border border-rose-200 bg-rose-50 px-4 py-3 text-sm text-rose-700">{errorApi}</p>}
            {cargando && <p role="status" className="py-8 text-center text-sm text-slate-500">Cargando incidentes...</p>}

            {/*Las tarjetas cambian de una columna en móvil a cuatro en escritorio*/}
            <section className="mb-4 grid grid-cols-1 gap-3 sm:grid-cols-2 xl:grid-cols-4">
                <CartaEstadistica variant="compact" icon={Siren} iconClassName="bg-slate-700" title="Total" value={String(incidentes.length)} subtitle="" />
                <CartaEstadistica variant="compact" icon={Clock3} iconClassName="bg-amber-500" title="Pendientes" value={String(pendientes)} subtitle="" />
                <CartaEstadistica variant="compact" icon={Hammer} iconClassName="bg-blue-500" title="En progreso" value={String(enProgreso)} subtitle="" />
                <CartaEstadistica variant="compact" icon={CircleCheck} iconClassName="bg-emerald-500" title="Resueltos" value={String(resueltos)} subtitle="" className="border-2 border-emerald-400" />
            </section>

            <nav aria-label="Filtrar incidentes por estado" className="mb-4 flex flex-wrap gap-2">
                {filtros.map((opcion) => (
                    <button
                        key={opcion}
                        type="button"
                        aria-pressed={filtro === opcion}
                        onClick={() => {
                            setFiltro(opcion)
                            setPaginaActual(1)
                        }}
                        className={`rounded-full border px-4 py-2 text-sm font-medium transition-colors ${filtro === opcion ? 'border-amber-500 bg-amber-500 text-slate-950' : 'border-slate-200 bg-white text-slate-600 hover:bg-slate-100'}`}
                    >
                        {opcion}
                    </button>
                ))}
            </nav>

            <DataTable
                columns={columnasIncidentes}
                rows={cargando ? [] : incidentesPagina}
                getRowKey={(incidente) => String(incidente.id)}
                footer={
                    <Paginacion
                        paginaActual={pagina}
                        tamanoPagina={tamanoPagina}
                        totalElementos={incidentesFiltrados.length}
                        etiqueta="incidentes"
                        alCambiarPagina={setPaginaActual}
                    />
                }
            />

            {(modalRegistroAbierto || incidenteSeleccionado) && (
                <div className="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/40 p-3 sm:p-4" role="presentation" onMouseDown={(evento) => {
                    if (evento.target === evento.currentTarget) {
                        setModalRegistroAbierto(false)
                        setIncidenteSeleccionado(null)
                    }
                }}>
                    {/*El formulario cabe en pantallas bajas y permite desplazarse dentro del modal*/}
                    <section role="dialog" aria-modal="true" aria-labelledby="incidente-dialog-title" className="max-h-[calc(100dvh-1.5rem)] w-full max-w-lg overflow-y-auto rounded-xl bg-white p-4 shadow-xl sm:max-h-[calc(100dvh-2rem)] sm:p-5">
                        <div className="mb-4 flex items-center justify-between">
                            <h3 id="incidente-dialog-title" className="text-lg font-semibold text-slate-900">
                                {incidenteSeleccionado ? `Incidente #${incidenteSeleccionado.id}` : 'Registrar incidente'}
                            </h3>
                            <button
                                type="button"
                                aria-label="Cerrar ventana"
                                onClick={() => {
                                    setModalRegistroAbierto(false)
                                    setIncidenteSeleccionado(null)
                                }}
                                className="rounded-md p-2 text-slate-500 hover:bg-slate-100"
                            >
                                <X size={18} />
                            </button>
                        </div>

                        {incidenteSeleccionado ? (
                            <dl className="grid grid-cols-[120px_1fr] gap-x-4 gap-y-3 text-sm">
                                <dt className="text-slate-500">Fecha y hora</dt><dd className="text-slate-800">{fechaVisible(incidenteSeleccionado.fecha)} · {incidenteSeleccionado.hora}</dd>
                                <dt className="text-slate-500">Tipo</dt><dd className="text-slate-800">{incidenteSeleccionado.tipo}</dd>
                                <dt className="text-slate-500">Descripción</dt><dd className="text-slate-800">{incidenteSeleccionado.descripcion}</dd>
                                <dt className="text-slate-500">Prioridad</dt><dd className="text-slate-800">{incidenteSeleccionado.prioridad}</dd>
                                <dt className="text-slate-500">Estado</dt><dd className="text-slate-800">{incidenteSeleccionado.estado}</dd>
                            </dl>
                        ) : (
                            <form onSubmit={registrarIncidente} className="space-y-4">
                                <label className="block text-sm font-medium text-slate-700">
                                    Tipo
                                    <select value={nuevoIncidente.tipo} onChange={(evento) => setNuevoIncidente({ ...nuevoIncidente, tipo: evento.target.value })} className="mt-1 h-10 w-full rounded-lg border border-slate-200 bg-white px-3">
                                        <option>QR inválido</option><option>Sin red</option><option>Energía</option><option>Fuerza física</option><option>Software</option><option>Otro</option>
                                    </select>
                                </label>
                                <label className="block text-sm font-medium text-slate-700">
                                    Descripción
                                    <textarea required value={nuevoIncidente.descripcion} onChange={(evento) => setNuevoIncidente({ ...nuevoIncidente, descripcion: evento.target.value })} rows={3} className="mt-1 w-full rounded-lg border border-slate-200 p-3" />
                                </label>
                                <label className="block text-sm font-medium text-slate-700">
                                    Prioridad
                                    <select value={nuevoIncidente.prioridad} onChange={(evento) => setNuevoIncidente({ ...nuevoIncidente, prioridad: evento.target.value as PrioridadIncidente })} className="mt-1 h-10 w-full rounded-lg border border-slate-200 bg-white px-3">
                                        <option>Crítica</option><option>Alta</option><option>Media</option><option>Baja</option>
                                    </select>
                                </label>
                                {errorApi && <p role="alert" className="text-sm text-rose-600">{errorApi}</p>}
                                <div className="flex justify-end gap-2 pt-2">
                                    <button type="button" onClick={() => setModalRegistroAbierto(false)} className="rounded-lg border border-slate-200 px-4 py-2 text-sm text-slate-600 hover:bg-slate-50">Cancelar</button>
                                    <button type="submit" disabled={guardando} className="rounded-lg bg-rose-500 px-4 py-2 text-sm font-semibold text-white hover:bg-rose-600 disabled:cursor-wait disabled:opacity-60">{guardando ? 'Registrando...' : 'Registrar'}</button>
                                </div>
                            </form>
                        )}
                    </section>
                </div>
            )}
        </div>
    )
}