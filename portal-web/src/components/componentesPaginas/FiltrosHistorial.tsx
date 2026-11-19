import { useState } from "react"

export type ValoresFiltrosHistorial = {
    fechaInicio: string
    fechaFin: string
    resultado: string
    usuario: string
}

type FiltrosHistorialProps = {
  alAplicar: (filtros: ValoresFiltrosHistorial) => void
    usuarios: string[]
}


export default function FiltroHistorial({
    alAplicar,
    usuarios,
}: FiltrosHistorialProps) {
    
    const [fechaInicio, setFechaInicio] = useState('')
    const [fechaFin, setFechaFin] = useState('')
    const [resultado, setResultado] = useState('Todos')
    const [usuario, setUsuario] = useState('Todos')

    return (
        //Contenedor principal
        <section className="grid grid-cols-1 items-end gap-3 rounded-xl border border-slate-200 bg-white p-3 shadow-sm sm:grid-cols-2 lg:grid-cols-5">
            {/*Filtro de fecha inicial*/}
            <label className="flex min-w-0 w-full flex-col gap-1 text-sm text-slate-400">
                Fecha inicio
            <input
            type="date"
            value={fechaInicio}
            onChange={(evento) => setFechaInicio(evento.target.value)}
            className="h-10 min-w-0 w-full rounded-lg border border-slate-200 px-3 text-slate-700"/>
            </label>
            {/*Filtro de fecha final*/}
            <label className="flex min-w-0 w-full flex-col gap-1 text-sm text-slate-400">
                Fecha fin
            <input
            type="date"
            value={fechaFin}
            onChange={(evento) => setFechaFin(evento.target.value)}
            className="h-10 min-w-0 w-full rounded-lg border border-slate-200 px-3 text-slate-700"/>
            </label>
        
        {/*Filtro por resultado*/}
        <label className="flex min-w-0 w-full flex-col gap-1 text-sm text-slate-400">
                Resultado
                <select
                value={resultado}                 
                onChange={(evento) => setResultado(evento.target.value)}
                aria-label="Filtrar por resultado"
                className="h-10 min-w-0 w-full rounded-lg border border-slate-200 bg-white px-3 text-sm text-slate-700">

            <option>Todos</option>
            <option>Autorizado</option>
            <option>Rechazado</option>
        </select>
        </label>
        
        {/*Filtro por usuario*/}
        <label className="flex min-w-0 w-full flex-col gap-1 text-sm text-slate-400">
                Usuario
                <select
                value={usuario}
                onChange={(evento) => setUsuario(evento.target.value)}
                aria-label="Filtrar por usuario"
                className="h-10 min-w-0 w-full rounded-lg border border-slate-200 bg-white px-3 text-sm text-slate-700">
            
            <option>Todos</option>
            {usuarios.map((nombreUsuario) => <option key={nombreUsuario}>{nombreUsuario}</option>)}
        </select>
        </label>

        {/*Boton para realizar el filtro*/}
        <button
        type="button"
        onClick={() => alAplicar({fechaInicio, fechaFin, resultado, usuario})}
        className="h-10 w-full rounded-lg border border-amber-300 bg-amber-200 px-4 font-medium text-amber-700 hover:bg-amber-400">
            Filtrar
        </button>
</section>
    )
}
