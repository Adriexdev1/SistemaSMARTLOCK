//Definicion de los valores a emplear dentro de las cartas de estadisticas
type CartaEstadisticaProps = {
    title: string
    value: string
    subtitle: string
}

export default function CartaEstadistica({
    title,
    value,
    subtitle,
}:CartaEstadisticaProps){
    return(
        <article className="rounded-2xl border-5 border-black bg-white p-5 shadow-sm">
            <strong className="mt-2 block text-3xl text-slate-900">{value}</strong>
            <p className="text-sm text-slate-500">{title}</p>
            <span className="text-sm text-slate-400">{subtitle}</span>
        </article>
    )
}