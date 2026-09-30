//Variable para definir el tipo de dato en la paginacion de las tablas
type PaginacionProps = {
  paginaActual: number
  tamanoPagina: number
  totalElementos: number
  etiqueta: string
  alCambiarPagina: (pagina: number) => void
}

export default function Paginacion({
  paginaActual,
  tamanoPagina,
  totalElementos,
  etiqueta,
  alCambiarPagina,
}: PaginacionProps) {
    //Constantes para determinar el total de paginas, y los rangos que abarca la pagina
  const totalPaginas = Math.ceil(totalElementos / tamanoPagina)
  const desde = totalElementos === 0
    ? 0
    : (paginaActual - 1) * tamanoPagina + 1
  const hasta = Math.min(paginaActual * tamanoPagina, totalElementos)

  return (
    <footer className="flex flex-wrap items-center justify-between gap-3 bg-slate-50 px-4 py-3 text-sm text-slate-500 sm:px-5">
      {/*El pie puede partirse en dos filas cuando el ancho del teléfono es reducido*/}
      <span>
        Mostrando {desde}–{hasta} de {totalElementos} {etiqueta}
      </span>

      <nav aria-label={`Paginación de ${etiqueta}`} className="flex items-center gap-1">
        {Array.from({ length: totalPaginas }, (_, indice) => indice + 1).map((pagina) => (
          <button
            key={pagina}
            type="button"
            aria-current={paginaActual === pagina ? 'page' : undefined}
            onClick={() => alCambiarPagina(pagina)}
            className={`h-9 min-w-9 rounded-md px-3 ${
              paginaActual === pagina
                ? 'bg-amber-500 font-semibold text-slate-950'
                : 'text-slate-600 hover:bg-slate-200'
            }`}
          >
            {pagina}
          </button>
        ))}
      </nav>
    </footer>
  )
}