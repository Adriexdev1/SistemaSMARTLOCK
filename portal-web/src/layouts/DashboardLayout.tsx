//Funcion para traer la pagina necesaria de la aplicacion
import {Outlet} from 'react-router-dom'
//Funcion de barra lateral y cabecera de la aplicacion
import {Sidebar} from '../components/Sidebar'
import {Header} from '../components/Header'

//Funcion principal para el dashboard
export default function DashboardLayout() {
    return (
        //Estructura de la aplicacion con barra lateral, cabecera y contenido principal
        <div className="flex h-screen bg-slate-950 text-slate-100 overflow-hidden">
            <Sidebar />
            <div className="flex-1 flex flex-col overflow-hidden">
                <Header />
                <main className="flex-1 p-6 overflow-y-auto">
                    <Outlet />
                </main>
            </div>
        </div>
    )   
}