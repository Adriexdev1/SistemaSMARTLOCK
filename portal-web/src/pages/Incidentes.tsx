import { useState, type FormEvent } from 'react'
import { Hammer, CircleCheck, Clock3, Eye, Plus, RefreshCw, Siren, X } from 'lucide-react'
import CartaEstadistica from '../components/componentesPaginas/CartaEstadistica'
import DataTable from '../components/TablaBase'
import Paginacion from '../components/PieTabla'

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

const incidentesIniciales: Incidente[] = [
    {
        id: 1,
        fecha: '2026-08-28',
        hora: '09:40',
        tipo: 'QR inválido',
        descripcion: 'Código QR rechazado 3 veces consecutivas',
        prioridad: 'Alta',
        estado: 'Pendiente',
    },
    {
        id: 2,
        fecha: '2026-08-28',
        hora: '06:15',
        tipo: 'Sin red',
        descripcion: 'Pérdida de conexión WiFi por 8 min',
        prioridad: 'Media',
        estado: 'Resuelto',
    },
    {
        id: 3,
        fecha: '2026-08-27',
        hora: '18:30',
        tipo: 'Energía',
        descripcion: 'Corte de energía eléctrica en Zona B',
        prioridad: 'Alta',
        estado: 'Resuelto',
    },
    {
        id: 4,
        fecha: '2026-08-27',
        hora: '14:00',
        tipo: 'Fuerza física',
        descripcion: 'Intento de apertura forzada detectado',
        prioridad: 'Crítica',
        estado: 'En progreso',
    },
    {
        id: 5,
        fecha: '2026-08-26',
        hora: '11:20',
        tipo: 'Software',
        descripcion: 'Error en generación de QR: tiempo de espera agotado',
        prioridad: 'Baja',
        estado: 'Resuelto',
    },
    {
        id: 6,
        fecha: '2026-08-26',
        hora: '08:05',
        tipo: 'QR inválido',
        descripcion: 'Código expirado presentado en el acceso',
        prioridad: 'Alta',
        estado: 'Pendiente',
    },
]

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
    const [incidentes, setIncidentes] = useState(incidentesIniciales)
    const [filtro, setFiltro] = useState<FiltroIncidentes>('Todos')
    const [paginaActual, setPaginaActual] = useState(1)
    const [modalRegistroAbierto, setModalRegistroAbierto] = useState(false)
    const [incidenteSeleccionado, setIncidenteSeleccionado] = useState<Incidente | null>(null)
    const [nuevoIncidente, setNuevoIncidente] = useState({
        tipo: 'QR inválido',
        descripcion: '',
        prioridad: 'Media' as PrioridadIncidente,
    })

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

    function avanzarEstado(incidenteId: number) {
        setIncidentes((actuales) => actuales.map((incidente) => {
            if (incidente.id !== incidenteId) return incidente
            const siguienteEstado: EstadoIncidente = incidente.estado === 'Pendiente'
                ? 'En progreso'
                : 'Resuelto'
            return { ...incidente, estado: siguienteEstado }
        }))
    }

    function registrarIncidente(evento: FormEvent<HTMLFormElement>) {
        evento.preventDefault()
        const ahora = new Date()
        const fecha = `${ahora.getFullYear()}-${String(ahora.getMonth() + 1).padStart(2, '0')}-${String(ahora.getDate()).padStart(2, '0')}`
        const hora = `${String(ahora.getHours()).padStart(2, '0')}:${String(ahora.getMinutes()).padStart(2, '0')}`
        const nuevo: Incidente = {
            ...nuevoIncidente,
            id: Math.max(0, ...incidentes.map((incidente) => incidente.id)) + 1,
            fecha,
            hora,
            estado: 'Pendiente',
        }

        setIncidentes((actuales) => [nuevo, ...actuales])
        setFiltro('Todos')
        setPaginaActual(1)
        setNuevoIncidente({ tipo: 'QR inválido', descripcion: '', prioridad: 'Media' })
        setModalRegistroAbierto(false)
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
                            onClick={() => avanzarEstado(incidente.id)}
                            className="flex h-8 w-8 items-center justify-center rounded-md text-slate-500 transition-colors hover:bg-blue-100 hover:text-blue-700 focus-visible:outline-2 focus-visible:outline-blue-400"
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
                    onClick={() => setModalRegistroAbierto(true)}
                    className="inline-flex items-center gap-2 rounded-lg bg-rose-500 px-4 py-2.5 text-sm font-semibold text-white transition-colors hover:bg-rose-600"
                >
                    <Plus size={18} aria-hidden="true" />
                    Registrar incidente
                </button>
            </section>

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
                rows={incidentesPagina}
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
                                <div className="flex justify-end gap-2 pt-2">
                                    <button type="button" onClick={() => setModalRegistroAbierto(false)} className="rounded-lg border border-slate-200 px-4 py-2 text-sm text-slate-600 hover:bg-slate-50">Cancelar</button>
                                    <button type="submit" className="rounded-lg bg-rose-500 px-4 py-2 text-sm font-semibold text-white hover:bg-rose-600">Registrar</button>
                                </div>
                            </form>
                        )}
                    </section>
                </div>
            )}
        </div>
    )
}