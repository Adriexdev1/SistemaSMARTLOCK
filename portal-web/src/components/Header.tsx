//Liberia de iconos para la cabecera
import {Bell, User} from "lucide-react";
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

export function Header(){
    //Constante para determinar la ubicacion actual en la pagina
    const location = useLocation();
    
    //Constante que determina basandose en la ubicacion actual el nombre a mostrar en la cabecera
    const currentPage = pageName[location.pathname] ?? 'Error';
    
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
        <header className="h-14 border-b border-slate-800 bg-slate-900/50 backdrop-blur px-6 flex items-center justify-between">
            {/*Titulo de la cabecera*/}
            <div className="flex flex-col">
                <h1 className="text-lg font-semibold text-slate-400">{currentPage}</h1>
                <span className="text-sm text-slate-500">Control de Acceso industrial</span>
            </div>
            <div className="flex items-center gap-4">
                <button className="p-2 text-slate-400 hover:text-white rounded-lg hover:bg-slate-800 transition-colors">
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
                        <div className="absolute right-0 top-full z-50 mt-2 w-48 rounded-lg bg-slate-800 p-2 shadow-lg">
                            <span className="block w-full border-b text-center text-base font-medium text-slate-400">Administrador</span>
                            <Link 
                            to="/perfil"
                            onClick={() => setIsMenuOpen(false)}
                            className="block rounded px-3 py-2 text-sm text-slate-300 hover:bg-slate-700">Perfil</Link>
                            <button
                             type="button"
                             onClick={handleLogout}
                             className="w-full rounded px-3 py-2 text-left text-sm text-red-300 hover:bg-red-500/40">Cerrar Sesión</button>
                        </div>
                    )}
                </div>
            </div>
        </header>
    )
}