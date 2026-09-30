//Importacion para iconos dentro de las tarjetas
import type { LucideIcon } from "lucide-react"

//Definicion de los valores a emplear dentro de las cartas de estadisticas
type CartaEstadisticaProps = {
    title: string
    value: string
    subtitle: string
    icon: LucideIcon   //Icono aplicado a cada tarjeta
    iconClassName: string
}

export default function CartaEstadistica({
    title,
    value,
    subtitle,
    icon: Icon,
    iconClassName,
}:CartaEstadisticaProps){
    return(
        <article className="min-h-40 rounded-xl border border-slate-200 bg-white p-5 shadow-sm">
            {/*Campo del icono*/}
            <div className= {`flex h-11 w-11 items-center justify-center rounded-xl text-white ${iconClassName}`}>
                <Icon aria-hidden= "true" size={21}/>
            </div>
            {/*Campo de texto*/}
            <strong className="mt-3 block text-3xl font-bold leading-none text-slate-900">
                {value}
            </strong>

            <p className="mt-1 text-md font-medium text-slate-800">{title}</p>
            <span className="text-xs text-slate-400">{subtitle}</span>
        </article>
    )
}