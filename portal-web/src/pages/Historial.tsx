import { Download } from "lucide-react"
import Paginacion from "../components/PieTabla"
import DataTable from "../components/TablaBase"
import { useEffect, useState } from "react"
import FiltroHistorial, {type ValoresFiltrosHistorial} from "../components/componentesPaginas/FiltrosHistorial"
import * as XLSX from 'xlsx'

//Definicion de estructura para tabla
type Historial = {
  id: number
  fecha: string
  hora: string
  usuario: string
  cita: number
  resultado: string
  metodo_acceso: string
}

type HistorialApi = {
  id: number
  fecha: string
  hora: string
  usuario: string
  idDetalleEvento: number
  resultado: string
  metodoAcceso: string
}

const apiUrl = import.meta.env.VITE_API_URL || 'http://localhost:5000'

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
  const [historial, setHistorial] = useState<Historial[]>([])
  const [cargando, setCargando] = useState(true)
  const [error, setError] = useState('')
    //Constantes para determinar paginas
    const [paginaActual, setPaginaActual] = useState(1)
    const [filtros, setFiltros] = useState<ValoresFiltrosHistorial>({
      fechaInicio: '',
      fechaFin: '',
      resultado: 'Todos',
      usuario: 'Todos',
    })
    const tamanoPagina = 5

    useEffect(() => {
      async function cargarHistorial() {
        setCargando(true)
        setError('')
        try {
          const respuesta = await fetch(`${apiUrl}/api/historial`)
          if (!respuesta.ok) throw new Error('No se pudo cargar el historial.')
          const datos = await respuesta.json() as HistorialApi[]
          setHistorial(datos.map((registro) => ({
            id: registro.id,
            fecha: registro.fecha,
            hora: registro.hora,
            usuario: registro.usuario,
            cita: registro.idDetalleEvento,
            resultado: registro.resultado,
            metodo_acceso: registro.metodoAcceso,
          })))
        } catch (errorCarga) {
          setError(errorCarga instanceof Error ? errorCarga.message : 'No se pudo conectar con la API.')
        } finally {
          setCargando(false)
        }
      }

      void cargarHistorial()
    }, [])
    
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

    //Constante para total de registros
    const totalRegistros = registrosFiltrados.length
    //Registros autorizados
    const totalAutorizados = registrosFiltrados.filter(
        (registro) => registro.resultado === 'Autorizado'
    ).length
    //Funcion para trabajar con los datos en un excel
    function exportarHistorial() {
      const datosExcel = registrosFiltrados.map((registro) => {
        const [anio, mes, dia] = registro.fecha.split('-')
        
        return {
          Fecha: `${dia}/${mes}/${anio}`,
          Hora: registro.hora,
          Usuario: registro.usuario,
          Cita: `#${registro.cita}`,
          Resultado: registro.resultado,
          Método: registro.metodo_acceso,
        }
  })

  const hoja = XLSX.utils.json_to_sheet(datosExcel)
  const libro = XLSX.utils.book_new()

  XLSX.utils.book_append_sheet(libro, hoja, 'Historial de accesos')
  XLSX.writeFile(libro, 'historial-accesos.xlsx')
}

    return (
        <>
        <section className="mb-5 flex flex-wrap items-center justify-between gap-4">
        <div>
        <h2 className="text-xl font-semibold text-slate-900">
          Historial de Accesos
        </h2>
        <p className="mt-1 text-sm text-slate-500">
          Consulta los accesos completados registrados en el sistema
        </p>
      </div>

      <button
        type="button"
        onClick={exportarHistorial}
        disabled={cargando || registrosFiltrados.length === 0}
        className="inline-flex items-center gap-2 rounded-md bg-green-300 px-4 py-2 font-medium text-slate-900 hover:bg-green-500">
        
        <Download size={18} aria-hidden="true" />
        Exportar
      </button>
    </section>

    {error && <p role="alert" className="mb-4 rounded-md border border-rose-200 bg-rose-50 px-4 py-3 text-sm text-rose-700">{error}</p>}
    {cargando && <p role="status" className="py-8 text-center text-sm text-slate-500">Cargando historial...</p>}

    {/*Seccion de los filtros*/}
    <FiltroHistorial
      usuarios={[...new Set(historial.map((registro) => registro.usuario))]}
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
        
    </section>

    <DataTable
    columns={columnasHistorial}
    rows={cargando ? [] : registrosPagina}
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