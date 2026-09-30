//Importacion para iconos dentro de las tarjetas
import type { LucideIcon } from "lucide-react"

//Definicion de los valores a emplear dentro de las cartas de estadisticas
type CartaEstadisticaProps = {
    title: string
    value: string
    subtitle: string
    icon?: LucideIcon   //Icono aplicado a cada tarjeta
    iconClassName?: string
    variant?: 'default' | 'compact'
    className?: string
}

export default function CartaEstadistica({
    title,
    value,
    subtitle,
    icon: Icon,
    iconClassName,
    variant = 'default',
    className = '',
}:CartaEstadisticaProps){
    const isCompact = variant === 'compact'

    return(
        <article className={`${isCompact ? 'flex min-h-[84px] items-center justify-between rounded-xl border border-slate-200 bg-white px-4 py-3' : 'min-h-40 rounded-xl border border-slate-200 bg-white p-5'} shadow-sm ${className}`}>
            <div>
            {Icon && !isCompact && (
                <div className={`flex h-11 w-11 items-center justify-center rounded-xl text-white ${iconClassName ?? ''}`}>
                    <Icon aria-hidden="true" size={21}/>
                </div>
            )}
            <strong className={`${isCompact ? 'block text-2xl' : 'mt-3 block text-3xl'} font-bold leading-none text-slate-900`}>
                {value}
            </strong>

            <p className={`${isCompact ? 'mt-2' : 'mt-1'} text-sm font-medium text-slate-800`}>{title}</p>
            {subtitle && <span className="text-xs text-slate-400">{subtitle}</span>}
            </div>
            {Icon && isCompact && (
                <div className={`flex h-11 w-11 shrink-0 items-center justify-center rounded-xl text-white ${iconClassName ?? ''}`}>
                    <Icon aria-hidden="true" size={21}/>
                </div>
            )}
        </article>
    )
}