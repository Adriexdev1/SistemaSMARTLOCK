//Funcion para traer la pagina necesaria de la aplicacion
import {Outlet} from 'react-router-dom'
//Funcion de barra lateral y cabecera de la aplicacion
import {Sidebar} from '../components/Sidebar'
import {Header} from '../components/Header'

//Funcion principal para el dashboard
export default function DashboardLayout() {
    return (
        //Estructura de la aplicacion con barra lateral, cabecera y contenido principal
        <div className="flex h-screen overflow-hidden bg-gray-100 text-slate-900">
            <Sidebar />
            <div className="flex-1 flex flex-col overflow-hidden">
                <Header />
                <main className="flex-1 overflow-y-auto bg-gray-50 p-6">
                    <Outlet />
                </main>
            </div>
        </div>
    )   
}