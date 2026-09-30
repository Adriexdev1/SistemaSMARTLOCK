import { LockKeyhole } from "lucide-react";
//Importacion para las rutas
import { Outlet } from "react-router-dom";

const estadisticas = [
    {valor: '142', etiqueta: 'Empleados'},
    {valor: '67', etiqueta: 'Accesos hoy'},
    {valor: '3', etiqueta: 'Incidentes'},
]

export default function AuthLayout(){
    return (
        <div className="grid min-h-dvh grid-cols-1 lg:grid-cols-2">
            {/* En teléfonos y tablets las secciones se apilan; en escritorio se muestran en dos columnas. */}
            {/*Seccion del lado izquierdo*/}
            <section className="relative flex min-h-[340px] flex-col justify-between overflow-hidden bg-slate-900 p-5 text-white sm:min-h-[400px] sm:p-8 lg:min-h-dvh lg:p-10">
                {/*Componente de lado izquierdo*/}
                <div
                aria-hidden="true"
                className="pointer-events-none absolute inset-0"
                style={{
                    backgroundImage:
                    'radial-gradient(ellipse at 50% 48%, rgba(37,99,235,.30), transparent 68%), linear-gradient(rgba(255,255,255,.10) 1px, transparent 1px), linear-gradient(90deg, rgba(255,255,255,.10) 1px, transparent 1px)',
                    backgroundSize: 'auto, 48px 48px, 48px 48px',
                }}/>

                {/*Contenedor del icono*/}
                <div className="relative flex items-center gap-3">
                    <span className="flex h-11 w-11 items-center justify-center rounded-xl bg-amber-500 text-slate-950">
                        <LockKeyhole size={22} />
                    </span>
                    <span className="text-lg font-bold">SMARTLOCK</span>
                </div>

                {/*Contenedor de textos*/}
                <div className="relative my-8 max-w-lg sm:my-10 lg:my-0">
                    <p className="mb-5 font-bold inline-flex items-center gap-2 rounded-full border border-amber-400/30 bg-amber-400/10 px-4 py-2 text-sm text-amber-300">
                    <span
                    aria-hidden="true"
                    className="font-bold h-2 w-2 rounded-full bg-amber-400 shadow-[0_0_8px_rgba(251,191,36,0.8)]"/>
                    Sistema de Control
                    </p>
                    
                    <h1 className="text-3xl font-bold leading-tight sm:text-4xl">
                        Control de acceso inteligente.
                    </h1>
                    
                    <p className="mt-4 max-w-md text-slate-300">
                        Gestiona empleados, visitas y eventos de seguridad mediante
                        códigos QR y autenticación por PIN.
                    </p>
                </div>

                {/*Contenedor de tarjetas de estadisticas*/}
                {/* Las estadísticas siguen en una fila y amplían separación y padding desde sm. */}
                <div className="relative grid grid-cols-3 gap-2 sm:gap-4">
                    {estadisticas.map((dato) => (

                        <article
                        key={dato.etiqueta}
                        className="min-w-0 rounded-lg border border-white/10 bg-white/10 p-2.5 sm:rounded-xl sm:p-4">
                            
                        <strong className="block text-lg font-bold text-amber-400 drop-shadow-[0_0_8px_rgba(251,191,36,0.45)] sm:text-2xl">
                            {dato.valor}
                        </strong>
                        
                        <span className="mt-1 block break-words text-[11px] leading-tight text-slate-300 sm:text-sm">
                            {dato.etiqueta}
                        </span>
                        </article>
          ))}
        </div>
      </section>
      {/*Contenedor para aviso legal*/}
    {/* El formulario usa padding compacto en móvil y recupera espacio desde sm. */}
    <main className="flex min-h-[460px] items-center justify-center bg-slate-50 px-4 py-8 sm:min-h-[500px] sm:px-8 sm:py-10 lg:min-h-dvh">
        <div className="w-full max-w-md">
          <Outlet />
          <p className="mt-8 text-center text-sm text-slate-400">
            © 2026 SMARTLOCK · Control de Acceso Industrial
          </p>
        </div>
      </main>
    </div>
    )
}