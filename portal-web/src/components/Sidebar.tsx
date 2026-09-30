    //Liberia de herramientas para navegacion en la pagina web
    import {Link, useLocation} from 'react-router-dom'
    //Liberia de iconos para la barra lateral
    import {House, Users, Calendar, RotateCcwClock, AlertTriangle, User, LockKeyhole,LogOut} from 'lucide-react'
    //Libreria para navegacion en la pagina web
    import { useNavigate } from 'react-router-dom'

    //Funcion para la barra lateral de la pagina web
    type SidebarProps = {
        abierto: boolean
        alCerrar: () => void
    }

    export function Sidebar({ abierto, alCerrar }: SidebarProps) {
        //Constante para obtener la ubicacion actual de la pagina web
        const location = useLocation()
        
        //Funcion con todos los elementos del menu de la barra lateral de la pagina web
        const menuItems = [
            { name: 'Inicio', path: '/inicio', icon: House },
            { name: 'Usuarios', path: '/usuarios', icon: Users },
            { name: 'Citas / Eventos', path: '/eventos', icon: Calendar },
            { name: 'Historial', path: '/historial', icon: RotateCcwClock },
            { name: 'Incidentes', path: '/incidentes', icon: AlertTriangle },
            { name: 'Perfil', path: '/perfil', icon: User },
        ]

        //Constante para volver al inicio de sesion
        const navigate = useNavigate()
        
        //Funcion encargada de cerrar sesion y redirigir al inicio de sesion
        function handleLogout() {
            //Envia al inicio de sesion 
            navigate('/login')
        }


        return (
            <>
            {abierto && (
                <button
                    type="button"
                    aria-label="Cerrar menú de navegación"
                    onClick={alCerrar}
                    className="fixed inset-0 z-40 bg-slate-950/40 md:hidden"
                />
            )}
            {/*En teléfono la barra se abre sobre el contenido; desde md queda fija al lado*/}
            <aside className={`fixed inset-y-0 left-0 z-50 flex h-dvh w-60 shrink-0 flex-col bg-[#111827] text-slate-100 transition-transform duration-200 md:static md:z-auto md:h-screen md:translate-x-0 ${abierto ? 'translate-x-0' : '-translate-x-full'}`}>
                {/* Contenedor de la lista de elementos del menu de la barra lateral */}
                <div>
                    {/*Contenedor para el titulo de la barra lateral*/}
                    <div className="flex items-center gap-3 px-6 py-5 border-b border-slate-700 mb-6">
                        <div className="w-10 h-10 bg-amber-500 px-2.5 py-1 rounded-lg font-bold text-white flex items-center justify-center shrink-10"><LockKeyhole size={32} color="black" strokeWidth={4}/></div>
                        {/* Contenedor para el titulo y subtitulo de la barra lateral */}
                        <div className="flex flex-col">
                            <span className="font-bold text-lg text-white tracking-wide leading-none">SMARTLOCK</span>
                            <span className="text-[11px] text-slate-400">Control de Acceso</span>
                        </div>
                    </div>
                    {/* Contenedor para la lista de elementos del menu de la barra lateral */}
                    <nav className="space-y-1 px-2">
                        {menuItems.map((item) => {
                            //Constante para determinar el icono
                            const Icon = item.icon
                            //Constante para determinar si el elemento del menu existe
                            const isActive = location.pathname === item.path
                            return (
                            //Componente de enlace para la barra lateral
                            <Link
                                key={item.path}
                                to={item.path}
                                onClick={alCerrar}
                                className={`flex items-center gap-3 px-3 py-2.5 rounded-lg text-base font-medium transition-colors border-r- ${
                                    isActive
                                    ? 'text-amber-400 bg-amber-500/10 border-amber-400 border'
                                    :  'text-slate-400 hover:bg-slate-800 hover:text-slate-200'
                                }`}
                            >
                                <Icon className="w-5 h-5" />
                                {item.name}
                            </Link>
                        )
                    })}
                    </nav>
                    </div>
                    {/* Contenedor para boton de cerrar sesion*/}
                    <div className="mt-auto border-t border-slate-700 pt-1">
                        <div className="px-2 py-2">
                            {/* Boton para cerrar sesion */}
                            <button type="button"
                            onClick={handleLogout}
                            className="flex w-full items-center gap-3 px-3 py-2.5 text-base font-medium text-slate-400 hover:bg-red-500/40 hover:text-red-300 rounded-lg transition-colors duration-500">
                                <LogOut className="w-5 h-5" />
                                Cerrar Sesión 
                            </button>
                        </div>
                    </div>
            </aside>
            </>
        )
    }