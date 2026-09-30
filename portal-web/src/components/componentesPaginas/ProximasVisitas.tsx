//Importacion para icono dentro de seccion de proxima visita
import { ChevronRight } from "lucide-react"
//Importacion para conexion con interfaces
import {Link} from 'react-router-dom'

//Variable con la informacion de las visitas
const visitas = [
    { hora: '08:00', nombre: 'Carlos Mendoza', detalle: 'Turno matutino' },
    { hora: '09:30', nombre: 'Grupo Logística NL', detalle: 'Visita proveedor' },
    { hora: '11:00', nombre: 'Ing. Ramírez Torres', detalle: 'Supervisión IMSS' },
    { hora: '14:00', nombre: 'Mantenimiento Rápido SA', detalle: 'Mantenimiento' },
]

export default function ProximasVisitas(){
    return(
        //Seccion para las proximas visitas
        <section className="overflow-hidden rounded-xl border border-slate-200 bg-white shadow-sm">
            {/*Seccion de ver mas la cual envia a la interfaz de citas*/}
            <div className="flex min-h-14 items-center justify-between border-b border-slate-100 px-5">
                <h2 className="text-base font-semibold text-slate-800">Proximas visitas</h2>
                <Link to="/eventos" className="flex items-center gap-1 text-sm font-medium text-orange-400">
                 {/*Vinculo a interfaz de citas*/}
                  Ver todas <ChevronRight size={20}/>
                </Link>
            </div>

            {/*Funcion para crear y mostrar los datos de cada visita */}
            {visitas.map((visita) =>(
                <div key={visita.hora}
                className="flex min-h-[72px] items-center gap-4 border-b border-slate-100 px-5 last:border-0">
                    <time className="w-12 shrink-0 font-mono text-sm font-semibold text-slate-400">
                        {visita.hora}
                    </time>
                    <div className="min-w-0 flex-1">
                        <p className="truncate text-sm font-semibold text-slate-800">{visita.nombre}</p>
                        <p className="truncate text-sm text-slate-400">{visita.detalle}</p>
                    </div>
                    <span className="shrink-0 rounded-full bg-emerald-100 px-2.5 py-1 text-xs font-semibold text-emerald-700">
                        Activo
                    </span>
                </div>

            ))}

        </section>
    )
}