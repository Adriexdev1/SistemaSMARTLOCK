import { useState, type FormEvent } from "react"
//Importacion de iconos
import { Eye, EyeOff } from "lucide-react"
//Importacion para manejo de rutas
import { useNavigate } from "react-router-dom"

export default function Login() {
  //Constantes para almacenar el valor de la navegacion, el correo, la contraseña
  const navigate = useNavigate()
  const [correo, setCorreo] = useState('')
  const [contrasena, setContrasena] = useState('')
  const [mostrarContrasena, setMostrarContrasena] = useState(false)
  const [error, setError] = useState('')

  //Funcion para inicio de sesion
  function iniciarSesion(evento: FormEvent<HTMLFormElement>){
    evento.preventDefault()

    //Condicional para determinar si el correo y contraseña son validos
    if(correo === 'admin@smartlock.com' && contrasena === '2173754'){
      navigate('/inicio')
      return
    }

    setError('Correo o contraseña incorrectos')
  }

  return (
  <>
     {/*Textos del formulario*/}
      {/* El título y el espacio del formulario crecen ligeramente desde sm. */}
      <h2 className="text-2xl font-bold text-slate-900 sm:text-[28px]">Iniciar sesión</h2>
      <p className="mt-2 text-slate-500">
        Ingresa tus credenciales para acceder
      </p>

      {/*Formulario*/}
      <form onSubmit={iniciarSesion} className="mt-7 space-y-5 sm:mt-8">
        
        {/*Entrada de correo electronico*/}
        <label className="block text-sm font-medium text-slate-700">
          Correo electrónico
          <input
            required
            type="email"
            autoComplete="email"
            value={correo}
            onChange={(evento) => setCorreo(evento.target.value)}
            className="mt-2 h-12 w-full rounded-lg border border-slate-300 bg-white px-3"
          />
        </label>
        
        {/*Entrada de contraseña*/}
        <label className="block text-sm font-medium text-slate-700">
          Contraseña
          <span className="mt-2 flex h-12 items-center rounded-lg border border-slate-300 bg-white pr-3">
            <input
              required
              type={mostrarContrasena ? 'text' : 'password'}
              autoComplete="current-password"
              value={contrasena}
              onChange={(evento) => setContrasena(evento.target.value)}
              className="h-full min-w-0 flex-1 rounded-lg bg-transparent px-3 outline-none"
            />

            {/*Boton para mostrar u ocultar contraseña*/}
            <button
              type="button"
              onClick={() => setMostrarContrasena((actual) => !actual)}
              aria-label={mostrarContrasena ? 'Ocultar contraseña' : 'Mostrar contraseña'}
              className="flex h-10 w-10 shrink-0 items-center justify-center rounded-md text-slate-500 hover:bg-slate-100"
            >
              {mostrarContrasena ? <EyeOff size={18} /> : <Eye size={18} />}
            </button>
          </span>
        </label>

        {/*Seccion para opciones de recordar contraseña*/}
        <div className="flex flex-wrap items-center justify-between gap-3 text-sm">
          <label className="flex items-center gap-2 text-slate-600">
            <input type="checkbox" />
            Recordarme
          </label>
          <button type="button" className="font-medium text-orange-600">
            ¿Olvidaste tu contraseña?
          </button>
        </div>

        {error && <p role="alert" className="text-sm text-rose-600">{error}</p>}

        {/*Boton para iniciar sesion*/}
        <button
          type="submit"
          className="h-12 w-full rounded-lg bg-amber-500 font-semibold text-slate-950 hover:bg-amber-400"
        >
          Iniciar sesión
        </button>
      </form>
    </>
  )
} 