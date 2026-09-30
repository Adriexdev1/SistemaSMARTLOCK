//Funcion para traer la pagina necesaria de la aplicacion
import { useState } from 'react'
import { Outlet } from 'react-router-dom'
//Funcion de barra lateral y cabecera de la aplicacion
import { Sidebar } from '../components/Sidebar'
import { Header } from '../components/Header'

//Funcion principal para el dashboard
export default function DashboardLayout() {
    const [menuMovilAbierto, setMenuMovilAbierto] = useState(false)

    return (
        //Estructura de la aplicacion con barra lateral, cabecera y contenido principal
        <div className="flex h-dvh overflow-hidden bg-gray-100 text-slate-900">
            <Sidebar
                abierto={menuMovilAbierto}
                alCerrar={() => setMenuMovilAbierto(false)}
            />
            <div className="flex min-w-0 flex-1 flex-col overflow-hidden">
                <Header alAbrirMenu={() => setMenuMovilAbierto(true)} />
                {/*El contenido usa menos margen en telefono y aumenta al crecer la pantalla*/}
                <main className="min-w-0 flex-1 overflow-y-auto bg-gray-50 p-3 sm:p-4 lg:p-6">
                    <Outlet />
                </main>
            </div>
        </div>
    )   
}