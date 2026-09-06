
import {Bell, User} from "lucide-react";

export function Header(){
    return (
        //Contenedor principal para la cabecera
        <header className="h-16 border-b border-slate-800 bg-slate-900/50 backdrop-blur px-6 flex items-center justify-between">
            {/*Titulo de la cabecera*/}
            <h2 className="text-sm font-semibold text-slate-400">Portal de Control</h2>
            <div className="flex items-center gap-4">
                <button className="p-2 text-slate-400 hover:text-white rounded-lg hover:bg-slate-800 transition-colors">
                    <Bell className="w-5 h-5" />
                </button>

                <div className="flex items-center gap-3 pl-4 border-l border-slate-800">
                    <div className="w-8 h-8 rounded-full bg-blue-600 flex items-center justify-center font-bold text-white">
                        <User className="w-4 h-4" />
                    </div>
                    <span className="text-sm font-medium text-slate-400">Administrador</span>
                </div>
            </div>
        </header>
    )
}