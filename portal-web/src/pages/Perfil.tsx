import { useState, type FormEvent } from 'react'
import { Check, KeyRound, LockKeyhole, Pencil, ShieldCheck, X } from 'lucide-react'
import { actualizarEstadoPinSesion, actualizarLongitudContrasenaSesion, guardarUsuarioSesion, obtenerUsuarioSesion } from '../utils/usuarioSesion'

type DatosPerfil = {
    nombre: string
    correo: string
    telefono: string
    rol: string
}

type PanelSeguridad = 'contrasena' | 'pin' | 'eliminar-pin' | null

function inicialesDe(nombre: string) {
    return nombre
        .trim()
        .split(/\s+/)
        .slice(0, 2)
        .map((parte) => parte[0]?.toUpperCase() ?? '')
        .join('')
}

export default function Perfil() {
    const [perfil, setPerfil] = useState<DatosPerfil>(() => {
        const usuario = obtenerUsuarioSesion()
        return {
            nombre: usuario?.nombreUsuario ?? 'Usuario',
            correo: usuario?.correoUsuario ?? '',
            telefono: usuario?.telefonoUsuario ?? 'No registrado',
            rol: usuario?.rol ?? 'Sin rol asignado',
        }
    })
    const [perfilBorrador, setPerfilBorrador] = useState(perfil)
    const [editandoPerfil, setEditandoPerfil] = useState(false)
    const [contrasenaPerfil, setContrasenaPerfil] = useState('')
    const [guardandoPerfil, setGuardandoPerfil] = useState(false)
    const [errorPerfil, setErrorPerfil] = useState('')
    const [panelSeguridad, setPanelSeguridad] = useState<PanelSeguridad>(null)
    const [mensaje, setMensaje] = useState('')
    const [errorPin, setErrorPin] = useState('')
    const [contrasenaActual, setContrasenaActual] = useState('')
    const [nuevaContrasena, setNuevaContrasena] = useState('')
    const [nuevoPin, setNuevoPin] = useState('')
    const [pinConfigurado, setPinConfigurado] = useState(() => Boolean(obtenerUsuarioSesion()?.tienePinConfigurado))
    const [guardandoPin, setGuardandoPin] = useState(false)
    const [guardandoContrasena, setGuardandoContrasena] = useState(false)
    const [errorContrasena, setErrorContrasena] = useState('')
    const [longitudContrasena, setLongitudContrasena] = useState(() => obtenerUsuarioSesion()?.contrasenaCaracteres ?? 0)

    async function guardarPerfil(evento: FormEvent<HTMLFormElement>) {
        evento.preventDefault()
        setGuardandoPerfil(true)
        setErrorPerfil('')

        try {
            const apiUrl = import.meta.env.VITE_API_URL || 'http://localhost:5000'
            const usuario = obtenerUsuarioSesion()
            if (!usuario) {
                setErrorPerfil('Tu sesión terminó. Inicia sesión nuevamente.')
                return
            }

            const cambios = {
                nombreUsuario: perfilBorrador.nombre.trim(),
                correoUsuario: perfilBorrador.correo.trim(),
                telefonoUsuario: perfilBorrador.telefono.trim() || null,
            }
            const respuesta = await fetch(`${apiUrl}/api/auth/profile`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    idUsuario: usuario.idUsuario,
                    ...cambios,
                    contrasenaActual: contrasenaPerfil,
                }),
            })

            if (!respuesta.ok) {
                setErrorPerfil(respuesta.status === 401
                    ? 'La contraseña actual no es correcta.'
                    : respuesta.status === 409
                        ? 'Ese correo ya está registrado por otro usuario.'
                        : respuesta.status === 404
                            ? 'No se encontró el usuario en la base de datos.'
                            : 'No se pudo guardar la información.')
                return
            }

            setPerfil({ ...perfilBorrador, ...cambios, telefono: cambios.telefonoUsuario ?? '' })
            guardarUsuarioSesion({ ...usuario, ...cambios })
            setContrasenaPerfil('')
            setEditandoPerfil(false)
            setMensaje('La información del perfil se guardó correctamente.')
        } catch {
            setErrorPerfil('No se pudo conectar con la API.')
        } finally {
            setGuardandoPerfil(false)
        }
    }

    function alternarEdicion() {
        setPerfilBorrador(perfil)
        setEditandoPerfil((actual) => !actual)
        setContrasenaPerfil('')
        setErrorPerfil('')
        setMensaje('')
    }

    async function guardarSeguridad(evento: FormEvent<HTMLFormElement>) {
        evento.preventDefault()
        setGuardandoContrasena(true)
        setErrorContrasena('')

        try {
            const apiUrl = import.meta.env.VITE_API_URL || 'http://localhost:5000'
            const usuario = obtenerUsuarioSesion()
            const respuesta = await fetch(`${apiUrl}/api/auth/password`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    idUsuario: usuario?.idUsuario,
                    contrasenaActual,
                    nuevaContrasena,
                }),
            })

            if (!respuesta.ok) {
                setErrorContrasena(respuesta.status === 401
                    ? 'La contraseña actual no es correcta.'
                    : 'No se pudo actualizar la contraseña.')
                return
            }

            const longitudNueva = Array.from(nuevaContrasena).length
            setLongitudContrasena(longitudNueva)
            actualizarLongitudContrasenaSesion(longitudNueva)
            setPanelSeguridad(null)
            setContrasenaActual('')
            setNuevaContrasena('')
            setMensaje('La contraseña se actualizó correctamente.')
        } catch {
            setErrorContrasena('No se pudo conectar con la API.')
        } finally {
            setGuardandoContrasena(false)
        }
    }

    async function guardarPin(evento: FormEvent<HTMLFormElement>) {
        evento.preventDefault()
        setGuardandoPin(true)
        setErrorPin('')

        try {
            const apiUrl = import.meta.env.VITE_API_URL || 'http://localhost:5000'
            const usuario = obtenerUsuarioSesion()
            const respuesta = await fetch(`${apiUrl}/api/auth/pin`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    idUsuario: usuario?.idUsuario,
                    contrasenaActual,
                    pin: nuevoPin,
                }),
            })

            if (!respuesta.ok) {
                setErrorPin(respuesta.status === 401
                    ? 'La contraseña actual no es correcta.'
                    : 'No se pudo guardar el PIN.')
                return
            }

            setPinConfigurado(true)
            actualizarEstadoPinSesion(true)
            setPanelSeguridad(null)
            setContrasenaActual('')
            setNuevoPin('')
            setMensaje('El PIN se guardó de forma segura.')
        } catch {
            setErrorPin('No se pudo conectar con la API.')
        } finally {
            setGuardandoPin(false)
        }
    }

    async function eliminarPin(evento: FormEvent<HTMLFormElement>) {
        evento.preventDefault()
        setGuardandoPin(true)
        setErrorPin('')

        try {
            const apiUrl = import.meta.env.VITE_API_URL || 'http://localhost:5000'
            const usuario = obtenerUsuarioSesion()
            const respuesta = await fetch(`${apiUrl}/api/auth/pin`, {
                method: 'DELETE',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    idUsuario: usuario?.idUsuario,
                    contrasenaActual,
                }),
            })

            if (!respuesta.ok) {
                setErrorPin(respuesta.status === 401
                    ? 'La contraseña actual no es correcta.'
                    : 'No se pudo quitar el PIN.')
                return
            }

            setPinConfigurado(false)
            actualizarEstadoPinSesion(false)
            setPanelSeguridad(null)
            setContrasenaActual('')
            setMensaje('El PIN se quitó. El acceso QR no solicitará PIN.')
        } catch {
            setErrorPin('No se pudo conectar con la API.')
        } finally {
            setGuardandoPin(false)
        }
    }

    const campoClase = 'mt-1 h-10 w-full rounded-lg border border-slate-200 bg-white px-3 text-sm text-slate-800 outline-none focus:border-amber-400 focus:ring-2 focus:ring-amber-100'

    return (
        <div className="mx-auto w-full max-w-[1500px]">
            <section className="mb-5">
                <h2 className="text-xl font-semibold text-slate-900">Mi perfil</h2>
                <p className="mt-1 text-sm text-slate-500">Gestiona tu información personal y seguridad</p>
            </section>

            <article className="overflow-hidden rounded-xl border border-slate-200 bg-white shadow-sm">
                <section className="p-5 sm:p-7">
                    <div className="mb-6 flex flex-wrap items-center justify-between gap-3 border-b border-slate-100 pb-4">
                        <h3 className="text-base font-semibold text-slate-800">Información personal</h3>
                        <button
                            type="button"
                            onClick={alternarEdicion}
                            className="inline-flex items-center gap-2 rounded-md px-2 py-1 text-sm font-medium text-orange-600 transition-colors hover:bg-orange-50"
                        >
                            {editandoPerfil ? <X size={16} /> : <Pencil size={16} />}
                            {editandoPerfil ? 'Cancelar' : 'Editar información'}
                        </button>
                    </div>

                    <div className="grid gap-7 md:grid-cols-[120px_minmax(0,1fr)] md:items-start">
                        <div className="flex justify-start">
                            <div className="flex h-20 w-20 items-center justify-center rounded-full bg-amber-500 text-2xl font-bold text-slate-950">
                                {inicialesDe(perfil.nombre)}
                            </div>
                        </div>

                        {editandoPerfil ? (
                            <form onSubmit={guardarPerfil} className="grid gap-4 sm:grid-cols-2">
                                <label className="text-sm text-slate-500">
                                    Nombre completo
                                    <input required value={perfilBorrador.nombre} onChange={(evento) => setPerfilBorrador({ ...perfilBorrador, nombre: evento.target.value })} className={campoClase} />
                                </label>
                                <label className="text-sm text-slate-500">
                                    Correo electrónico
                                    <input required type="email" value={perfilBorrador.correo} onChange={(evento) => setPerfilBorrador({ ...perfilBorrador, correo: evento.target.value })} className={campoClase} />
                                </label>
                                <label className="text-sm text-slate-500">
                                    Teléfono
                                    <input value={perfilBorrador.telefono} onChange={(evento) => setPerfilBorrador({ ...perfilBorrador, telefono: evento.target.value })} className={campoClase} />
                                </label>
                                <label className="text-sm text-slate-500 sm:col-span-2">
                                    Confirma tu contraseña actual para guardar
                                    <input required type="password" autoComplete="current-password" value={contrasenaPerfil} onChange={(evento) => setContrasenaPerfil(evento.target.value)} className={campoClase} />
                                </label>
                                {errorPerfil && <p role="alert" className="text-sm text-rose-600 sm:col-span-2">{errorPerfil}</p>}
                                <div className="flex items-end justify-end sm:col-span-2">
                                    <button type="submit" disabled={guardandoPerfil} className="inline-flex h-10 items-center gap-2 rounded-lg bg-amber-500 px-4 text-sm font-semibold text-slate-950 hover:bg-amber-400 disabled:cursor-wait disabled:opacity-60">
                                        <Check size={16} /> {guardandoPerfil ? 'Guardando...' : 'Guardar cambios'}
                                    </button>
                                </div>
                            </form>
                        ) : (
                            <dl className="grid gap-x-8 gap-y-5 sm:grid-cols-2">
                                <div>
                                    <dt className="text-sm text-slate-500">Nombre completo</dt>
                                    <dd className="mt-1 font-medium text-slate-800">{perfil.nombre}</dd>
                                </div>
                                <div>
                                    <dt className="text-sm text-slate-500">Correo electrónico</dt>
                                    <dd className="mt-1 break-all font-medium text-slate-800">{perfil.correo}</dd>
                                </div>
                                <div>
                                    <dt className="text-sm text-slate-500">Teléfono</dt>
                                    <dd className="mt-1 font-medium text-slate-800">{perfil.telefono}</dd>
                                </div>
                                <div>
                                    <dt className="text-sm text-slate-500">Rol</dt>
                                    <dd className="mt-1">
                                        <span className="rounded-full bg-emerald-100 px-2.5 py-1 text-xs font-semibold text-emerald-700">{perfil.rol}</span>
                                    </dd>
                                </div>
                            </dl>
                        )}
                    </div>
                </section>

                <section className="border-t border-slate-200 p-5 sm:p-7">
                    <div className="mb-2 border-b border-slate-100 pb-4">
                        <h3 className="text-base font-semibold text-slate-800">Seguridad</h3>
                    </div>

                    {/*Los controles de seguridad pueden bajar a otra línea cuando el teléfono es estrecho*/}
                    <div className="divide-y divide-slate-100">
                        <div className="flex flex-wrap items-center justify-between gap-4 py-5">
                            <div className="flex items-start gap-3">
                                <span className="mt-0.5 flex h-9 w-9 items-center justify-center rounded-lg bg-slate-100 text-slate-600">
                                    <LockKeyhole size={18} />
                                </span>
                                <div>
                                    <p className="text-sm font-medium text-slate-800">Contraseña</p>
                                    <p className="mt-1 text-sm tracking-[0.2em] text-slate-400" aria-label="Contraseña oculta">
                                        {longitudContrasena > 0
                                            ? '•'.repeat(longitudContrasena)
                                            : 'Vuelve a iniciar sesión para cargar su longitud'}
                                    </p>
                                </div>
                            </div>
                            <button
                                type="button"
                                onClick={() => setPanelSeguridad(panelSeguridad === 'contrasena' ? null : 'contrasena')}
                                className="rounded-lg border border-slate-200 px-3 py-2 text-sm text-slate-600 transition-colors hover:bg-slate-50"
                            >
                                Cambiar contraseña
                            </button>
                        </div>

                        <div className="flex flex-wrap items-center justify-between gap-4 py-5">
                            <div className="flex items-start gap-3">
                                <span className="mt-0.5 flex h-9 w-9 items-center justify-center rounded-lg bg-emerald-50 text-emerald-600">
                                    <KeyRound size={18} />
                                </span>
                                <div>
                                    <p className="text-sm font-medium text-slate-800">PIN de acceso</p>
                                    <p className={`mt-1 text-sm font-medium ${pinConfigurado ? 'text-emerald-600' : 'text-slate-500'}`}>
                                        {pinConfigurado ? 'PIN habilitado' : 'Sin PIN configurado'}
                                    </p>
                                </div>
                            </div>
                            <button
                                type="button"
                                onClick={() => {
                                    setPanelSeguridad(panelSeguridad === 'pin' ? null : 'pin')
                                    setErrorPin('')
                                    setMensaje('')
                                }}
                                className="rounded-lg border border-slate-200 px-3 py-2 text-sm text-slate-600 transition-colors hover:bg-slate-50"
                            >
                                {pinConfigurado ? 'Cambiar PIN' : 'Configurar PIN'}
                            </button>
                            {pinConfigurado && (
                                <button
                                    type="button"
                                    onClick={() => {
                                        setPanelSeguridad(panelSeguridad === 'eliminar-pin' ? null : 'eliminar-pin')
                                        setErrorPin('')
                                        setMensaje('')
                                    }}
                                    className="rounded-lg border border-rose-200 px-3 py-2 text-sm text-rose-700 transition-colors hover:bg-rose-50"
                                >
                                    Quitar PIN
                                </button>
                            )}
                        </div>
                    </div>

                    {panelSeguridad && (
                        <form
                            onSubmit={panelSeguridad === 'pin' ? guardarPin : panelSeguridad === 'eliminar-pin' ? eliminarPin : guardarSeguridad}
                            className="mt-2 grid gap-4 rounded-lg bg-slate-50 p-4 sm:grid-cols-2"
                        >
                            {panelSeguridad === 'contrasena' ? (
                                <>
                                    <label className="text-sm text-slate-600">
                                        Contraseña actual
                                        <input required type="password" autoComplete="current-password" value={contrasenaActual} onChange={(evento) => setContrasenaActual(evento.target.value)} className={campoClase} />
                                    </label>
                                    <label className="text-sm text-slate-600">
                                        Nueva contraseña
                                        <input required type="password" minLength={8} maxLength={100} autoComplete="new-password" value={nuevaContrasena} onChange={(evento) => setNuevaContrasena(evento.target.value)} className={campoClase} />
                                    </label>
                                </>
                            ) : panelSeguridad === 'pin' ? (
                                <>
                                    <label className="text-sm text-slate-600 sm:col-span-2">
                                        Contraseña actual
                                        <input required type="password" autoComplete="current-password" value={contrasenaActual} onChange={(evento) => setContrasenaActual(evento.target.value)} className={campoClase} />
                                    </label>
                                    <label className="text-sm text-slate-600 sm:col-span-2">
                                        Nuevo PIN
                                        <input required type="password" inputMode="numeric" pattern="[0-9]{4,8}" minLength={4} maxLength={8} value={nuevoPin} onChange={(evento) => setNuevoPin(evento.target.value)} placeholder="4 a 8 dígitos" className={campoClase} />
                                    </label>
                                </>
                            ) : (
                                <label className="text-sm text-slate-600 sm:col-span-2">
                                    Confirma tu contraseña para quitar el PIN
                                    <input required type="password" autoComplete="current-password" value={contrasenaActual} onChange={(evento) => setContrasenaActual(evento.target.value)} className={campoClase} />
                                </label>
                            )}
                            {panelSeguridad === 'contrasena' && errorContrasena && <p role="alert" className="text-sm text-rose-600 sm:col-span-2">{errorContrasena}</p>}
                            {panelSeguridad !== 'contrasena' && errorPin && <p role="alert" className="text-sm text-rose-600 sm:col-span-2">{errorPin}</p>}
                            <div className="flex justify-end gap-2 sm:col-span-2">
                                <button type="button" onClick={() => {
                                    setPanelSeguridad(null)
                                    setContrasenaActual('')
                                    setNuevaContrasena('')
                                    setErrorContrasena('')
                                    setErrorPin('')
                                }} className="rounded-lg border border-slate-200 px-4 py-2 text-sm text-slate-600 hover:bg-white">Cancelar</button>
                                <button type="submit" disabled={panelSeguridad === 'contrasena' ? guardandoContrasena : guardandoPin} className="rounded-lg bg-amber-500 px-4 py-2 text-sm font-semibold text-slate-950 hover:bg-amber-400 disabled:cursor-wait disabled:opacity-60">
                                    {(panelSeguridad === 'contrasena' ? guardandoContrasena : guardandoPin)
                                        ? 'Guardando...'
                                        : panelSeguridad === 'eliminar-pin'
                                            ? 'Quitar PIN'
                                            : 'Guardar'}
                                </button>
                            </div>
                        </form>
                    )}

                    {mensaje && (
                        <p role="status" className="mt-4 flex items-center gap-2 text-sm text-emerald-700">
                            <ShieldCheck size={16} /> {mensaje}
                        </p>
                    )}
                </section>
            </article>
        </div>
    )
}