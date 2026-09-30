//Importacion de iconos
import { ChevronRight } from 'lucide-react'
//Importacion para manejo de rutas
import { Link } from 'react-router-dom'

//Constante en la cual se almacenara la informacion de actividad reciente
const actividades = [
  { texto: 'Acceso autorizado - Carlos Mendoza', hora: '07:58', color: 'bg-emerald-500' },
  { texto: 'Nuevo acceso programado - Grupo Logística NL', hora: '08:03', color: 'bg-blue-500' },
  { texto: 'QR generado - Ing. Ramírez Torres', hora: '08:15', color: 'bg-amber-500' },
  { texto: 'Acceso rechazado - PIN incorrecto - Lector Norte', hora: '09:02', color: 'bg-rose-500' },
  { texto: 'Incidente registrado - Lector QR Almacén inactivo', hora: '10:11', color: 'bg-orange-500' },
]

export default function ActividadReciente(){
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
            {actividades.map((actividad) =>( 
                <div key={actividad.hora}
                className="flex min-h-12 items-center gap-3 px-5 last:border-0">

                    <span aria-hidden="true" className={`h-2 w-2 shrink-0 rounded-full ${actividad.color}`}/>
                    <p className="min-w-0 flex-1 truncate text-sm text-slate-600">{actividad.texto}</p>
                    <time className="shrink-0 font-mono text-xs text-slate-400">{actividad.hora}</time>
                </div>
            ))}
        </section>
    )
}