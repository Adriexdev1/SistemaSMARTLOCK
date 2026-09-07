export default function ProximasVisitas(){
    return(
        //Seccion para las proximas visitas
        <section className="rounded-lg border border-gray-200 bg-white p-5 shadow-sm">
            
            {/*Seccion de encabezado de la seccion*/}
            <h2 className="text-lg font-semibold text-slate-900">
                Proximas visitas
            </h2>

            {/*Seccion de citas proximas*/}
            <div className="mt-4 space-y-4">
                <p className="text-slate-600">8:00 - Carlos Mendoza</p>
                <p className="text-slate-600">9:30 - Grupo Logistica NL</p>
                <p className="text-slate-600">11:00 - Ing. Ramirez Torres</p>
            </div>
        </section>
    )
}