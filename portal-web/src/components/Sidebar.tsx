    //Liberia de herramientas para navegacion en la pagina web
    import {Link, useLocation} from 'react-router-dom'
    //Liberia de iconos para la barra lateral
    import {House, Users, Calendar, ShieldCheck, AlertTriangle, User, LockKeyhole} from 'lucide-react'

    //Funcion para la barra lateral de la pagina web
    export function Sidebar() {
        //Constante para obtener la ubicacion actual de la pagina web
        const location = useLocation()
        
        //Funcion con todos los elementos del menu de la barra lateral de la pagina web
        const menuItems = [
            { name: 'Inicio', path: '/dashboard', icon: House },
            { name: 'Usuarios', path: '/usuarios', icon: Users },
            { name: 'Citas / Eventos', path: '/eventos', icon: Calendar },
            { name: 'Accesos', path: '/accesos', icon: ShieldCheck },
            { name: 'Incidentes', path: '/incidentes', icon: AlertTriangle },
            { name: 'Perfil', path: '/perfil', icon: User },
        ]

        return (
            //Contenedor central para la estructura de la barra lateral
            <aside className="w-64 bg-slate-800 text-slate-100 flex-shrink-0">
                {/* Contenedor de la lista de elementos del menu de la barra lateral */}
                <div>
                    {/*Contenedor para el titulo de la barra lateral*/}
                    <div className="flex items-center gap-3 px-2 py-3 border-b border-slate-700 mb-6">
                        <div className="w-10 h-10 bg-amber-500 px-2.5 py-1 rounded-lg font-bold text-white flex items-center justify-center shrink-10"><LockKeyhole size={32} color="black" strokeWidth={4}/></div>
                        {/* Contenedor para el titulo y subtitulo de la barra lateral */}
                        <div className="flex flex-col">
                            <span className="font-bold text-lg text-white tracking-wide leading-none">SMARTLOCK</span>
                            <span className="text-[11px] text-slate-400">Control de Acceso</span>
                        </div>
                    </div>
                    {/* Contenedor para la lista de elementos del menu de la barra lateral */}
                    <nav className="space-y-1">
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
                                className={`flex items-center gap-3 px-3 py-2.5 rounded-lg text-sm font-medium transition-colors ${
                                    isActive
                                    ? 'bg-blue-600 text-white'
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
            </aside>
        )
    }