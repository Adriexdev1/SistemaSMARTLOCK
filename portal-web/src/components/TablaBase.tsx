//Importacion para pie de tabla
import type { ReactNode } from "react"

//Definicion para las columnas de la tabla
type Column<T> = {
  header: string
  width?: string
  render: (row: T) => React.ReactNode
}

type DataTableProps<T> = {
  columns: Column<T>[]
  rows: T[]
  getRowKey: (row: T) => string
  footer?: ReactNode
}

export default function DataTable<T>({
  columns,
  rows,
  getRowKey,
  footer,
}: DataTableProps<T>) {
  return (
    //Definicion de estructura de la tabla
    <div className="w-full overflow-x-auto rounded-xl border border-slate-200 bg-white shadow-sm">
      {/*La tabla mantiene un ancho legible y se desplaza horizontalmente en teléfonos*/}
      <table className="w-full min-w-[760px] table-fixed text-left text-sm">
        <thead className="bg-slate-50 text-xs uppercase text-slate-500">
          <tr>
            {columns.map((column) => (
              <th key={column.header} style={{width: column.width}} className="px-5 py-4 font-semibold">
                {column.header}
              </th>
            ))}
          </tr>
        </thead>

        <tbody>
          {rows.map((row) => (
            <tr key={getRowKey(row)} className="border-t border-slate-100">
              {columns.map((column) => (
                <td key={column.header} style={{width: column.width}} className="px-5 py-4">
                  {column.render(row)}
                </td>
              ))}
            </tr>
          ))}

          {rows.length === 0 && (
            <tr>
              <td colSpan={columns.length} className="px-5 py-8 text-center text-slate-500">
                No hay resultados
              </td>
            </tr>
          )}
        </tbody>
      </table>
      
      {/*Pie de Pagina de la tabla*/}
      {footer && (
        <div className="border-t border-slate-100">
            {footer}
        </div>
      )}
    </div>
  )
}