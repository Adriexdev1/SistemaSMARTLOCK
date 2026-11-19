import { Eye, QrCode, Pencil, Trash, Plus, X } from "lucide-react"
import { QRCodeSVG } from 'qrcode.react'
import { useNavigate } from 'react-router-dom'
import { useCitas, type Cita } from '../contexts/CitasContextState'
//Importacion de tabla base
import DataTable  from "../components/TablaBase"
//Importacion de pie de tabla
import Paginacion from "../components/PieTabla"
//Libreria para realizar cambios en la informacion mostrada al realizar busquedas
import { useState, type FormEvent } from "react"
//Importacion de la barra de busqueda
import BarraBusqueda from "../components/componentesPaginas/BarraBusqueda"
import SelectorCamposBusqueda, { type CampoBusqueda } from "../components/componentesPaginas/SelectorCamposBusqueda"
import { crearContenidoQrPuerta } from '../utils/codigoQrPuerta'

const coloresEstado: Record<Cita['estado'], string> = {
  Activo: 'bg-emerald-100 text-emerald-700',
  Inactivo: 'bg-rose-100 text-rose-700',
}

const camposBusquedaEvento: CampoBusqueda[] = [
  { clave: 'id', etiqueta: 'ID' },
  { clave: 'fecha', etiqueta: 'Fecha de inicio' },
  { clave: 'fechaFin', etiqueta: 'Fecha fin' },
  { clave: 'hora', etiqueta: 'Hora de inicio' },
  { clave: 'horaFin', etiqueta: 'Hora fin' },
  { clave: 'descripcion', etiqueta: 'Descripción' },
  { clave: 'participantes', etiqueta: 'Participantes' },
  { clave: 'estado', etiqueta: 'Estado' },
]

function fechaActualLocal() {
  const fecha = new Date()
  const anio = fecha.getFullYear()
  const mes = String(fecha.getMonth() + 1).padStart(2, '0')
  const dia = String(fecha.getDate()).padStart(2, '0')
  return `${anio}-${mes}-${dia}`
}

function fechaTabla(fecha: string) {
  if (!fecha) return '—'
  const [anio, mes, dia] = fecha.split('-')
  return `${dia}/${mes}/${anio}`
}

function crearColumnasCitas(
  verCita: (cita: Cita) => void,
  verQr: (cita: Cita) => void,
  editarCita: (cita: Cita) => void,
  confirmarBorrado: (cita: Cita) => void,
) {
  return [
    {
      header: 'ID', width: '7%', render: (cita: Cita) => <span className="font-mono font-semibold">{cita.id}</span>,
    },
    {
      header: 'Fecha inicio', width: '13%',
      render: (cita: Cita) => <span className="font-mono whitespace-nowrap text-slate-500">{fechaTabla(cita.fecha)}</span>,
    },
    { header: 'Hora inicio', width: '9%', render: (cita: Cita) => <span className="font-mono font-semibold whitespace-nowrap">{cita.hora}</span> },
    {
      header: 'Fecha fin', width: '13%',
      render: (cita: Cita) => <span className="font-mono whitespace-nowrap text-slate-500">{fechaTabla(cita.fechaFin)}</span>,
    },
    { header: 'Hora fin', width: '9%', render: (cita: Cita) => <span className="font-mono font-semibold whitespace-nowrap">{cita.horaFin}</span> },
    { header: 'Descripción', width: '15%', render: (cita: Cita) => <span className="font-mono text-slate-500">{cita.tipo}</span> },
    { header: 'Estado', width: '12%', render: (cita: Cita) => <span className={`rounded-full px-2.5 py-1 text-xs font-semibold ${coloresEstado[cita.estado]}`}>{cita.estado}</span> },
    {
      header: 'Acciones', width: '22%',
      render: (cita: Cita) => (
        <div className="flex items-center gap-1">
          <button type="button" onClick={() => verCita(cita)} aria-label={`Ver cita ${cita.id}`} className="flex h-8 w-8 items-center justify-center rounded-md text-slate-500 hover:bg-slate-100"><Eye size={16} /></button>
          <button type="button" onClick={() => verQr(cita)} aria-label={`Mostrar QR de cita ${cita.id}`} className="flex h-8 w-8 items-center justify-center rounded-md text-violet-600 hover:bg-violet-100"><QrCode size={16} /></button>
          <button type="button" onClick={() => editarCita(cita)} aria-label={`Editar cita ${cita.id}`} className="flex h-8 w-8 items-center justify-center rounded-md text-blue-600 hover:bg-blue-100"><Pencil size={16} /></button>
          <button type="button" onClick={() => confirmarBorrado(cita)} aria-label={`Eliminar cita ${cita.id}`} className="flex h-8 w-8 items-center justify-center rounded-md text-rose-600 hover:bg-rose-100"><Trash size={16} /></button>
        </div>
      ),
    },
  ]
}

