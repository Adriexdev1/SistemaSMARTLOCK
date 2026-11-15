import { useEffect, useState, type FormEvent } from 'react'
import { Pencil, Plus, Trash2 } from 'lucide-react'
import BarraBusqueda from '../components/componentesPaginas/BarraBusqueda'
import SelectorCamposBusqueda, { type CampoBusqueda } from '../components/componentesPaginas/SelectorCamposBusqueda'
import DataTable from '../components/TablaBase'
import Paginacion from '../components/PieTabla'

type Rol = {
  id: number
  nombre: string
  descripcion: string
  permiso: number
  estado: number
  fecha: string
}

type RolApi = {
  idRol: number
  nombreRol: string
  descripcionRol: string | null
  permisoRol: number
  estadoRol: number
  fechaRol: string | null
}

const apiUrl = import.meta.env.VITE_API_URL || 'http://localhost:5000'

const camposBusquedaRol: CampoBusqueda[] = [
  { clave: 'id', etiqueta: 'ID' },
  { clave: 'nombre', etiqueta: 'Nombre' },
  { clave: 'descripcion', etiqueta: 'Descripción' },
  { clave: 'permiso', etiqueta: 'Nivel de permiso' },
  { clave: 'estado', etiqueta: 'Estado' },
  { clave: 'fecha', etiqueta: 'Fecha de registro' },
]

function adaptarRol(rol: RolApi): Rol {
  return {
    id: rol.idRol,
    nombre: rol.nombreRol,
    descripcion: rol.descripcionRol ?? '',
    permiso: rol.permisoRol,
    estado: rol.estadoRol,
    fecha: rol.fechaRol ? new Date(rol.fechaRol).toLocaleDateString('es-MX') : 'Sin fecha',
  }
}

function normalizar(texto: string) {
  return texto
    .normalize('NFD')
    .replace(/[\u0300-\u036f]/g, '')
    .toLowerCase()
    .trim()
}

async function leerMensajeError(respuesta: Response) {
  try {
    const cuerpo = await respuesta.json() as { mensaje?: string; title?: string }
    return cuerpo.mensaje ?? cuerpo.title ?? 'No se pudo completar la operación.'
  } catch {
    return 'No se pudo completar la operación.'
  }
}

