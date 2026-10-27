//Importacion de iconos
import { ChevronRight } from 'lucide-react'
//Importacion para manejo de rutas
import { Link } from 'react-router-dom'

type Actividad = { texto: string; hora: string; color: string }

type ActividadRecienteProps = { actividades: Actividad[]; cargando: boolean }

export default function ActividadReciente({ actividades, cargando }: ActividadRecienteProps){
    return(
        <section className="overflow-hidden rounded-xl border border-slate-200 bg-white shadow-sm">
            {/*Seccion en donde estara el texto link hacia el historial*/}
            <div className="flex min-h-14 items-center justify-between border-b border-slate-100 px-5">
                <h2 className="text-base font-semibold text-slate-800">Actividad Reciente</h2>
                {/*Link hacia historial*/}
                <Link to="/historial" className="flex items-center gap-1 text-sm font-medium text-orange-400">
                 Ver todas <ChevronRight size={20}/>
                </Link>
            </div>

            {/*Funcion para incorporar la actividad reciente al contenedor*/}
            {cargando ? (
                <p className="px-5 py-6 text-sm text-slate-500">Cargando actividad...</p>
            ) : actividades.length === 0 ? (
                <p className="px-5 py-6 text-sm text-slate-500">Todavía no hay actividad registrada.</p>
            ) : actividades.map((actividad) =>(
                <div key={`${actividad.hora}-${actividad.texto}`}
                className="flex min-h-12 items-center gap-3 px-5 last:border-0">

                    <span aria-hidden="true" className={`h-2 w-2 shrink-0 rounded-full ${actividad.color}`}/>
                    <p className="min-w-0 flex-1 truncate text-sm text-slate-600">{actividad.texto}</p>
                    <time className="shrink-0 font-mono text-xs text-slate-400">{actividad.hora}</time>
                </div>
            ))}
        </section>
    )
}