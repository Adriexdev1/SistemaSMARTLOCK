//Liberia de iconos para la cabecera
import {Bell, Menu, User} from "lucide-react";
import { useState } from "react";
//Liberia para determinar la ubicacion dentro de la pagina web
import {Link, useLocation} from "react-router-dom";
//Libreria para navegar al inicio de sesion desde la cabecera
import { useNavigate } from "react-router-dom";

//Constante para determinar el nombre a mostrar en la cabecera dependiendo de la ubicacion dentro de la pagina web
const pageName: Record<string, string> = {
    //Se hace referencia a las rutas dentro de app.tsx las cuales hacen uso del dashboard y la sidebar
    '/inicio': 'Inicio',
    '/usuarios': 'Usuarios',
    '/eventos': 'Citas / Eventos',
    '/historial': 'Historial',
    '/incidentes': 'Incidentes',
    '/perfil': 'Perfil',
}

type HeaderProps = {
    alAbrirMenu: () => void
}

export function Header({ alAbrirMenu }: HeaderProps){
    //Constante para determinar la ubicacion actual en la pagina
    const location = useLocation();
    
    //Constante que determina basandose en la ubicacion actual el nombre a mostrar en la cabecera
    const currentPage = location.pathname.startsWith('/eventos/')
        ? 'Citas / Eventos'
        : pageName[location.pathname] ?? 'Error';
    
    //Constantes para determinar si el menu de usuario esta abierto o cerrado
    const [isMenuOpen, setIsMenuOpen] = useState(false);

    //Constante para navegar al inicio de sesion desde la cabecera
    const navigate = useNavigate()
    //Funcion encargada de cerrar sesion y redirigir al inicio de sesion
    function handleLogout() {
        //Envia al inicio de sesion 
        navigate('/login')
    }

    return (
        //Contenedor principal para la cabecera
        <header className="flex h-14 shrink-0 items-center justify-between gap-2 border-b border-gray-200 bg-white px-3 shadow-sm sm:px-6">
            {/*Titulo de la cabecera*/}
            <div className="flex min-w-0 items-center gap-2 sm:gap-3">
                {/*El botón de menú solo aparece en teléfono; md recupera la navegación lateral fija*/}
                <button type="button" onClick={alAbrirMenu} aria-label="Abrir menú de navegación" className="flex h-9 w-9 shrink-0 items-center justify-center rounded-md text-slate-600 hover:bg-slate-100 md:hidden">
                    <Menu size={20} />
                </button>
                <div className="flex min-w-0 flex-col">
                    <h1 className="truncate text-base font-semibold text-slate-800 sm:text-lg">{currentPage}</h1>
                    <span className="hidden truncate text-xs text-slate-500 sm:block sm:text-sm">Control de Acceso industrial</span>
                </div>
            </div>
            <div className="flex shrink-0 items-center gap-1 sm:gap-4">
                <button className="rounded-lg p-2 text-slate-500 transition-colors hover:bg-gray-100 hover:text-slate-800">
                    <Bell className="w-5 h-5" />
                </button>

                <div className="relative">
                    {/* Contenedor para el boton de perfil de usuario */} 
                    <button type="button" onClick={() => setIsMenuOpen(!isMenuOpen)}
                        className="flex items-center gap-3">
                    {/* Icono de usuario y rol*/} 
                    <div className="w-8 h-8 rounded-full bg-blue-600 flex items-center justify-center font-bold text-white">
                        <User className="w-4 h-4" />
                    </div>
                    </button>

                    {/*Menu desplegable*/}
                    {isMenuOpen && (
                        <div className="absolute right-0 top-full z-50 mt-2 w-48 rounded-lg border border-gray-200 bg-white p-2 shadow-lg">
                            <span className="block w-full border-b border-gray-200 pb-2 text-center text-base font-medium text-slate-700">Administrador</span>
                            <Link 
                            to="/perfil"
                            onClick={() => setIsMenuOpen(false)}
                            className="block rounded px-3 py-2 text-sm text-slate-600 hover:bg-gray-100">Perfil</Link>
                            <button
                             type="button"
                             onClick={handleLogout}
                             className="w-full rounded px-3 py-2 text-left text-sm text-red-600 hover:bg-red-50">Cerrar Sesión</button>
                        </div>
                    )}
                </div>
            </div>
        </header>
    )
}