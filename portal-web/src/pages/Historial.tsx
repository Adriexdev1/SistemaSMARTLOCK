import { Download } from "lucide-react"
import Paginacion from "../components/PieTabla"
import DataTable from "../components/TablaBase"
import { useState } from "react"
import FiltroHistorial, {type ValoresFiltrosHistorial} from "../components/componentesPaginas/FiltrosHistorial"

//Definicion de estructura para tabla
type Historial = {
  id: number
  fecha: string
  hora: string
  usuario: string
  cita: number
  resultado: 'Autorizado' | 'Rechazado'
  metodo_acceso: 'QR + PIN' | 'Solo QR' | 'QR invalido' | 'PIN incorrecto'
}

const historial: Historial[] = [
  {
    id: 1,
    fecha: '2026-08-28',
    hora: '08:00:00',
    usuario:'Carlos Mendoza',
    cita: 1,
    resultado: 'Autorizado',
    metodo_acceso:'QR + PIN',
  },
    {
    id: 2,
    fecha: '2026-08-28',
    hora: '08:00:00',
    usuario:'Carlos Mendoza',
    cita: 2,
    resultado: 'Rechazado',
    metodo_acceso:'QR + PIN',
  },
]

const coloresResultado: Record<Historial['resultado'], string> ={
    Autorizado: 'bg-emerald-100 text-emerald-700',
    Rechazado: 'bg-rose-100 text-rose-700',
}

const columnasHistorial = [
  {
    header: 'Fecha',
    width: '12%',
    render: (registro: Historial) => {
        const [anio, mes, dia] = registro.fecha.split('-')
        return (
            <span className="font-mono text-slate-500">
                {dia}/{mes}/{anio}
            </span>
        )
    },
  },
  {
    header: 'Hora',
    width: '18%',
    render: (registro: Historial) => (
        <span className="font-mono font-semibold">
            {registro.hora}
        </span>
    )
  },
  {
    header: 'Usuario',
    width: '20%',
    render: (registro: Historial) => (
        <span className="font-mono font-semibold">
        {registro.usuario}
        </span>
    )
  },
  {
    header: 'Cita',
    width: '10%',
    render: (registro: Historial) => 
        <span className="font-mono text-base font-semibold text-orange-400"> 
        #{registro.cita}
        </span>
  },
  {
    header: 'Resultado',
    width: '20%',
    render: (registro: Historial) => (
        <span className={`rounded-full px-2.5 py-1 text-xs font-semibold ${coloresResultado[registro.resultado]}`}>
        {registro.resultado}
        </span>
    )
  },
  {
    header: 'Método',
    width: '20%',
    render: (registro: Historial) => (
        <span className="font-mono text-slate-500">
        {registro.metodo_acceso}
        </span>
    )
  },
]

export default function Historial(){
    //Constantes para determinar paginas
    const [paginaActual, setPaginaActual] = useState(1)
    const [filtros, setFiltros] = useState<ValoresFiltrosHistorial>({
      fechaInicio: '',
      fechaFin: '',
      resultado: 'Todos',
      usuario: 'Todos',
    })
    const tamanoPagina = 5
    
    const registrosFiltrados = historial.filter((registro) => {
      const cumpleFechaInicio =
        filtros.fechaInicio === '' || registro.fecha >= filtros.fechaInicio
      const cumpleFechaFin =
        filtros.fechaFin === '' || registro.fecha <= filtros.fechaFin
      const cumpleResultado =
        filtros.resultado === 'Todos' || registro.resultado === filtros.resultado
      const cumpleUsuario =
        filtros.usuario === 'Todos' || registro.usuario === filtros.usuario

      return (
        cumpleFechaInicio &&
        cumpleFechaFin &&
        cumpleResultado &&
        cumpleUsuario
      )
    })
    const pagina = Math.max(
        1,Math.min(paginaActual, Math.ceil(registrosFiltrados.length / tamanoPagina))
    )
    
    const inicio = (pagina - 1) * tamanoPagina
    const registrosPagina = registrosFiltrados.slice(
        inicio, inicio + tamanoPagina)

    //Recuento de Registros totales - Autorizados y Rechazados 
    //Constante para total de registros
    const totalRegistros = registrosFiltrados.length
    //Registros autorizados
    const totalAutorizados = registrosFiltrados.filter(
        (registro) => registro.resultado === 'Autorizado'
    ).length
    //Registros rechazados
    const totalRechazados = registrosFiltrados.filter(
        (registro) => registro.resultado === 'Rechazado'
    ).length

    return (
        <>
        <section className="mb-5 flex flex-wrap items-center justify-between gap-4">
        <div>
        <h2 className="text-xl font-semibold text-slate-900">
          Historial de Accesos
        </h2>
        <p className="mt-1 text-sm text-slate-500">
          Consulta los registros de los accesos al sistema
        </p>
      </div>

      <button
        type="button"
        className="inline-flex items-center gap-2 rounded-md bg-green-300 px-4 py-2 font-medium text-slate-900 hover:bg-green-500">
        
        <Download size={18} aria-hidden="true" />
        Exportar
      </button>
    </section>

    {/*Seccion de los filtros*/}
    <FiltroHistorial
      alAplicar={(nuevosFiltros) => {
        setFiltros(nuevosFiltros)
        setPaginaActual(1)
      }}
    />

    {/*Los contadores pueden pasar a otra fila cuando el ancho del teléfono es corto*/}
    {/*Seccion de Cantidaddes de registros*/}
    <section className="flex flex-wrap items-center gap-3 py-4 text-sm">
        <span className="font-semibold text-slate-700">
            {totalRegistros} Total registros
        </span>
        
        <span className="rounded-full bg-emerald-100 px-4 py-2 font-semibold text-emerald-700">
            {totalAutorizados} Autorizados
        </span>
        
        <span className="rounded-full bg-rose-100 px-4 py-2 font-semibold text-rose-700">
            {totalRechazados} Rechazados
        </span>
    </section>

    <DataTable
    columns={columnasHistorial}
    rows={registrosPagina}
    getRowKey={(registro) => String(registro.id)}
    footer={
    <Paginacion
      paginaActual={paginaActual}
      tamanoPagina={tamanoPagina}
      totalElementos={registrosFiltrados.length}
      etiqueta="registros"
      alCambiarPagina={setPaginaActual}
    />
  }
  />
  </>
 )
}