//Funcion para ignorar mayusculas y acentos en las busquedas
function normalizar(texto: string) {
  return texto
    .normalize('NFD')
    .replace(/[\u0300-\u036f]/g, '')
    .toLowerCase()
    .trim()
}

export default function Citas(){
  const navigate = useNavigate()
  const { citas, usuarios, cargando, error: errorCarga, agregarCita, eliminarCita, generarQr } = useCitas()
  const [citaQr, setCitaQr] = useState<Cita | null>(null)
  const [citaAEliminar, setCitaAEliminar] = useState<Cita | null>(null)
  const [guardando, setGuardando] = useState(false)
  const [errorAccion, setErrorAccion] = useState('')
  const [modalNuevoAbierto, setModalNuevoAbierto] = useState(false)
  const [nuevoAcceso, setNuevoAcceso] = useState({
    idUsuarios: [] as string[],
    tipo: '',
    fechaInicio: fechaActualLocal(),
    hora: '',
    fechaFin: fechaActualLocal(),
    horaFin: '',
  })
  const [busqueda, setBusqueda] = useState('')
  const [camposBusqueda, setCamposBusqueda] = useState<string[]>(() => camposBusquedaEvento.map((campo) => campo.clave))
  const [estado, setEstado] = useState('Todos')
  const [fecha, setFecha] = useState('')
  const [paginaActual, setPaginaActual] = useState(1)
  const tamanoPagina = 5

  const citasFiltradas = citas.filter((cita) => {
    const fechaLegible = new Date(`${cita.fecha}T00:00:00`).toLocaleDateString('es-MX')
    const valores: Record<string, string> = {
      id: String(cita.id),
      fecha: `${cita.fecha} ${fechaLegible}`,
      fechaFin: cita.fechaFin,
      hora: cita.hora,
      horaFin: cita.horaFin,
      descripcion: cita.tipo,
      participantes: cita.participantes.map((participante) => participante.nombre).join(' '),
      estado: cita.estado,
    }
    const terminos = normalizar(busqueda).split(/\s+/).filter(Boolean)
    const coincideTexto = terminos.every((termino) =>
      camposBusqueda.some((campo) => normalizar(valores[campo] ?? '').includes(termino))
    )
    return coincideTexto &&
      (estado === 'Todos' || cita.estado === estado) &&
      (fecha === '' || cita.fecha === fecha)
  })

  const totalPaginas = Math.max(1, Math.ceil(citasFiltradas.length / tamanoPagina))
  const pagina = Math.min(paginaActual, totalPaginas)
  const inicio = (pagina - 1) * tamanoPagina
  const citasPagina = citasFiltradas.slice(inicio, inicio + tamanoPagina)
  const columnasCitas = crearColumnasCitas(
    (cita) => navigate(`/eventos/${cita.id}`),
    (cita) => void abrirQr(cita),
    (cita) => navigate(`/eventos/${cita.id}?editar=1`),
    (cita) => setCitaAEliminar(cita),
  )

  async function borrarCita() {
    if (!citaAEliminar) return
    setGuardando(true)
    setErrorAccion('')
    try {
      await eliminarCita(citaAEliminar.id)
      setCitaAEliminar(null)
    } catch (error) {
      setErrorAccion(error instanceof Error ? error.message : 'No se pudo eliminar el acceso.')
    } finally {
      setGuardando(false)
    }
  }

  async function registrarAcceso(evento: FormEvent<HTMLFormElement>) {
    evento.preventDefault()
    const inicio = Date.parse(`${nuevoAcceso.fechaInicio}T${nuevoAcceso.hora}:00`)
    const fin = Date.parse(`${nuevoAcceso.fechaFin}T${nuevoAcceso.horaFin}:00`)
    if (!Number.isFinite(inicio) || !Number.isFinite(fin)) {
      setErrorAccion('Selecciona la fecha y hora de inicio y terminación.')
      return
    }
    if (fin < inicio) {
      setErrorAccion('La fecha y hora de terminación no puede ser anterior al inicio.')
      return
    }

    const idUsuarios = [...new Set(nuevoAcceso.idUsuarios.map(Number))]
    if (idUsuarios.length === 0) {
      setErrorAccion('Selecciona al menos un usuario.')
      return
    }

    setGuardando(true)
    setErrorAccion('')
    try {
      await agregarCita({
        tipo: nuevoAcceso.tipo,
        fecha: nuevoAcceso.fechaInicio,
        fechaFin: nuevoAcceso.fechaFin,
        hora: nuevoAcceso.hora,
        horaFin: nuevoAcceso.horaFin,
        estado: 'Activo',
      }, idUsuarios)
      const hoy = fechaActualLocal()
      setNuevoAcceso({ idUsuarios: [], tipo: '', fechaInicio: hoy, hora: '', fechaFin: hoy, horaFin: '' })
      setModalNuevoAbierto(false)
    } catch (error) {
      setErrorAccion(error instanceof Error ? error.message : 'No se pudo guardar el acceso.')
    } finally {
      setGuardando(false)
    }
  }

  function abrirQr(cita: Cita) {
    setCitaQr(cita)
    setErrorAccion('')
  }

  async function generarQrParticipante(cita: Cita, idDetalleEvento: number) {
    setGuardando(true)
    setErrorAccion('')
    try {
      const codigoQr = await generarQr(idDetalleEvento)
      setCitaQr((actual) => actual?.id === cita.id ? {
        ...actual,
        participantes: actual.participantes.map((participante) => participante.idDetalleEvento === idDetalleEvento
          ? { ...participante, qr: 'Generado', qrToken: codigoQr }
          : participante),
      } : actual)
    } catch (error) {
      setErrorAccion(error instanceof Error ? error.message : 'No se pudo generar el QR.')
    } finally {
      setGuardando(false)
    }
  }

  return (
  <>
  {/*En móvil los filtros se apilan y en pantallas sm se alinean en una fila*/}
      <section className="mb-5 flex flex-wrap items-center justify-between gap-4">
        <div>
        <h2 className="text-xl font-semibold text-slate-900">
          Eventos
        </h2>
        <p className="mt-1 text-sm text-slate-500">
          Gestiona los eventos programados
        </p>
      </div>

      <button
        type="button"
        onClick={() => {
          setErrorAccion('')
          const hoy = fechaActualLocal()
          setNuevoAcceso({ idUsuarios: [], tipo: '', fechaInicio: hoy, hora: '', fechaFin: hoy, horaFin: '' })
          setModalNuevoAbierto(true)
        }}
        disabled={cargando || usuarios.length === 0}
        className="inline-flex items-center gap-2 rounded-md bg-amber-500 px-4 py-2 font-medium text-slate-950 hover:bg-amber-400 disabled:cursor-not-allowed disabled:opacity-50"
      >
        <Plus size={18} aria-hidden="true" />
        Nuevo evento
      </button>
    </section>
    {errorCarga && <p role="alert" className="mb-4 rounded-md border border-rose-200 bg-rose-50 px-4 py-3 text-sm text-rose-700">{errorCarga}</p>}
    {cargando && <p role="status" className="py-8 text-center text-sm text-slate-500">Cargando accesos...</p>}
    <BarraBusqueda
      valor={busqueda}
      alCambiar={(valor) => {
        setBusqueda(valor)
        setPaginaActual(1)
      }}
      placeholder="Buscar evento..."
    >
      <SelectorCamposBusqueda
        campos={camposBusquedaEvento}
        seleccionados={camposBusqueda}
        alCambiar={(campos) => {
          setCamposBusqueda(campos)
          setPaginaActual(1)
        }}
      />
      {/*Calendario para buscar por fecha*/}
      <input
        type="date"
        value={fecha}
        onChange={(evento) => {
          setFecha(evento.target.value)
          setPaginaActual(1)
        }}
        aria-label="Filtrar por fecha"
        className="h-11 w-full rounded-lg border border-slate-200 px-3 text-sm sm:w-auto"
      />

      {/*Filtro por estados*/}
      <select
        value={estado}
        onChange={(evento) => {
          setEstado(evento.target.value)
          setPaginaActual(1)
        }}
        aria-label="Filtrar por estado"
        className="h-11 w-full rounded-lg border border-slate-200 bg-white px-3 text-sm sm:w-auto"
      >
        <option>Todos</option>
        <option>Activo</option>
        <option>Inactivo</option>
      </select>
    </BarraBusqueda>

    <div className="mt-4">
      <DataTable
        columns={columnasCitas}
        rows={cargando ? [] : citasPagina}
        getRowKey={(cita) => String(cita.id)}
        minWidth={1000}
        footer={
          <Paginacion
            paginaActual={pagina}
            tamanoPagina={tamanoPagina}
            totalElementos={citasFiltradas.length}
            etiqueta="registros"
            alCambiarPagina={setPaginaActual}
          />
        }
      />
    </div>

    {/*Funcionalidad del modal para nuevos accesos*/}
    {modalNuevoAbierto && (
      <div className="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/45 p-4">
        <section
        role="dialog"
        aria-modal="true"
        aria-labelledby="nuevo-acceso-titulo"
        className="max-h-[calc(100dvh-2rem)] w-full max-w-lg overflow-y-auto rounded-xl bg-white p-5 shadow-xl">
          
        <h2 id="nuevo-acceso-titulo" className="text-lg font-semibold text-slate-900">
          Nuevo evento
        </h2>

      <form onSubmit={registrarAcceso} className="mt-5 space-y-4">
        {errorAccion && <p role="alert" className="rounded-md border border-rose-200 bg-rose-50 px-3 py-2 text-sm text-rose-700">{errorAccion}</p>}
        <label className="block text-sm text-slate-600">
          Visitante / Empleado
          <select
            multiple
            required
            size={Math.min(Math.max(usuarios.length, 2), 5)}
            value={nuevoAcceso.idUsuarios}
            onChange={(evento) => setNuevoAcceso({
              ...nuevoAcceso,
              idUsuarios: Array.from(evento.target.selectedOptions, (opcion) => opcion.value),
            })}
            className="mt-1 w-full rounded-lg border border-slate-200 bg-white p-2"
          >
            {usuarios.map((usuario) => <option key={usuario.id} value={usuario.id}>{usuario.nombre}</option>)}
          </select>
          <span className="mt-1 block text-xs text-slate-500">Seleccionados: {nuevoAcceso.idUsuarios.length}</span>
        </label>

        <label className="block text-sm text-slate-600">
          Tipo de evento
          <input
            required
            value={nuevoAcceso.tipo}
            onChange={(evento) =>
              setNuevoAcceso({ ...nuevoAcceso, tipo: evento.target.value })
            }
            className="mt-1 h-10 w-full rounded-lg border border-slate-200 px-3"
          />
        </label>

        <label className="block text-sm text-slate-600">
          Fecha de inicio
          <input
            required
            type="date"
            value={nuevoAcceso.fechaInicio}
            onChange={(evento) => {
              const fechaInicio = evento.target.value
              setNuevoAcceso((actual) => ({
                ...actual,
                fechaInicio,
                fechaFin: actual.fechaFin < fechaInicio ? fechaInicio : actual.fechaFin,
              }))
            }}
            className="mt-1 h-10 w-full rounded-lg border border-slate-200 px-3"
          />
        </label>

        <label className="block text-sm text-slate-600">
          Fecha de terminación
          <input
            required
            type="date"
            min={nuevoAcceso.fechaInicio}
            value={nuevoAcceso.fechaFin}
            onChange={(evento) =>
              setNuevoAcceso({ ...nuevoAcceso, fechaFin: evento.target.value })
            }
            className="mt-1 h-10 w-full rounded-lg border border-slate-200 px-3"
          />
        </label>

        <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
          <label className="block text-sm text-slate-600">
            Hora de inicio
            <input
              required
              type="time"
              value={nuevoAcceso.hora}
              onChange={(evento) =>
                setNuevoAcceso({ ...nuevoAcceso, hora: evento.target.value })
              }
              className="mt-1 h-10 w-full rounded-lg border border-slate-200 px-3"
            />
          </label>

          <label className="block text-sm text-slate-600">
            Hora de terminación
            <input
              required
              type="time"
              value={nuevoAcceso.horaFin}
              min={nuevoAcceso.fechaFin === nuevoAcceso.fechaInicio ? nuevoAcceso.hora : undefined}
              onChange={(evento) =>
                setNuevoAcceso({ ...nuevoAcceso, horaFin: evento.target.value })
              }
              className="mt-1 h-10 w-full rounded-lg border border-slate-200 px-3"
            />
          </label>
        </div>

        <div className="flex flex-wrap justify-end gap-2 pt-2">
          <button
            type="button"
            onClick={() => {
              setModalNuevoAbierto(false)
              setErrorAccion('')
            }}
            className="rounded-lg border border-slate-200 px-4 py-2 text-sm text-slate-600"
          >
            Cancelar
          </button>
          <button
            type="submit"
            disabled={guardando || usuarios.length === 0}
            className="rounded-lg bg-amber-500 px-4 py-2 text-sm font-semibold text-slate-950 disabled:cursor-wait disabled:opacity-60"
          >
            {guardando ? 'Guardando...' : 'Crear evento'}
          </button>
        </div>
      </form>
    </section>
  </div>
)}

    {/*Funcionalidad de codigos QR*/}
    {citaQr && (
      <div className="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/45 p-4" role="presentation" onMouseDown={(evento) => {
        if (evento.target === evento.currentTarget) setCitaQr(null)
      }}>
        <section role="dialog" aria-modal="true" aria-labelledby="qr-dialog-title" className="w-full max-w-sm rounded-xl bg-white p-5 shadow-xl">
          <div className="mb-5 flex items-center justify-between">
            <h2 id="qr-dialog-title" className="text-lg font-semibold text-slate-900">Participantes del evento #{citaQr.id}</h2>
            <button type="button" onClick={() => setCitaQr(null)} aria-label="Cerrar QR" className="rounded-md p-2 text-slate-500 hover:bg-slate-100">
              <X size={18} />
            </button>
          </div>
          {errorAccion && <p role="alert" className="mb-3 text-sm text-rose-600">{errorAccion}</p>}
          <div className="max-h-[65vh] space-y-3 overflow-y-auto">
            {citaQr.participantes.map((participante) => (
              <article key={participante.idDetalleEvento} className="rounded-lg border border-slate-200 p-3">
                <div className="flex flex-wrap items-center justify-between gap-3">
                  <div>
                    <p className="font-medium text-slate-800">{participante.nombre}</p>
                    <p className="text-xs text-slate-500">{participante.qr}</p>
                  </div>
                  {participante.qr === 'Pendiente' && <button type="button" disabled={guardando} onClick={() => void generarQrParticipante(citaQr, participante.idDetalleEvento)} className="rounded-md bg-amber-500 px-3 py-2 text-sm font-semibold text-slate-950 disabled:opacity-60">{guardando ? 'Generando...' : 'Generar QR'}</button>}
                </div>
                {participante.qrToken && <div className="mt-3 flex flex-col items-center gap-2 border-t border-slate-100 pt-3"><QRCodeSVG value={crearContenidoQrPuerta(participante.idDetalleEvento, participante.qrToken)} size={176} level="M" includeMargin /><p className="break-all text-center font-mono text-xs text-slate-500">{crearContenidoQrPuerta(participante.idDetalleEvento, participante.qrToken)}</p></div>}
              </article>
            ))}
            {citaQr.participantes.length === 0 && <p className="py-5 text-center text-sm text-slate-500">Este evento no tiene participantes asignados.</p>}
          </div>
        </section>
      </div>
    )}

    {citaAEliminar && (
      <div className="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/45 p-4" role="presentation" onMouseDown={(evento) => {
        if (evento.target === evento.currentTarget) setCitaAEliminar(null)
      }}>
        <section role="alertdialog" aria-modal="true" aria-labelledby="delete-dialog-title" aria-describedby="delete-dialog-description" className="w-full max-w-md rounded-xl bg-white p-5 shadow-xl">
          <h2 id="delete-dialog-title" className="text-lg font-semibold text-slate-900">¿Eliminar este acceso?</h2>
          <p id="delete-dialog-description" className="mt-2 text-sm text-slate-600">
            Se eliminará el evento {citaAEliminar.tipo} y sus {citaAEliminar.participantes.length} asignaciones. Esta acción no se puede deshacer.
          </p>
          {errorAccion && <p role="alert" className="mt-3 text-sm text-rose-600">{errorAccion}</p>}
          <div className="mt-6 flex justify-end gap-2">
            <button type="button" onClick={() => setCitaAEliminar(null)} className="rounded-lg border border-slate-200 px-4 py-2 text-sm text-slate-600 hover:bg-slate-50">Cancelar</button>
            <button type="button" disabled={guardando} onClick={() => void borrarCita()} className="rounded-lg bg-rose-600 px-4 py-2 text-sm font-semibold text-white hover:bg-rose-700 disabled:cursor-wait disabled:opacity-60">{guardando ? 'Eliminando...' : 'Eliminar acceso'}</button>
          </div>
        </section>
      </div>
    )}
  </>
)
}
