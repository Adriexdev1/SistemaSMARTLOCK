import { Eye, QrCode, Pencil, Trash, Plus } from "lucide-react"
//Importacion de tabla base
import DataTable  from "../components/TablaBase"
//Importacion de pie de tabla
import Paginacion from "../components/PieTabla"
//Libreria para realizar cambios en la informacion mostrada al realizar busquedas
import { useState } from "react"
//Importacion de la barra de busqueda
import BarraBusqueda from "../components/componentesPaginas/BarraBusqueda"

//Definicion de estructura para tabla
type Cita = {
  id: number
  fecha: string
  hora: string
  visitante: string
  tipo: string
  estado: 'Activo' | 'Completado'
  qr: 'Generado' | 'Pendiente' | 'Utilizado'
}

//Variable con datos a mostrar en la tabla
const citas: Cita[] = [
  {
    id: 1,
    fecha: '2026-08-28',
    hora: '08:00',
    visitante: 'Carlos Mendoza',
    tipo: 'Turno matutino',
    estado: 'Activo',
    qr: 'Generado',
  },
  {
    id: 2,
    fecha: '2026-08-28',
    hora: '09:30',
    visitante: 'Grupo Logística NL',
    tipo: 'Visita proveedor',
    estado: 'Activo',
    qr: 'Pendiente',
  },
]

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
const columnasCitas = [
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
        <button type="button" aria-label={`Ver cita ${cita.id}`} className="flex h-8 w-8 items-center justify-center rounded-md text-slate-500 transition-colors hover:bg-slate-100 hover:text-slate-900 focus-visible:outline-2 focus-visible:outline-slate-400">
            <Eye size={16} strokeWidth={2.5} />
        </button>
        {/*Boton de ver QR*/}
        <button type="button" aria-label={`Mostrar QR de cita ${cita.id}`} className="flex h-8 w-8 items-center justify-center rounded-md text-violet-600 transition-colors hover:bg-violet-100 hover:text-violet-800 focus-visible:outline-2 focus-visible:outline-violet-400">
            <QrCode size={16} strokeWidth={2.5}/>
        </button>
        {/*Boton de editar cita*/}
        <button type="button" aria-label={`Editar cita ${cita.id}`} className="flex h-8 w-8 items-center justify-center rounded-md text-blue-600 transition-colors hover:bg-blue-100 hover:text-blue-800 focus-visible:outline-2 focus-visible:outline-blue-400">
            <Pencil size={16} strokeWidth={2.5}/>
        </button>
        {/*Boton de eliminar cita*/}
        <button type="button" aria-label={`Eliminar cita ${cita.id}`} className="flex h-8 w-8 items-center justify-center rounded-md text-rose-600 transition-colors hover:bg-rose-100 hover:text-rose-800 focus-visible:outline-2 focus-visible:outline-rose-400">
            <Trash size={16} strokeWidth={2.5}/>
            </button>
    </div>
    ),
  },
]

//Funcion para ignorar mayusculas y acentos en las busquedas
function normalizar(texto: string) {
  return texto
    .normalize('NFD')
    .replace(/[\u0300-\u036f]/g, '')
    .toLowerCase()
    .trim()
}

export default function Citas(){
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
            `${cita.id} ${cita.fecha} ${fechaLegible} ${cita.hora} ` +
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
  </>
)
}