export default function Roles() {
  const [roles, setRoles] = useState<Rol[]>([])
  const [cargando, setCargando] = useState(true)
  const [guardando, setGuardando] = useState(false)
  const [error, setError] = useState('')
  const [busqueda, setBusqueda] = useState('')
  const [camposBusqueda, setCamposBusqueda] = useState<string[]>(() => camposBusquedaRol.map((campo) => campo.clave))
  const [estadoFiltro, setEstadoFiltro] = useState('Todos')
  const [paginaActual, setPaginaActual] = useState(1)
  const [modalAbierto, setModalAbierto] = useState(false)
  const [rolEditado, setRolEditado] = useState<Rol | null>(null)
  const [rolAEliminar, setRolAEliminar] = useState<Rol | null>(null)
  const [borrador, setBorrador] = useState({ nombre: '', descripcion: '', permiso: 0, estado: 1 })

  async function cargarRoles() {
    setCargando(true)
    setError('')
    try {
      const respuesta = await fetch(`${apiUrl}/api/roles`)
      if (!respuesta.ok) throw new Error(await leerMensajeError(respuesta))
      const datos = await respuesta.json() as RolApi[]
      setRoles(datos.map(adaptarRol))
    } catch (errorCarga) {
      setError(errorCarga instanceof Error ? errorCarga.message : 'No se pudo conectar con la API.')
    } finally {
      setCargando(false)
    }
  }

  useEffect(() => {
    void cargarRoles()
  }, [])

  const rolesFiltrados = roles.filter((rol) => {
    const valores: Record<string, string> = {
      id: String(rol.id),
      nombre: rol.nombre,
      descripcion: rol.descripcion,
      permiso: `Nivel ${rol.permiso}`,
      estado: rol.estado === 1 ? 'Activo' : 'Inactivo',
      fecha: rol.fecha,
    }
    const terminos = normalizar(busqueda).split(/\s+/).filter(Boolean)
    const coincideTexto = terminos.every((termino) =>
      camposBusqueda.some((campo) => normalizar(valores[campo] ?? '').includes(termino))
    )
    return coincideTexto && (estadoFiltro === 'Todos' || rol.estado === Number(estadoFiltro))
  })
  const tamanoPagina = 8
  const totalPaginas = Math.max(1, Math.ceil(rolesFiltrados.length / tamanoPagina))
  const pagina = Math.min(paginaActual, totalPaginas)
  const rolesPagina = rolesFiltrados.slice((pagina - 1) * tamanoPagina, pagina * tamanoPagina)

  function iniciarNuevoRol() {
    setError('')
    setRolEditado(null)
    setBorrador({ nombre: '', descripcion: '', permiso: 0, estado: 1 })
    setModalAbierto(true)
  }

  function iniciarEdicion(rol: Rol) {
    setError('')
    setRolEditado(rol)
    setBorrador({ nombre: rol.nombre, descripcion: rol.descripcion, permiso: rol.permiso, estado: rol.estado })
    setModalAbierto(true)
  }

  async function guardarRol(evento: FormEvent<HTMLFormElement>) {
    evento.preventDefault()
    setGuardando(true)
    setError('')
    const edicion = rolEditado !== null

    try {
      const respuesta = await fetch(edicion ? `${apiUrl}/api/roles/${rolEditado.id}` : `${apiUrl}/api/roles`, {
        method: edicion ? 'PUT' : 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          nombreRol: borrador.nombre.trim(),
          descripcionRol: borrador.descripcion.trim() || null,
          permisoRol: borrador.permiso,
          estadoRol: borrador.estado,
        }),
      })

      if (!respuesta.ok) {
        setError(await leerMensajeError(respuesta))
        return
      }

      setRolEditado(null)
      setModalAbierto(false)
      await cargarRoles()
    } catch {
      setError('No se pudo conectar con la API.')
    } finally {
      setGuardando(false)
    }
  }

  async function eliminarRol() {
    if (!rolAEliminar) return
    setGuardando(true)
    setError('')
    try {
      const respuesta = await fetch(`${apiUrl}/api/roles/${rolAEliminar.id}`, { method: 'DELETE' })
      if (!respuesta.ok) {
        setError(await leerMensajeError(respuesta))
        return
      }
      setRoles((actuales) => actuales.filter((rol) => rol.id !== rolAEliminar.id))
      setRolAEliminar(null)
    } catch {
      setError('No se pudo conectar con la API.')
    } finally {
      setGuardando(false)
    }
  }

  const columnas = [
    { header: 'ID', width: '8%', render: (rol: Rol) => <span className="font-mono font-semibold text-slate-700">{rol.id}</span> },
    { header: 'Rol', width: '17%', render: (rol: Rol) => <span className="font-semibold text-slate-800">{rol.nombre}</span> },
    { header: 'Descripción', width: '30%', render: (rol: Rol) => <span className="text-slate-600">{rol.descripcion || 'Sin descripción'}</span> },
    { header: 'Permiso', width: '12%', render: (rol: Rol) => <span className="font-mono text-slate-600">Nivel {rol.permiso}</span> },
    { header: 'Estado', width: '12%', render: (rol: Rol) => <span className={`rounded-full px-2.5 py-1 text-xs font-semibold ${rol.estado === 1 ? 'bg-emerald-100 text-emerald-700' : 'bg-slate-100 text-slate-600'}`}>{rol.estado === 1 ? 'Activo' : 'Inactivo'}</span> },
    { header: 'Registro', width: '11%', render: (rol: Rol) => <span className="font-mono text-sm text-slate-500">{rol.fecha}</span> },
    {
      header: 'Acciones', width: '10%',
      render: (rol: Rol) => (
        <div className="flex items-center gap-1">
          <button type="button" onClick={() => iniciarEdicion(rol)} aria-label={`Editar rol ${rol.nombre}`} className="flex h-9 w-9 items-center justify-center rounded-md text-blue-600 hover:bg-blue-50"><Pencil size={16} /></button>
          <button type="button" onClick={() => { setError(''); setRolAEliminar(rol) }} aria-label={`Eliminar rol ${rol.nombre}`} className="flex h-9 w-9 items-center justify-center rounded-md text-rose-600 hover:bg-rose-50"><Trash2 size={16} /></button>
        </div>
      ),
    },
  ]

  return (
    <>
      <section className="mb-5 flex flex-wrap items-center justify-between gap-4">
        <div>
          <h2 className="text-xl font-semibold text-slate-900">Roles del sistema</h2>
          <p className="mt-1 text-sm text-slate-500">Administra permisos y estados de los roles</p>
        </div>
        <button type="button" onClick={iniciarNuevoRol} className="inline-flex items-center gap-2 rounded-md bg-amber-500 px-4 py-2 font-medium text-slate-950 hover:bg-amber-400">
          <Plus size={18} /> Nuevo rol
        </button>
      </section>

      {error && <p role="alert" className="mb-4 rounded-md border border-rose-200 bg-rose-50 px-4 py-3 text-sm text-rose-700">{error}</p>}
      {cargando && <p role="status" className="py-8 text-center text-sm text-slate-500">Cargando roles...</p>}

      <BarraBusqueda valor={busqueda} alCambiar={(valor) => { setBusqueda(valor); setPaginaActual(1) }} placeholder="Buscar rol...">
        <SelectorCamposBusqueda
          campos={camposBusquedaRol}
          seleccionados={camposBusqueda}
          alCambiar={(campos) => {
            setCamposBusqueda(campos)
            setPaginaActual(1)
          }}
        />
        <select value={estadoFiltro} onChange={(evento) => { setEstadoFiltro(evento.target.value); setPaginaActual(1) }} aria-label="Filtrar por estado" className="h-11 rounded-lg border border-slate-200 bg-white px-3 text-sm text-slate-700">
          <option value="Todos">Todos</option>
          <option value="1">Activo</option>
          <option value="0">Inactivo</option>
        </select>
      </BarraBusqueda>

      <div className="mt-4">
        <DataTable columns={columnas} rows={cargando ? [] : rolesPagina} getRowKey={(rol) => String(rol.id)} footer={
          <Paginacion paginaActual={pagina} tamanoPagina={tamanoPagina} totalElementos={rolesFiltrados.length} etiqueta="roles" alCambiarPagina={setPaginaActual} />
        } />
      </div>

      {modalAbierto && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/45 p-4">
          <section role="dialog" aria-modal="true" aria-labelledby="rol-formulario-titulo" className="w-full max-w-lg rounded-xl bg-white p-5 shadow-xl">
            <h2 id="rol-formulario-titulo" className="text-lg font-semibold text-slate-900">
              {rolEditado ? 'Editar rol' : 'Nuevo rol'}
            </h2>
            <form onSubmit={guardarRol} className="mt-5 space-y-4">
              <label className="block text-sm text-slate-600">
                Nombre del rol
                <input required maxLength={50} value={borrador.nombre} onChange={(evento) => setBorrador({ ...borrador, nombre: evento.target.value })} className="mt-1 h-10 w-full rounded-lg border border-slate-200 px-3" />
              </label>
              <label className="block text-sm text-slate-600">
                Descripción
                <textarea maxLength={150} rows={3} value={borrador.descripcion} onChange={(evento) => setBorrador({ ...borrador, descripcion: evento.target.value })} className="mt-1 w-full rounded-lg border border-slate-200 p-3" />
              </label>
              <label className="block text-sm text-slate-600">
                Nivel de permiso
                <select value={borrador.permiso} onChange={(evento) => setBorrador({ ...borrador, permiso: Number(evento.target.value) })} className="mt-1 h-10 w-full rounded-lg border border-slate-200 bg-white px-3">
                  <option value={0}>0</option><option value={1}>1</option><option value={2}>2</option>
                </select>
              </label>
              <label className="block text-sm text-slate-600">
                Estado
                <select value={borrador.estado} onChange={(evento) => setBorrador({ ...borrador, estado: Number(evento.target.value) })} className="mt-1 h-10 w-full rounded-lg border border-slate-200 bg-white px-3">
                  <option value={1}>Activo</option><option value={0}>Inactivo</option>
                </select>
              </label>
              {error && <p role="alert" className="text-sm text-rose-600">{error}</p>}
              <div className="flex justify-end gap-2 pt-2">
                <button type="button" onClick={() => setModalAbierto(false)} className="rounded-lg border border-slate-200 px-4 py-2 text-sm text-slate-600">Cancelar</button>
                <button type="submit" disabled={guardando} className="rounded-lg bg-amber-500 px-4 py-2 text-sm font-semibold text-slate-950 disabled:cursor-wait disabled:opacity-60">{guardando ? 'Guardando...' : rolEditado ? 'Guardar cambios' : 'Crear rol'}</button>
              </div>
            </form>
          </section>
        </div>
      )}

      {rolAEliminar && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/45 p-4">
          <section role="alertdialog" aria-modal="true" aria-labelledby="eliminar-rol-titulo" className="w-full max-w-md rounded-xl bg-white p-5 shadow-xl">
            <h2 id="eliminar-rol-titulo" className="text-lg font-semibold text-slate-900">¿Eliminar el rol {rolAEliminar.nombre}?</h2>
            <p className="mt-2 text-sm text-slate-600">No se puede eliminar si hay usuarios que lo tienen asignado.</p>
            {error && <p role="alert" className="mt-3 text-sm text-rose-600">{error}</p>}
            <div className="mt-6 flex justify-end gap-2">
              <button type="button" onClick={() => setRolAEliminar(null)} className="rounded-lg border border-slate-200 px-4 py-2 text-sm text-slate-600">Cancelar</button>
              <button type="button" disabled={guardando} onClick={() => void eliminarRol()} className="rounded-lg bg-rose-600 px-4 py-2 text-sm font-semibold text-white disabled:cursor-wait disabled:opacity-60">{guardando ? 'Eliminando...' : 'Eliminar rol'}</button>
            </div>
          </section>
        </div>
      )}
      {(rolEditado !== null || false) && null}
    </>
  )
}