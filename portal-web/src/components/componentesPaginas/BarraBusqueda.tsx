import type { ReactNode } from 'react'
//Icono para barra de busqueda
import { Search } from 'lucide-react'

//Variable para barra de busqueda
type BarraBusquedaProps = {
  valor: string
  alCambiar: (nuevoValor: string) => void
  placeholder?: string
  children?: ReactNode
}

export default function BarraBusqueda({
  valor,
  alCambiar,
  placeholder = 'Buscar...',
  children,
}: BarraBusquedaProps) {
  return (
    <section className="flex flex-col gap-3 rounded-xl border border-slate-200 bg-white p-3 shadow-sm sm:flex-row sm:items-center">
      {/*En teléfono los filtros bajan debajo de la búsqueda; desde sm se alinean en una fila*/}
      <label className="flex min-w-0 flex-1 items-center gap-2 rounded-lg border border-slate-200 px-3">
        <Search
          size={18}
          aria-hidden="true"
          className="shrink-0 text-slate-400"
        />
        <span className="sr-only">{placeholder}</span>
        <input
          value={valor}
          onChange={(evento) => alCambiar(evento.target.value)}
          placeholder={placeholder}
          className="h-11 min-w-0 flex-1 bg-transparent text-sm outline-none"
        />
      </label>

      {children && (
        <div className="flex w-full flex-wrap items-center gap-2 sm:w-auto">
          {children}
        </div>
      )}
    </section>
  )
}