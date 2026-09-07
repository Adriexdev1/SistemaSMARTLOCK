export default function ActividadReciente(){
    return(
        <section className="rounded-lg border border-gray-200 bg-white p-5 shadow-sm">
            {/*Seccion de titulo*/}
            <h2 className="text-lg font-semibold text-slate-900">
                Actividad Reciente
            </h2>

            {/*Seccion de actividad*/}
            <div className="mt-4 space-y-4">
                <p className="text-slate-600">Acceso autorizado - Carlos Mendoza</p>
                <p className="text-slate-600">Nuevo acceso programado</p>
                <p className="text-slate-600">QR generado - Ing Ramirez Torres</p>
            </div>
        </section>
    )
}