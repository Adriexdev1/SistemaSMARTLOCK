//Libreria para realizar cambios en la informacion mostrada al realizar busquedas
import { useEffect, useState, type FormEvent } from "react"
//Importacion de la barra de busqueda
import BarraBusqueda from "../components/componentesPaginas/BarraBusqueda"
//Importacion de tabla base
import DataTable  from "../components/TablaBase"
import { Eye, Pencil, Plus, Trash2 } from 'lucide-react'
//Importacion de pie de tabla
import Paginacion from "../components/PieTabla"

//Variable con los campos de datos definidos para usuarios
type Usuario = {
    id: number
    iniciales: string
    nombre: string
    correo: string
    idRol: number | null
    rol: string
    estado: 'Activo' | 'Inactivo'| 'Suspendido'
}

type Rol = { id: number; nombre: string }
type UsuarioApi = {
  idUsuario: number
  nombreUsuario: string
  correoUsuario: string
  idRol: number | null
  rol: string | null
  estadoUsuario: number
}

const apiUrl = import.meta.env.VITE_API_URL || 'http://localhost:5000'

function adaptarUsuario(usuario: UsuarioApi): Usuario {
  return {
    id: usuario.idUsuario,
    iniciales: obtenerIniciales(usuario.nombreUsuario),
    nombre: usuario.nombreUsuario,
    correo: usuario.correoUsuario,
    idRol: usuario.idRol,
    rol: usuario.rol ?? 'Sin rol asignado',
    estado: usuario.estadoUsuario === 1
      ? 'Activo'
      : usuario.estadoUsuario === 2 ? 'Suspendido' : 'Inactivo',
  }
}

function estadoAApi(estado: Usuario['estado']) {
  return estado === 'Activo' ? 1 : estado === 'Suspendido' ? 2 : 0
}

async function errorDeRespuesta(respuesta: Response, mensajePredeterminado: string) {
  try {
    const cuerpo = await respuesta.json() as { mensaje?: string; title?: string }
    return cuerpo.mensaje ?? cuerpo.title ?? mensajePredeterminado
  } catch {
    return mensajePredeterminado
  }
}

//Funcion para definir colores en los roles
function clasesRol(rol: string){
    const colores: Record<string, string> ={
        Operario: 'bg-blue-100 text-blue-700',
        Supervisora: 'bg-purple-100 text-purple-700',
        Proveedor: 'bg-amber-100 text-amber-700',
        Seguridad: 'bg-orange-100 text-orange-700',
        Administrador: 'bg-emerald-100 text-emerald-700',
    }

    return colores[rol] ?? 'bg-slate-100 text-slate-700'
}

//Funcion para definir colores en los estados
function clasesEstado(estado: Usuario['estado']) {
  const colores: Record<Usuario['estado'], { etiqueta: string; punto: string }> = {
    Activo: {
      etiqueta: 'bg-emerald-100 text-emerald-700',
      punto: 'bg-emerald-600',
    },
    Inactivo: {
      etiqueta: 'bg-amber-100 text-amber-700',
      punto: 'bg-amber-600',
    },
    Suspendido: {
      etiqueta: 'bg-rose-100 text-rose-700',
      punto: 'bg-rose-600',
    },
  }

  return colores[estado]
}


