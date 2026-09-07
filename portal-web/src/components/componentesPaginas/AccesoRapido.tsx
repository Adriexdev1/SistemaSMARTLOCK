//Libreria para navegar entre interfaces
import { Link } from "react-router-dom";

export default function AccesoRapido(){
    return(
        //Seccion de texto a mostrar en el componente
        <section className="mt-4 flex items-center justify-between rounded-lg bg-slate-900 p-5 text-white">
            <div>
                <h2 className="text-lg font-semibold">Acceso rápido</h2>
                <p className="mt-1 text-slate-300">Registra o programa una nueva cita</p>
            </div>
        <Link 
        to={"/eventos"}
        className="rounded-md bg-amber-500 px-4 py-2 font-medium text-slate-950 hover:bg-amber-400"
        >
            + Nuevo Acceso
        </Link>
        </section>
    )
}