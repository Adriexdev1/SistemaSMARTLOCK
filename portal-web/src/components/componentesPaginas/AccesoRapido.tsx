//Libreria para navegar entre interfaces
import { Link } from "react-router-dom";

export default function AccesoRapido(){
    return(
        //Seccion de texto a mostrar en el componente
        <section className="mt-4 flex flex-col items-stretch gap-4 rounded-lg bg-slate-900 p-4 text-white sm:flex-row sm:items-center sm:justify-between sm:p-5">
            {/*En móvil el texto y el botón se apilan; en sm vuelven a quedar en una fila*/}
            <div>
                <h2 className="text-lg font-semibold">Acceso rápido</h2>
                <p className="mt-1 text-slate-300">Registra o programa una nueva cita</p>
            </div>
        <Link 
        to={"/eventos"}
        className="w-full rounded-md bg-amber-500 px-4 py-2 text-center font-medium text-slate-950 hover:bg-amber-400 sm:w-auto"
        >
            + Nuevo Acceso
        </Link>
        </section>
    )
}