//Funcion para armar la tabla y conectar las acciones de cada fila
function crearColumnasUsuarios(
  verUsuario: (usuario: Usuario) => void,
  editarUsuario: (usuario: Usuario) => void,
  confirmarEliminacion: (usuario: Usuario) => void,
) {
return [
  {
    header: 'Nombre', width: '30%',
    render: (usuario: Usuario) => (
      <div className="flex items-center gap-3">
        <span className="flex h-9 w-9 shrink-0 items-center justify-center rounded-full bg-amber-500 text-xs font-bold text-slate-950">
          {usuario.iniciales}
        </span>
        <span className="font-medium text-slate-800">{usuario.nombre}</span>
      </div>
    ),
  },
  {
    header: 'Correo', width: '26%',
    render: (usuario: Usuario) => (
      <span className="font-mono text-slate-500">{usuario.correo}</span>
    ),
  },
  {
    header: 'Rol', width: '16%',
    render: (usuario: Usuario) => (
        <span className={`rounded-full px-2.5 py-1 text-xs font-semibold ${clasesRol(usuario.rol)}`}>
            {usuario.rol}
        </span>
    ),
  },
  {
    header: 'Estado', width: '14%',
    render: (usuario: Usuario) => {
        const colores = clasesEstado(usuario.estado)

        return (
        <span className={`inline-flex items-center gap-1.5 rounded-full px-2.5 py-1 text-xs font-semibold ${colores.etiqueta}`}>
            <span className={`h-1.5 w-1.5 rounded-full ${colores.punto}`} />
            {usuario.estado}
            </span>
    )},
  },
  {
    header: 'Acciones', width: '14%',
    render: (usuario: Usuario) => (
      <div className="flex items-center gap-2">
        <button type="button" onClick={() => verUsuario(usuario)} aria-label={`Ver ${usuario.nombre}`} className="flex h-9 w-9 items-center justify-center rounded-md text-slate-500 transition-colors hover:bg-slate-100 hover:text-slate-900 focus-visible:outline-2 focus-visible:outline-slate-400">
          <Eye size={17} strokeWidth={3.5} />
        </button>
        <button type="button" onClick={() => editarUsuario(usuario)} aria-label={`Editar ${usuario.nombre}`} className="flex h-9 w-9 items-center justify-center rounded-md text-blue-600 transition-colors hover:bg-blue-100 hover:text-blue-800 focus-visible:outline-2 focus-visible:outline-blue-400">
          <Pencil size={16} strokeWidth={3.5}/>
        </button>
        <button type="button" onClick={() => confirmarEliminacion(usuario)} aria-label={`Eliminar ${usuario.nombre}`} className="flex h-9 w-9 items-center justify-center rounded-md text-rose-600 transition-colors hover:bg-rose-100 hover:text-rose-800 focus-visible:outline-2 focus-visible:outline-rose-400">
          <Trash2 size={16} strokeWidth={3.5}/>
        </button>
      </div>
    ),
  },
]
}

//Funcion para permitir busqueda con errores
function normalizar(texto: string){
    return texto
    .normalize('NFD')
    .replace(/[\u0300-\u036f]/g, '')
    .toLowerCase()
    .trim()
}

function obtenerIniciales(nombre: string) {
  return nombre
    .trim()
    .split(/\s+/)
    .slice(0, 2)
    .map((parte) => parte[0]?.toUpperCase() ?? '')
    .join('')
}


