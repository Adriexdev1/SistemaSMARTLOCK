import { ChevronDown, ListFilter } from 'lucide-react'

export type CampoBusqueda = {
  clave: string
  etiqueta: string
}

type SelectorCamposBusquedaProps = {
  campos: CampoBusqueda[]
  seleccionados: string[]
  alCambiar: (seleccionados: string[]) => void
}

export default function SelectorCamposBusqueda({
  campos,
  seleccionados,
  alCambiar,
}: SelectorCamposBusquedaProps) {
  function alternarCampo(clave: string, seleccionado: boolean) {
    alCambiar(seleccionado
      ? [...seleccionados, clave]
      : seleccionados.filter((actual) => actual !== clave))
  }

  return (
    <details className="group relative w-full sm:w-auto">
      <summary className="flex h-11 cursor-pointer list-none items-center justify-between gap-2 rounded-lg border border-slate-200 bg-white px-3 text-sm text-slate-700 [&::-webkit-details-marker]:hidden">
        <span className="flex items-center gap-2">
          <ListFilter size={16} aria-hidden="true" />
          Campos ({seleccionados.length}/{campos.length})
        </span>
        <ChevronDown size={16} aria-hidden="true" className="transition-transform group-open:rotate-180" />
      </summary>
      <div className="absolute left-0 z-30 mt-2 max-h-80 w-full min-w-56 overflow-y-auto rounded-lg border border-slate-200 bg-white p-3 shadow-lg sm:left-auto sm:right-0">
        <div className="mb-2 flex items-center justify-between gap-3">
          <span className="text-xs font-semibold uppercase tracking-wide text-slate-500">Buscar en</span>
          <div className="flex gap-2">
            <button type="button" onClick={() => alCambiar(campos.map((campo) => campo.clave))} className="text-xs font-medium text-blue-700 hover:underline">
              Todos
            </button>
            <button type="button" onClick={() => alCambiar([])} className="text-xs font-medium text-slate-600 hover:underline">
              Ninguno
            </button>
          </div>
        </div>
        <div className="space-y-1">
          {campos.map((campo) => (
            <label key={campo.clave} className="flex cursor-pointer items-center gap-2 rounded px-2 py-1.5 text-sm text-slate-700 hover:bg-slate-50">
              <input
                type="checkbox"
                checked={seleccionados.includes(campo.clave)}
                onChange={(evento) => alternarCampo(campo.clave, evento.target.checked)}
                className="h-4 w-4 rounded border-slate-300 accent-amber-500"
              />
              {campo.etiqueta}
            </label>
          ))}
        </div>
      </div>
    </details>
  )
}
