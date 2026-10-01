import { Eye, QrCode, Pencil, Trash, Plus, X } from "lucide-react"
import { QRCodeSVG } from 'qrcode.react'
import { useNavigate } from 'react-router-dom'
import { useCitas } from '../contexts/CitasContext'
import type { Cita } from '../contexts/CitasContext'
//Importacion de tabla base
import DataTable  from "../components/TablaBase"
//Importacion de pie de tabla
import Paginacion from "../components/PieTabla"
//Libreria para realizar cambios en la informacion mostrada al realizar busquedas
import { useState, type FormEvent } from "react"
//Importacion de la barra de busqueda
import BarraBusqueda from "../components/componentesPaginas/BarraBusqueda"

//Constante para aplicar color al estado de la visita
const coloresEstado: Record<Cita['estado'], string> = {
  Activo: 'bg-emerald-100 text-emerald-700',
  Completado: 'bg-blue-100 text-blue-700',
}

//Constante para aplicar color al estado del QR
const coloresQr: Record<Cita['qr'], string> = {
  Generado: 'bg-emerald-100 text-emerald-700',
  Pendiente: 'bg-amber-100 text-amber-700',
  Utilizado: 'bg-slate-100 text-slate-600',
}

//Variable para estructura de Datos en la tabla
function crearColumnasCitas(
  verCita: (cita: Cita) => void,
  verQr: (cita: Cita) => void,
  editarCita: (cita: Cita) => void,
  confirmarBorrado: (cita: Cita) => void,
) {
  return [
  {
    header: 'Fecha',
    width: '12%',
    render: (cita: Cita) => {
        const [anio, mes, dia] = cita.fecha.split('-')
        return (
            <span className="font-mono text-slate-500">
                {dia}/{mes}/{anio}
            </span>
        )
    },
  },
  {
    header: 'Hora',
    width: '9%',
    render: (cita: Cita) => (
      <span className="font-mono font-semibold">{cita.hora}</span>
    ),
  },
  {
    header: 'Visitante',
    width: '21%',
    render: (cita: Cita) => (
      <span className="font-semibold text-slate-800">{cita.visitante}</span>
    ),
  },
  {
    header: 'Tipo',
    width: '17%',
    render: (cita: Cita) => (
      <span className="font-mono text-slate-500">
        {cita.tipo}
      </span>
    ),
  },
  {
    header: 'Estado',
    width: '12%',
    render: (cita: Cita) => (
        <span className={`rounded-full px-2.5 py-1 text-xs font-semibold ${coloresEstado[cita.estado]}`}>
        {cita.estado}
        </span>
    ),
  },
  {
    header: 'QR',
    width: '12%',
    render: (cita: Cita) =>(
        <span className={`rounded-full px-2.5 py-1 text-xs font-semibold ${coloresQr[cita.qr]}`}>
        {cita.qr}
        </span>
    ),
  },
  {
    header: 'Acciones',
    width: '17%',
    render: (cita: Cita) => (
    //Contenedor central de los iconos
    <div className="flex items-center gap-1">
        {/*Boton de ver cita*/}
        <button type="button" onClick={() => verCita(cita)} aria-label={`Ver cita ${cita.id}`} className="flex h-8 w-8 items-center justify-center rounded-md text-slate-500 transition-colors hover:bg-slate-100 hover:text-slate-900 focus-visible:outline-2 focus-visible:outline-slate-400">
            <Eye size={16} strokeWidth={2.5} />
        </button>
        {/*Boton de ver QR*/}
        <button type="button" onClick={() => verQr(cita)} aria-label={`Mostrar QR de cita ${cita.id}`} className="flex h-8 w-8 items-center justify-center rounded-md text-violet-600 transition-colors hover:bg-violet-100 hover:text-violet-800 focus-visible:outline-2 focus-visible:outline-violet-400">
            <QrCode size={16} strokeWidth={2.5}/>
        </button>
        {/*Boton de editar cita*/}
        <button type="button" onClick={() => editarCita(cita)} aria-label={`Editar cita ${cita.id}`} className="flex h-8 w-8 items-center justify-center rounded-md text-blue-600 transition-colors hover:bg-blue-100 hover:text-blue-800 focus-visible:outline-2 focus-visible:outline-blue-400">
            <Pencil size={16} strokeWidth={2.5}/>
        </button>
        {/*Boton de eliminar cita*/}
        <button type="button" onClick={() => confirmarBorrado(cita)} aria-label={`Eliminar cita ${cita.id}`} className="flex h-8 w-8 items-center justify-center rounded-md text-rose-600 transition-colors hover:bg-rose-100 hover:text-rose-800 focus-visible:outline-2 focus-visible:outline-rose-400">
            <Trash size={16} strokeWidth={2.5}/>
            </button>
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
  //Constantes para llevar acabo acciones con las citas
  const navigate = useNavigate()
  const { citas, agregarCita,  eliminarCita } = useCitas()
  const [citaQr, setCitaQr] = useState<Cita | null>(null)
  const [citaAEliminar, setCitaAEliminar] = useState<Cita | null>(null)

  //Apartado visual para agregar las citas
  const [modalNuevoAbierto, setModalNuevoAbierto] = useState(false)
  const [nuevoAcceso, setNuevoAcceso] = useState({
    visitante: '',
    tipo: '',
    fecha: '',
    hora: '',
    horaFin: '',
  })

    //Constantes para llevar acabo las busquedas dentro de las citas
    const [busqueda, setBusqueda] = useState('')
    const [estado, setEstado] = useState('Todos')
    const [fecha, setFecha] = useState('')
    const [paginaActual, setPaginaActual] = useState(1)    
    const tamanoPagina = 5
    
    //Constante para realizar el filtro de las citas
    const citasFiltradas = citas.filter((cita) => {
        //Constantes para busqueda en base al texto
         const fechaLegible = new Date(`${cita.fecha}T00:00:00`).toLocaleDateString('es-MX')
         
         //Campos utilizados en las busquedas
        const campos = normalizar(
          `${cita.id} ${cita.fecha} ${fechaLegible} ${cita.hora} ${cita.horaFin} ` +
          `${cita.visitante} ${cita.tipo} ${cita.estado} ${cita.qr}`
        )

        //Termino para realizar la busqueda y encontrar las coincidencias
        const terminos = normalizar(busqueda).split(/\s+/).filter(Boolean)
        const coincideBusqueda = terminos.every((termino) =>
            campos.includes(termino)
        )
        const coincideEstado = estado === 'Todos' || cita.estado === estado
        const coincideFecha = fecha === '' || cita.fecha === fecha

  return coincideBusqueda && coincideEstado && coincideFecha
})
    
    const totalPaginas = Math.max(
        1, Math.ceil(citasFiltradas.length / tamanoPagina)
    )
    
    const pagina = Math.min(paginaActual, totalPaginas)
    const inicio = (pagina - 1) * tamanoPagina
    const citasPagina = citasFiltradas.slice(inicio, inicio + tamanoPagina)
    const columnasCitas = crearColumnasCitas(
      (cita) => navigate(`/eventos/${cita.id}`),
      (cita) => setCitaQr(cita),
      (cita) => navigate(`/eventos/${cita.id}?editar=1`),
      (cita) => setCitaAEliminar(cita),
    )

    function borrarCita() {
      if (!citaAEliminar) return
      eliminarCita(citaAEliminar.id)
      setCitaAEliminar(null)
    }

    //Funcion encargada de manejar el formulario de nuevas citas
    function registrarAcceso(evento: FormEvent<HTMLFormElement>) {
      evento.preventDefault()
      
      agregarCita({
        ...nuevoAcceso,
        estado: 'Activo',
        qr: 'Pendiente',        
        qrToken: '',        
        historial: [],
      })
      
      setNuevoAcceso({
        visitante: '',
        tipo: '',
        fecha: '',
        hora: '',
        horaFin: '',
      })
  setModalNuevoAbierto(false)
}
return (
  <>
  {/*En móvil los filtros se apilan y en pantallas sm se alinean en una fila*/}
      <section className="mb-5 flex flex-wrap items-center justify-between gap-4">
        <div>
        <h2 className="text-xl font-semibold text-slate-900">
          Citas/Accesos
        </h2>
        <p className="mt-1 text-sm text-slate-500">
          Gestiona los accesos programados
        </p>
      </div>

      <button
        type="button"
        onClick={() => setModalNuevoAbierto(true)}
        className="inline-flex items-center gap-2 rounded-md bg-amber-500 px-4 py-2 font-medium text-slate-950 hover:bg-amber-400"
      >
        <Plus size={18} aria-hidden="true" />
        Nuevo acceso
      </button>
    </section>
    <BarraBusqueda
      valor={busqueda}
      alCambiar={(valor) => {
        setBusqueda(valor)
        setPaginaActual(1)
      }}
      placeholder="Buscar cita..."
    >
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
        <option>Completado</option>
      </select>
    </BarraBusqueda>

    <div className="mt-4">
      <DataTable
        columns={columnasCitas}
        rows={citasPagina}
        getRowKey={(cita) => String(cita.id)}
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
          Nuevo acceso
        </h2>

      <form onSubmit={registrarAcceso} className="mt-5 space-y-4">
        <label className="block text-sm text-slate-600">
          Visitante / Empleado
          <input
            required
            value={nuevoAcceso.visitante}
            onChange={(evento) =>
              setNuevoAcceso({ ...nuevoAcceso, visitante: evento.target.value })
            }
            className="mt-1 h-10 w-full rounded-lg border border-slate-200 px-3"
          />
        </label>

        <label className="block text-sm text-slate-600">
          Tipo de acceso
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
          Fecha
          <input
            required
            type="date"
            value={nuevoAcceso.fecha}
            onChange={(evento) =>
              setNuevoAcceso({ ...nuevoAcceso, fecha: evento.target.value })
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
            Hora de fin
            <input
              required
              type="time"
              value={nuevoAcceso.horaFin}
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
            onClick={() => setModalNuevoAbierto(false)}
            className="rounded-lg border border-slate-200 px-4 py-2 text-sm text-slate-600"
          >
            Cancelar
          </button>
          <button
            type="submit"
            className="rounded-lg bg-amber-500 px-4 py-2 text-sm font-semibold text-slate-950"
          >
            Crear acceso
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
            <h2 id="qr-dialog-title" className="text-lg font-semibold text-slate-900">Código QR del acceso #{citaQr.id}</h2>
            <button type="button" onClick={() => setCitaQr(null)} aria-label="Cerrar QR" className="rounded-md p-2 text-slate-500 hover:bg-slate-100">
              <X size={18} />
            </button>
          </div>
          {citaQr.qr === 'Generado' ? (
            <div className="flex flex-col items-center gap-4">
              <div className="rounded-xl border border-slate-200 bg-white p-3 shadow-sm">
                <QRCodeSVG value={citaQr.qrToken} size={220} level="M" includeMargin />
              </div>
              <p className="text-center text-sm text-slate-500">
                QR para <span className="font-medium text-slate-800">{citaQr.visitante}</span>
              </p>
              <p className="text-center text-xs text-amber-700">Código de demostración; todavía no está vinculado a la API.</p>
            </div>
          ) : (
            <div className="rounded-lg bg-amber-50 p-4 text-sm text-amber-800">
              Este acceso todavía no tiene un código QR generado.
            </div>
          )}
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
            Se eliminará la cita de {citaAEliminar.visitante}. Esta acción no se puede deshacer.
          </p>
          <div className="mt-6 flex justify-end gap-2">
            <button type="button" onClick={() => setCitaAEliminar(null)} className="rounded-lg border border-slate-200 px-4 py-2 text-sm text-slate-600 hover:bg-slate-50">Cancelar</button>
            <button type="button" onClick={borrarCita} className="rounded-lg bg-rose-600 px-4 py-2 text-sm font-semibold text-white hover:bg-rose-700">Eliminar acceso</button>
          </div>
        </section>
      </div>
    )}
  </>
)
}