export default function Usuarios(){
    const[usuarios, setUsuarios] = useState<Usuario[]>([])
    const[roles, setRoles] = useState<Rol[]>([])
    const[cargando, setCargando] = useState(true)
    const[guardando, setGuardando] = useState(false)
    const[errorApi, setErrorApi] = useState('')
    const[modalAbierto, setModalAbierto] = useState(false)
    const[nuevoUsuario, setNuevoUsuario] = useState({
      nombre: '',
      correo: '',
      idRol: '',
      contrasena: '',
    })
    const [usuarioSeleccionado, setUsuarioSeleccionado] = useState<Usuario | null>(null)
    const [modoUsuario, setModoUsuario] = useState<'ver' | 'editar' | null>(null)
    const [usuarioAEliminar, setUsuarioAEliminar] = useState<Usuario | null>(null)
    const [borradorUsuario, setBorradorUsuario] = useState<Usuario | null>(null)

    //Constante para realizar busquedas en la interfaz
    const [busqueda, setBusqueda] = useState('')
    const [estado, setEstado] = useState('Todos') 

    //Constantes para definir los usuarios por pagina 
    const [paginaActual, setPaginaActual] = useState(1)
    const tamanoPagina = 5

    useEffect(() => {
      async function cargarDatos() {
        setCargando(true)
        setErrorApi('')

        try {
          const [respuestaUsuarios, respuestaRoles] = await Promise.all([
            fetch(`${apiUrl}/api/usuarios`),
            fetch(`${apiUrl}/api/usuarios/roles`),
          ])

          if (!respuestaUsuarios.ok || !respuestaRoles.ok) {
            throw new Error('No se pudieron cargar usuarios y roles desde la API.')
          }

          const [usuariosApi, rolesApi] = await Promise.all([
            respuestaUsuarios.json() as Promise<UsuarioApi[]>,
            respuestaRoles.json() as Promise<{ idRol: number; nombreRol: string }[]>,
          ])

          setUsuarios(usuariosApi.map(adaptarUsuario))
          const rolesDisponibles = rolesApi.map((rol) => ({ id: rol.idRol, nombre: rol.nombreRol }))
          setRoles(rolesDisponibles)
          setNuevoUsuario((actual) => ({
            ...actual,
            idRol: actual.idRol || String(rolesDisponibles[0]?.id ?? ''),
          }))
        } catch (error) {
          setErrorApi(error instanceof Error ? error.message : 'Error al conectar con la API.')
        } finally {
          setCargando(false)
        }
      }

      void cargarDatos()
    }, [])
    
    const usuariosFiltrados = usuarios.filter((usuario) => {
        const campos = normalizar(
            `${usuario.nombre} ${usuario.correo} ${usuario.rol} ${usuario.iniciales}`
        )
    
        const terminos = normalizar(busqueda).split(/\s+/).filter(Boolean)
        const coincideTexto = terminos.every((termino) => campos.includes(termino))
        const coincideEstado =
        estado === 'Todos' || usuario.estado === estado
  return coincideTexto && coincideEstado
})
    const totalPaginas = Math.max(
        1,
        Math.ceil(usuariosFiltrados.length / tamanoPagina)
    )

    //Constantes para determinar la Pagina
    const pagina = Math.min(paginaActual, totalPaginas)
    const indiceInicial = (pagina - 1) * tamanoPagina
    
    //Constante que determina los usuarios en la pagina
    const usuariosPagina = usuariosFiltrados.slice(
        indiceInicial,
        indiceInicial + tamanoPagina
    )

    //Funcion para manejar el registro de usuarios nuevos
    async function registrarUsuario(evento: FormEvent<HTMLFormElement>) {
      evento.preventDefault()
      setGuardando(true)
      setErrorApi('')

      try {
        const respuesta = await fetch(`${apiUrl}/api/usuarios`, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({
            nombreUsuario: nuevoUsuario.nombre.trim(),
            correoUsuario: nuevoUsuario.correo.trim(),
            contrasena: nuevoUsuario.contrasena,
            idRol: nuevoUsuario.idRol ? Number(nuevoUsuario.idRol) : null,
          }),
        })

        if (!respuesta.ok) {
          setErrorApi(await errorDeRespuesta(respuesta, 'No se pudo crear el usuario.'))
          return
        }

        const usuarioCreado = adaptarUsuario(await respuesta.json() as UsuarioApi)
        setUsuarios((actuales) => [...actuales, usuarioCreado])
        setBusqueda('')
        setEstado('Todos')
        setPaginaActual(1)
        setModalAbierto(false)
        setNuevoUsuario({ nombre: '', correo: '', idRol: String(roles[0]?.id ?? ''), contrasena: '' })
      } catch {
        setErrorApi('No se pudo conectar con la API.')
      } finally {
        setGuardando(false)
      }
    }

    //Funcion para abrir la informacion del usuario seleccionado
    function abrirUsuario(usuario: Usuario) {
      setUsuarioSeleccionado(usuario)
      setModoUsuario('ver')
    }

    //Funcion para cargar los datos actuales en el formulario de edicion
    function iniciarEdicionUsuario(usuario: Usuario) {
      setUsuarioSeleccionado(usuario)
      setBorradorUsuario({ ...usuario })
      setModoUsuario('editar')
    }

    //Funcion para guardar cambios en la lista de usuarios
    async function guardarEdicionUsuario(evento: FormEvent<HTMLFormElement>) {
      evento.preventDefault()
      if (!usuarioSeleccionado || !borradorUsuario) return

      setGuardando(true)
      setErrorApi('')

      try {
        const respuesta = await fetch(`${apiUrl}/api/usuarios/${usuarioSeleccionado.id}`, {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({
            nombreUsuario: borradorUsuario.nombre.trim(),
            correoUsuario: borradorUsuario.correo.trim(),
            idRol: borradorUsuario.idRol,
            estadoUsuario: estadoAApi(borradorUsuario.estado),
          }),
        })

        if (!respuesta.ok) {
          setErrorApi(await errorDeRespuesta(respuesta, 'No se pudieron guardar los cambios.'))
          return
        }

        const usuarioActualizado = {
          ...borradorUsuario,
          nombre: borradorUsuario.nombre.trim(),
          correo: borradorUsuario.correo.trim(),
          iniciales: obtenerIniciales(borradorUsuario.nombre),
          rol: roles.find((rol) => rol.id === borradorUsuario.idRol)?.nombre ?? 'Sin rol asignado',
        }
        setUsuarios((actuales) => actuales.map((usuario) =>
          usuario.id === usuarioSeleccionado.id ? usuarioActualizado : usuario
        ))
        setUsuarioSeleccionado(usuarioActualizado)
        setBorradorUsuario(null)
        setModoUsuario('ver')
      } catch {
        setErrorApi('No se pudo conectar con la API.')
      } finally {
        setGuardando(false)
      }
    }

    //Funcion para eliminar el usuario despues de confirmar
    async function eliminarUsuario() {
      if (!usuarioAEliminar) return
      setGuardando(true)
      setErrorApi('')

      try {
        const respuesta = await fetch(`${apiUrl}/api/usuarios/${usuarioAEliminar.id}`, { method: 'DELETE' })
        if (!respuesta.ok) {
          setErrorApi(await errorDeRespuesta(respuesta, 'No se pudo eliminar el usuario.'))
          return
        }

        setUsuarios((actuales) => actuales.filter((usuario) => usuario.id !== usuarioAEliminar.id))
        setUsuarioAEliminar(null)
        setPaginaActual(1)
      } catch {
        setErrorApi('No se pudo conectar con la API.')
      } finally {
        setGuardando(false)
      }
    }

    //Las columnas reciben estas funciones para actuar sobre la fila seleccionada
    const columnasUsuarios = crearColumnasUsuarios(
      abrirUsuario,
      iniciarEdicionUsuario,
      setUsuarioAEliminar,
    )

    return (
    <>
    {/*Las tablas mantienen columnas legibles y se desplazan horizontalmente en móvil*/}
    <section className="mb-5 flex flex-wrap items-center justify-between gap-4">
        <div>
        <h2 className="text-xl font-semibold text-slate-900">
          Usuarios del sistema
        </h2>
        <p className="mt-1 text-sm text-slate-500">
          Administra a tus usuarios
        </p>
      </div>

      <button
        type="button"
        onClick={() => {
          setErrorApi('')
          setModalAbierto(true)
        }}
        disabled={cargando}
        className="inline-flex items-center gap-2 rounded-md bg-amber-500 px-4 py-2 font-medium text-slate-950 hover:bg-amber-400 disabled:cursor-not-allowed disabled:opacity-50"
      >
        <Plus size={18} aria-hidden="true" />
        Nuevo usuario
      </button>
    </section>

        {errorApi && <p role="alert" className="mb-4 rounded-md border border-rose-200 bg-rose-50 px-4 py-3 text-sm text-rose-700">{errorApi}</p>}
        {cargando && <p role="status" className="py-8 text-center text-sm text-slate-500">Cargando usuarios...</p>}
        {!cargando && !errorApi && roles.length === 0 && <p role="status" className="mb-4 rounded-md border border-amber-200 bg-amber-50 px-4 py-3 text-sm text-amber-800">No hay roles registrados; los nuevos usuarios se crearán sin rol asignado.</p>}

        {/*Barra de busqueda*/}
        <BarraBusqueda valor={busqueda}
        alCambiar={(valor) => {
            setBusqueda(valor)
            setPaginaActual(1)
        }}
        placeholder="Buscar usuario...">
            
            {/*Filtros a aplicar en busquedas*/}
            <select value={estado}
            onChange={(evento) => {setEstado(evento.target.value) 
                setPaginaActual(1)}}
            aria-label="Filtrar por estado"
            className="h-11 rounded-lg border border-slate-200 bg-white px-3 text-sm text-slate-700">
                <option>Todos</option>
                <option>Activo</option>
                <option>Inactivo</option>
                <option>Suspendido</option>
            </select>
        </BarraBusqueda>

        {/*Funcionalidad de modal para registro de usuarios*/}
        {modalAbierto && (
  <div className="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/45 p-4">
    <section
      role="dialog"
      aria-modal="true"
      aria-labelledby="nuevo-usuario-titulo"
      className="max-h-[calc(100dvh-2rem)] w-full max-w-lg overflow-y-auto rounded-xl bg-white p-5 shadow-xl"
    >
      <h2 id="nuevo-usuario-titulo" className="text-lg font-semibold text-slate-900">
        Nuevo usuario
      </h2>

      <form onSubmit={registrarUsuario} className="mt-5 space-y-4">
        <label className="block text-sm text-slate-600">
          Nombre completo
          <input
            required
            value={nuevoUsuario.nombre}
            onChange={(evento) =>
              setNuevoUsuario({ ...nuevoUsuario, nombre: evento.target.value })
            }
            className="mt-1 h-10 w-full rounded-lg border border-slate-200 px-3"
          />
        </label>

        <label className="block text-sm text-slate-600">
          Correo electrónico
          <input
            required
            type="email"
            value={nuevoUsuario.correo}
            onChange={(evento) =>
              setNuevoUsuario({ ...nuevoUsuario, correo: evento.target.value })
            }
            className="mt-1 h-10 w-full rounded-lg border border-slate-200 px-3"
          />
        </label>

        <label className="block text-sm text-slate-600">
          Contraseña inicial
          <input
            required
            type="password"
            minLength={8}
            maxLength={100}
            autoComplete="new-password"
            value={nuevoUsuario.contrasena}
            onChange={(evento) => setNuevoUsuario({ ...nuevoUsuario, contrasena: evento.target.value })}
            className="mt-1 h-10 w-full rounded-lg border border-slate-200 px-3"
          />
        </label>

        <label className="block text-sm text-slate-600">
          Rol
          <select
            value={nuevoUsuario.idRol}
            onChange={(evento) => setNuevoUsuario({ ...nuevoUsuario, idRol: evento.target.value })}
            className="mt-1 h-10 w-full rounded-lg border border-slate-200 bg-white px-3"
          >
            <option value="">Sin rol asignado</option>
            {roles.map((rol) => <option key={rol.id} value={rol.id}>{rol.nombre}</option>)}
          </select>
        </label>

        {errorApi && <p role="alert" className="text-sm text-rose-600">{errorApi}</p>}

        <div className="flex justify-end gap-2 pt-2">
          <button
            type="button"
            onClick={() => setModalAbierto(false)}
            className="rounded-lg border border-slate-200 px-4 py-2 text-sm text-slate-600"
          >
            Cancelar
          </button>
          <button
            type="submit"
            disabled={guardando}
            className="rounded-lg bg-amber-500 px-4 py-2 text-sm font-semibold text-slate-950 disabled:cursor-wait disabled:opacity-60"
          >
            {guardando ? 'Creando...' : 'Crear usuario'}
          </button>
        </div>
      </form>
    </section>
  </div>
)}

        <div className="mt-4">
            <DataTable columns={columnasUsuarios}
          rows={cargando ? [] : usuariosPagina}
            getRowKey={(usuario) => String(usuario.id)} 
            footer={
                <Paginacion
                paginaActual={paginaActual}
                tamanoPagina={tamanoPagina}
                totalElementos={usuariosFiltrados.length}
                etiqueta="usuarios"
                alCambiarPagina={setPaginaActual}
                />
            }
            />
        </div>

        {/*Modal para ver o editar los datos del usuario seleccionado*/}
        {usuarioSeleccionado && modoUsuario && (
          <div
            className="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/45 p-4"
            onMouseDown={(evento) => {
              if (evento.target === evento.currentTarget) {
                setUsuarioSeleccionado(null)
                setModoUsuario(null)
                setBorradorUsuario(null)
              }
            }}
          >
            <section
              role="dialog"
              aria-modal="true"
              aria-labelledby="usuario-dialogo-titulo"
              className="max-h-[calc(100dvh-2rem)] w-full max-w-lg overflow-y-auto rounded-xl bg-white p-5 shadow-xl"
            >
              <div className="mb-5 flex items-center justify-between gap-3">
                <h2 id="usuario-dialogo-titulo" className="text-lg font-semibold text-slate-900">
                  {modoUsuario === 'ver' ? 'Información del usuario' : 'Editar usuario'}
                </h2>
                <button
                  type="button"
                  onClick={() => {
                    setUsuarioSeleccionado(null)
                    setModoUsuario(null)
                    setBorradorUsuario(null)
                  }}
                  className="rounded-md px-3 py-2 text-sm text-slate-500 hover:bg-slate-100"
                >
                  Cerrar
                </button>
              </div>

              {modoUsuario === 'ver' ? (
                <dl className="space-y-4 text-sm">
                  <div className="flex items-center gap-3">
                    <span className="flex h-11 w-11 items-center justify-center rounded-full bg-amber-500 font-bold text-slate-950">
                      {usuarioSeleccionado.iniciales}
                    </span>
                    <div>
                      <dt className="text-slate-500">Nombre completo</dt>
                      <dd className="font-medium text-slate-800">{usuarioSeleccionado.nombre}</dd>
                    </div>
                  </div>
                  <div>
                    <dt className="text-slate-500">Correo electrónico</dt>
                    <dd className="mt-1 break-all font-medium text-slate-800">{usuarioSeleccionado.correo}</dd>
                  </div>
                  <div>
                    <dt className="text-slate-500">Rol</dt>
                    <dd className="mt-1 font-medium text-slate-800">{usuarioSeleccionado.rol}</dd>
                  </div>
                  <div>
                    <dt className="text-slate-500">Estado</dt>
                    <dd className="mt-1 font-medium text-slate-800">{usuarioSeleccionado.estado}</dd>
                  </div>
                </dl>
              ) : borradorUsuario && (
                <form onSubmit={guardarEdicionUsuario} className="space-y-4">
                  <label className="block text-sm text-slate-600">
                    Nombre completo
                    <input
                      required
                      value={borradorUsuario.nombre}
                      onChange={(evento) => setBorradorUsuario({ ...borradorUsuario, nombre: evento.target.value })}
                      className="mt-1 h-10 w-full rounded-lg border border-slate-200 px-3"
                    />
                  </label>
                  <label className="block text-sm text-slate-600">
                    Correo electrónico
                    <input
                      required
                      type="email"
                      value={borradorUsuario.correo}
                      onChange={(evento) => setBorradorUsuario({ ...borradorUsuario, correo: evento.target.value })}
                      className="mt-1 h-10 w-full rounded-lg border border-slate-200 px-3"
                    />
                  </label>
                  <label className="block text-sm text-slate-600">
                    Rol
                    <select
                      required
                      value={borradorUsuario.idRol ?? ''}
                      onChange={(evento) => setBorradorUsuario({
                        ...borradorUsuario,
                        idRol: evento.target.value ? Number(evento.target.value) : null,
                        rol: roles.find((rol) => String(rol.id) === evento.target.value)?.nombre ?? 'Sin rol asignado',
                      })}
                      className="mt-1 h-10 w-full rounded-lg border border-slate-200 bg-white px-3"
                    >
                      <option value="">Selecciona un rol</option>
                      {roles.map((rol) => <option key={rol.id} value={rol.id}>{rol.nombre}</option>)}
                    </select>
                  </label>
                  <label className="block text-sm text-slate-600">
                    Estado
                    <select
                      value={borradorUsuario.estado}
                      onChange={(evento) => setBorradorUsuario({ ...borradorUsuario, estado: evento.target.value as Usuario['estado'] })}
                      className="mt-1 h-10 w-full rounded-lg border border-slate-200 bg-white px-3"
                    >
                      <option>Activo</option>
                      <option>Inactivo</option>
                      <option>Suspendido</option>
                    </select>
                  </label>
                  <div className="flex flex-wrap justify-end gap-2 pt-2">
                    <button
                      type="button"
                      onClick={() => {
                        setModoUsuario('ver')
                        setBorradorUsuario(null)
                      }}
                      className="rounded-lg border border-slate-200 px-4 py-2 text-sm text-slate-600 hover:bg-slate-50"
                    >
                      Cancelar
                    </button>
                    <button type="submit" disabled={guardando} className="rounded-lg bg-amber-500 px-4 py-2 text-sm font-semibold text-slate-950 hover:bg-amber-400 disabled:cursor-wait disabled:opacity-60">
                      {guardando ? 'Guardando...' : 'Guardar cambios'}
                    </button>
                  </div>
                </form>
              )}
            </section>
          </div>
        )}

        {/*Confirmacion antes de eliminar definitivamente al usuario*/}
        {usuarioAEliminar && (
          <div
            className="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/45 p-4"
            onMouseDown={(evento) => {
              if (evento.target === evento.currentTarget) setUsuarioAEliminar(null)
            }}
          >
            <section
              role="alertdialog"
              aria-modal="true"
              aria-labelledby="eliminar-usuario-titulo"
              className="w-full max-w-md rounded-xl bg-white p-5 shadow-xl"
            >
              <h2 id="eliminar-usuario-titulo" className="text-lg font-semibold text-slate-900">
                ¿Eliminar este usuario?
              </h2>
              <p className="mt-2 text-sm text-slate-600">
                Se eliminará a {usuarioAEliminar.nombre}. Esta acción no se puede deshacer.
              </p>
              <div className="mt-6 flex justify-end gap-2">
                <button type="button" onClick={() => setUsuarioAEliminar(null)} className="rounded-lg border border-slate-200 px-4 py-2 text-sm text-slate-600 hover:bg-slate-50">
                  Cancelar
                </button>
                <button type="button" onClick={eliminarUsuario} className="rounded-lg bg-rose-600 px-4 py-2 text-sm font-semibold text-white hover:bg-rose-700">
                  Eliminar usuario
                </button>
              </div>
            </section>
          </div>
        )}
    </>
    )
}