//Libreria para realizar cambios en la informacion mostrada al realizar busquedas
import { useState } from "react"
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
    rol: string
    estado: 'Activo' | 'Inactivo'| 'Suspendido'
}

//Constante la cual almacena a los usuarios
const usuarios: Usuario[] = [
  {
    id: 1,
    iniciales: 'CM',
    nombre: 'Carlos Mendoza Ruiz',
    correo: 'c.mendoza@planta.com',
    rol: 'Operario',
    estado: 'Activo',
  },
  {
    id: 2,
    iniciales: 'ML',
    nombre: 'María López Serna',
    correo: 'm.lopez@planta.com',
    rol: 'Supervisora',
    estado: 'Suspendido',
  },
  {
    id:3, 
    iniciales: 'IR',
    nombre: 'Ing. Ramírez Torres',
    correo: 'ramirez@imss.gob.mx',
    rol: 'Proveedor',
    estado: 'Activo',
  },
    {
        id:4,
    iniciales: 'Ig',
    nombre: 'Ing',
    correo: 'ramirez@imss.gob.mx',
    rol: 'Proveedor',
    estado: 'Activo',
  },
    {
        id:5,
    iniciales: 'RE',
    nombre: 'REX',
    correo: 'ramirez@imss.gob.mx',
    rol: 'Proveedor',
    estado: 'Activo',
  },
    {
        id:6,
    iniciales: 'BEN',
    nombre: 'BEN 10',
    correo: 'ramirez@imss.gob.mx',
    rol: 'Proveedor',
    estado: 'Inactivo',
  },
      {
        id:7,
    iniciales: 'GWEN',
    nombre: 'GWEN',
    correo: 'ramirez@imss.gob.mx',
    rol: 'Proveedor',
    estado: 'Activo',
  },
]

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


//Variable para realizar el armado de la tabla
const columnasUsuarios = [
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
        <button type="button" aria-label={`Ver ${usuario.nombre}`} className="flex h-9 w-9 items-center justify-center rounded-md text-slate-500 transition-colors hover:bg-slate-100 hover:text-slate-900 focus-visible:outline-2 focus-visible:outline-slate-400">
          <Eye size={17} strokeWidth={3.5} />
        </button>
        <button type="button" aria-label={`Editar ${usuario.nombre}`} className="flex h-9 w-9 items-center justify-center rounded-md text-blue-600 transition-colors hover:bg-blue-100 hover:text-blue-800 focus-visible:outline-2 focus-visible:outline-blue-400">
          <Pencil size={16} strokeWidth={3.5}/>
        </button>
        <button type="button" aria-label={`Eliminar ${usuario.nombre}`} className="flex h-9 w-9 items-center justify-center rounded-md text-rose-600 transition-colors hover:bg-rose-100 hover:text-rose-800 focus-visible:outline-2 focus-visible:outline-rose-400">
          <Trash2 size={16} strokeWidth={3.5}/>
        </button>
      </div>
    ),
  },
]

//Funcion para permitir busqueda con errores
function normalizar(texto: string){
    return texto
    .normalize('NFD')
    .replace(/[\u0300-\u036f]/g, '')
    .toLowerCase()
    .trim()
}

export default function Usuarios(){
    //Constante para realizar busquedas en la interfaz
    const [busqueda, setBusqueda] = useState('')
    const [estado, setEstado] = useState('Todos') 

    //Constantes para definir los usuarios por pagina 
    const [paginaActual, setPaginaActual] = useState(1)
    const tamanoPagina = 5
    
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
    //Numero total de paginas
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
        className="inline-flex items-center gap-2 rounded-md bg-amber-500 px-4 py-2 font-medium text-slate-950 hover:bg-amber-400"
      >
        <Plus size={18} aria-hidden="true" />
        Nuevo usuario
      </button>
    </section>

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
        <div className="mt-4">
            <DataTable columns={columnasUsuarios}
            rows={usuariosPagina}
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
    </>
    )
}