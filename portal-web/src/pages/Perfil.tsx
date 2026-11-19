import { useState, type FormEvent } from 'react'
import { Check, CreditCard, LockKeyhole, Pencil, ShieldCheck, X } from 'lucide-react'
import { actualizarEstadoTarjetaRfidSesion, actualizarLongitudContrasenaSesion, guardarUsuarioSesion, obtenerUsuarioSesion, tieneNivelPermiso } from '../utils/usuarioSesion'
import { useAutoRefresh } from '../hooks/useAutoRefresh'

type DatosPerfil = {
    nombre: string
    correo: string
    telefono: string
    rol: string
}

type PanelSeguridad = 'contrasena' | 'tarjeta-rfid' | 'eliminar-tarjeta-rfid' | null
type PerfilApi = {
    nombreUsuario: string
    correoUsuario: string
    telefonoUsuario: string | null
    rol: string | null
    tieneTarjetaRfidAsignada: boolean
}

const apiUrl = import.meta.env.VITE_API_URL || 'http://localhost:5000'

function inicialesDe(nombre: string) {
    return nombre
        .trim()
        .split(/\s+/)
        .slice(0, 2)
        .map((parte) => parte[0]?.toUpperCase() ?? '')
        .join('')
}

export default function Perfil() {
    const esAdminTipo2 = tieneNivelPermiso(obtenerUsuarioSesion(), [2])
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
    const [errorCargaPerfil, setErrorCargaPerfil] = useState('')
    const [panelSeguridad, setPanelSeguridad] = useState<PanelSeguridad>(null)
    const [mensaje, setMensaje] = useState('')
    const [errorTarjeta, setErrorTarjeta] = useState('')
    const [contrasenaActual, setContrasenaActual] = useState('')
    const [nuevaContrasena, setNuevaContrasena] = useState('')
    const [uidTarjeta, setUidTarjeta] = useState('')
    const [tarjetaConfigurada, setTarjetaConfigurada] = useState(() => Boolean(obtenerUsuarioSesion()?.tieneTarjetaRfidAsignada))
    const [guardandoTarjeta, setGuardandoTarjeta] = useState(false)
    const [guardandoContrasena, setGuardandoContrasena] = useState(false)
    const [errorContrasena, setErrorContrasena] = useState('')
    const [longitudContrasena, setLongitudContrasena] = useState(() => obtenerUsuarioSesion()?.contrasenaCaracteres ?? 0)

    useAutoRefresh(async () => {
        const usuario = obtenerUsuarioSesion()
        if (!usuario) {
            setErrorCargaPerfil('Tu sesión terminó. Inicia sesión nuevamente.')
            return
        }

        setErrorCargaPerfil('')
        try {
            const respuesta = await fetch(`${apiUrl}/api/usuarios/${usuario.idUsuario}`)
            if (!respuesta.ok) throw new Error('No se pudo actualizar la información del perfil.')
            const datos = await respuesta.json() as PerfilApi
            const perfilActualizado = {
                nombre: datos.nombreUsuario,
                correo: datos.correoUsuario,
                telefono: datos.telefonoUsuario ?? '',
                rol: datos.rol ?? 'Sin rol asignado',
            }
            setPerfil((actual) =>
                actual.nombre === perfilActualizado.nombre &&
                actual.correo === perfilActualizado.correo &&
                actual.telefono === perfilActualizado.telefono &&
                actual.rol === perfilActualizado.rol
                    ? actual
                    : perfilActualizado)
            setTarjetaConfigurada(datos.tieneTarjetaRfidAsignada)
            if (
                usuario.nombreUsuario !== datos.nombreUsuario ||
                usuario.correoUsuario !== datos.correoUsuario ||
                usuario.telefonoUsuario !== datos.telefonoUsuario ||
                usuario.rol !== datos.rol ||
                usuario.tieneTarjetaRfidAsignada !== datos.tieneTarjetaRfidAsignada
            ) {
                guardarUsuarioSesion({
                    ...usuario,
                    nombreUsuario: datos.nombreUsuario,
                    correoUsuario: datos.correoUsuario,
                    telefonoUsuario: datos.telefonoUsuario,
                    rol: datos.rol,
                    tieneTarjetaRfidAsignada: datos.tieneTarjetaRfidAsignada,
                })
            }
        } catch (error) {
            setErrorCargaPerfil(error instanceof Error ? error.message : 'No se pudo conectar con la API.')
        }
    }, 10_000, !editandoPerfil && panelSeguridad === null)

    async function guardarPerfil(evento: FormEvent<HTMLFormElement>) {
        evento.preventDefault()
        setGuardandoPerfil(true)
        setErrorPerfil('')

        try {
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

    async function guardarTarjetaRfid(evento: FormEvent<HTMLFormElement>) {
        evento.preventDefault()
        if (!tieneNivelPermiso(obtenerUsuarioSesion(), [2])) {
            setErrorTarjeta('Solo un administrador tipo 2 puede configurar tarjetas RFID.')
            return
        }
        setGuardandoTarjeta(true)
        setErrorTarjeta('')

        try {
            const usuario = obtenerUsuarioSesion()
            if (!usuario) {
                setErrorTarjeta('Tu sesión terminó. Inicia sesión nuevamente.')
                return
            }

            const respuesta = await fetch(`${apiUrl}/api/auth/tarjeta-rfid`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    idUsuario: usuario.idUsuario,
                    contrasenaActual,
                    uid: uidTarjeta.trim(),
                }),
            })

            if (!respuesta.ok) {
                setErrorTarjeta(respuesta.status === 401
                    ? 'La contraseña actual no es correcta.'
                    : respuesta.status === 403
                        ? 'Solo un administrador tipo 2 puede configurar tarjetas RFID.'
                    : respuesta.status === 400
                        ? 'El UID debe contener 4, 7 o 10 bytes en hexadecimal.'
                        : respuesta.status === 409
                            ? 'Esa tarjeta ya está vinculada a otro usuario.'
                            : respuesta.status === 503
                                ? 'La API todavía no está configurada para tarjetas RFID.'
                                : 'No se pudo vincular la tarjeta.')
                return
            }

            setTarjetaConfigurada(true)
            actualizarEstadoTarjetaRfidSesion(true)
            setPanelSeguridad(null)
            setContrasenaActual('')
            setUidTarjeta('')
            setMensaje('La tarjeta RFID se vinculó de forma segura.')
        } catch {
            setErrorTarjeta('No se pudo conectar con la API.')
        } finally {
            setGuardandoTarjeta(false)
        }
    }

    async function eliminarTarjetaRfid(evento: FormEvent<HTMLFormElement>) {
        evento.preventDefault()
        if (!tieneNivelPermiso(obtenerUsuarioSesion(), [2])) {
            setErrorTarjeta('Solo un administrador tipo 2 puede configurar tarjetas RFID.')
            return
        }
        setGuardandoTarjeta(true)
        setErrorTarjeta('')

        try {
            const usuario = obtenerUsuarioSesion()
            if (!usuario) {
                setErrorTarjeta('Tu sesión terminó. Inicia sesión nuevamente.')
                return
            }

            const respuesta = await fetch(`${apiUrl}/api/auth/tarjeta-rfid`, {
                method: 'DELETE',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    idUsuario: usuario.idUsuario,
                    contrasenaActual,
                }),
            })

            if (!respuesta.ok) {
                setErrorTarjeta(respuesta.status === 401
                    ? 'La contraseña actual no es correcta.'
                    : respuesta.status === 403
                        ? 'Solo un administrador tipo 2 puede configurar tarjetas RFID.'
                    : 'No se pudo quitar la tarjeta.')
                return
            }

            setTarjetaConfigurada(false)
            actualizarEstadoTarjetaRfidSesion(false)
            setPanelSeguridad(null)
            setContrasenaActual('')
            setMensaje('La tarjeta RFID se desvinculó.')
        } catch {
            setErrorTarjeta('No se pudo conectar con la API.')
        } finally {
            setGuardandoTarjeta(false)
        }
    }

    const campoClase = 'mt-1 h-10 w-full rounded-lg border border-slate-200 bg-white px-3 text-sm text-slate-800 outline-none focus:border-amber-400 focus:ring-2 focus:ring-amber-100'

    return (
        <div className="mx-auto w-full max-w-[1500px]">
            <section className="mb-5">
                <h2 className="text-xl font-semibold text-slate-900">Mi perfil</h2>
                <p className="mt-1 text-sm text-slate-500">Gestiona tu información personal y seguridad</p>
            </section>
            {errorCargaPerfil && <p role="alert" className="mb-4 rounded-md border border-rose-200 bg-rose-50 px-4 py-3 text-sm text-rose-700">{errorCargaPerfil}</p>}

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

                        {esAdminTipo2 && <div className="flex flex-wrap items-center justify-between gap-4 py-5">
                            <div className="flex items-start gap-3">
                                <span className="mt-0.5 flex h-9 w-9 items-center justify-center rounded-lg bg-emerald-50 text-emerald-600">
                                    <CreditCard size={18} />
                                </span>
                                <div>
                                    <p className="text-sm font-medium text-slate-800">Tarjeta RFID</p>
                                    <p className={`mt-1 text-sm font-medium ${tarjetaConfigurada ? 'text-emerald-600' : 'text-slate-500'}`}>
                                        {tarjetaConfigurada ? 'Tarjeta vinculada' : 'Sin tarjeta vinculada'}
                                    </p>
                                </div>
                            </div>
                            <button
                                type="button"
                                onClick={() => {
                                    setPanelSeguridad(panelSeguridad === 'tarjeta-rfid' ? null : 'tarjeta-rfid')
                                    setErrorTarjeta('')
                                    setMensaje('')
                                }}
                                className="rounded-lg border border-slate-200 px-3 py-2 text-sm text-slate-600 transition-colors hover:bg-slate-50"
                            >
                                {tarjetaConfigurada ? 'Cambiar tarjeta' : 'Vincular tarjeta'}
                            </button>
                            {tarjetaConfigurada && (
                                <button
                                    type="button"
                                    onClick={() => {
                                        setPanelSeguridad(panelSeguridad === 'eliminar-tarjeta-rfid' ? null : 'eliminar-tarjeta-rfid')
                                        setErrorTarjeta('')
                                        setMensaje('')
                                    }}
                                    className="rounded-lg border border-rose-200 px-3 py-2 text-sm text-rose-700 transition-colors hover:bg-rose-50"
                                >
                                    Desvincular tarjeta
                                </button>
                            )}
                        </div>}
                    </div>

                    {panelSeguridad && (panelSeguridad === 'contrasena' || esAdminTipo2) && (
                        <form
                            onSubmit={panelSeguridad === 'tarjeta-rfid'
                                ? guardarTarjetaRfid
                                : panelSeguridad === 'eliminar-tarjeta-rfid'
                                    ? eliminarTarjetaRfid
                                    : guardarSeguridad}
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
                            ) : panelSeguridad === 'tarjeta-rfid' ? (
                                <>
                                    <label className="text-sm text-slate-600 sm:col-span-2">
                                        Contraseña actual
                                        <input required type="password" autoComplete="current-password" value={contrasenaActual} onChange={(evento) => setContrasenaActual(evento.target.value)} className={campoClase} />
                                    </label>
                                    <label className="text-sm text-slate-600 sm:col-span-2">
                                        UID de la tarjeta (copiado del monitor Serial)
                                        <input required type="text" autoCapitalize="characters" maxLength={50} value={uidTarjeta} onChange={(evento) => setUidTarjeta(evento.target.value)} placeholder="A1 B2 C3 D4" className={campoClase} />
                                    </label>
                                </>
                            ) : (
                                <label className="text-sm text-slate-600 sm:col-span-2">
                                    Confirma tu contraseña para desvincular la tarjeta
                                    <input required type="password" autoComplete="current-password" value={contrasenaActual} onChange={(evento) => setContrasenaActual(evento.target.value)} className={campoClase} />
                                </label>
                            )}
                            {panelSeguridad === 'contrasena' && errorContrasena && <p role="alert" className="text-sm text-rose-600 sm:col-span-2">{errorContrasena}</p>}
                            {panelSeguridad !== 'contrasena' && errorTarjeta && <p role="alert" className="text-sm text-rose-600 sm:col-span-2">{errorTarjeta}</p>}
                            <div className="flex justify-end gap-2 sm:col-span-2">
                                <button type="button" onClick={() => {
                                    setPanelSeguridad(null)
                                    setContrasenaActual('')
                                    setNuevaContrasena('')
                                    setErrorContrasena('')
                                    setErrorTarjeta('')
                                }} className="rounded-lg border border-slate-200 px-4 py-2 text-sm text-slate-600 hover:bg-white">Cancelar</button>
                                <button type="submit" disabled={panelSeguridad === 'contrasena' ? guardandoContrasena : guardandoTarjeta} className="rounded-lg bg-amber-500 px-4 py-2 text-sm font-semibold text-slate-950 hover:bg-amber-400 disabled:cursor-wait disabled:opacity-60">
                                    {(panelSeguridad === 'contrasena' ? guardandoContrasena : guardandoTarjeta)
                                        ? 'Guardando...'
                                        : panelSeguridad === 'eliminar-tarjeta-rfid'
                                            ? 'Desvincular tarjeta'
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