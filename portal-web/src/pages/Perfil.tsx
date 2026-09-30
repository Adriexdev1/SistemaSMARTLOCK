import { useState, type FormEvent } from 'react'
import { Check, KeyRound, LockKeyhole, Pencil, ShieldCheck, X } from 'lucide-react'

type DatosPerfil = {
    nombre: string
    correo: string
    telefono: string
    rol: string
}

type PanelSeguridad = 'contrasena' | 'pin' | null

const datosIniciales: DatosPerfil = {
    nombre: 'Jesús Herrera Valdez',
    correo: 'admin@smartlock.com',
    telefono: '+52 (844) 123 4567',
    rol: 'Administrador',
}

function inicialesDe(nombre: string) {
    return nombre
        .trim()
        .split(/\s+/)
        .slice(0, 2)
        .map((parte) => parte[0]?.toUpperCase() ?? '')
        .join('')
}

export default function Perfil() {
    const [perfil, setPerfil] = useState(datosIniciales)
    const [perfilBorrador, setPerfilBorrador] = useState(datosIniciales)
    const [editandoPerfil, setEditandoPerfil] = useState(false)
    const [panelSeguridad, setPanelSeguridad] = useState<PanelSeguridad>(null)
    const [mensaje, setMensaje] = useState('')

    function guardarPerfil(evento: FormEvent<HTMLFormElement>) {
        evento.preventDefault()
        setPerfil(perfilBorrador)
        setEditandoPerfil(false)
        setMensaje('La información del perfil se actualizó.')
    }

    function alternarEdicion() {
        setPerfilBorrador(perfil)
        setEditandoPerfil((actual) => !actual)
        setMensaje('')
    }

    function guardarSeguridad(evento: FormEvent<HTMLFormElement>) {
        evento.preventDefault()
        setPanelSeguridad(null)
        setMensaje(panelSeguridad === 'pin' ? 'La configuración del PIN se guardó.' : 'La contraseña se actualizó.')
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
                                <div className="flex items-end justify-end sm:col-span-2">
                                    <button type="submit" className="inline-flex h-10 items-center gap-2 rounded-lg bg-amber-500 px-4 text-sm font-semibold text-slate-950 hover:bg-amber-400">
                                        <Check size={16} /> Guardar cambios
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
                                    <p className="mt-1 text-sm tracking-[0.2em] text-slate-400">••••••••••••</p>
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
                                    <p className="mt-1 text-sm font-medium text-emerald-600">PIN habilitado</p>
                                </div>
                            </div>
                            <button
                                type="button"
                                onClick={() => setPanelSeguridad(panelSeguridad === 'pin' ? null : 'pin')}
                                className="rounded-lg border border-slate-200 px-3 py-2 text-sm text-slate-600 transition-colors hover:bg-slate-50"
                            >
                                Configurar PIN
                            </button>
                        </div>
                    </div>

                    {panelSeguridad && (
                        <form onSubmit={guardarSeguridad} className="mt-2 grid gap-4 rounded-lg bg-slate-50 p-4 sm:grid-cols-2">
                            {panelSeguridad === 'contrasena' ? (
                                <>
                                    <label className="text-sm text-slate-600">Contraseña actual<input required type="password" autoComplete="current-password" className={campoClase} /></label>
                                    <label className="text-sm text-slate-600">Nueva contraseña<input required type="password" minLength={8} autoComplete="new-password" className={campoClase} /></label>
                                </>
                            ) : (
                                <label className="text-sm text-slate-600 sm:col-span-2">Nuevo PIN<input required type="password" inputMode="numeric" pattern="[0-9]{4,8}" minLength={4} maxLength={8} placeholder="4 a 8 dígitos" className={campoClase} /></label>
                            )}
                            <div className="flex justify-end gap-2 sm:col-span-2">
                                <button type="button" onClick={() => setPanelSeguridad(null)} className="rounded-lg border border-slate-200 px-4 py-2 text-sm text-slate-600 hover:bg-white">Cancelar</button>
                                <button type="submit" className="rounded-lg bg-amber-500 px-4 py-2 text-sm font-semibold text-slate-950 hover:bg-amber-400">Guardar</button>
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