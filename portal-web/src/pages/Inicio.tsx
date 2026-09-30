//Importacion de componentes internos reutilizables
import CartaEstadistica from '../components/componentesPaginas/CartaEstadistica'
import ProximasVisitas from '../components/componentesPaginas/ProximasVisitas'
import ActividadReciente from '../components/componentesPaginas/ActividadReciente'
import AccesoRapido from '../components/componentesPaginas/AccesoRapido'
import { AlertTriangle, CalendarDays, CircleCheck, Users } from 'lucide-react'

export default function Inicio() {
  return (
    <div>
      {/*Seccion superior*/}
      <section>
        <h1 className="text-lg font-semibold text-slate-800">Bienvenido, Jesus Herrera</h1>
        <p className="text-sm text-slate-400 sm:text-base">Administrador. Planta Industrial NL</p>
      </section>

      {/*Seccion de tarjetas con informacion relevante*/}
      {/*Las tarjetas pasan de una columna en móvil a cuatro en pantallas amplias*/}
      <section className="grid grid-cols-1 gap-4 py-5 sm:grid-cols-2 xl:grid-cols-4">
        <CartaEstadistica icon={Users} iconClassName="bg-blue-500" title="Empleados" value="100" subtitle="Activos"/>
        <CartaEstadistica icon={CalendarDays} iconClassName="bg-emerald-500" title="Accesos hoy" value="8" subtitle="Programados"/>
        <CartaEstadistica icon={CircleCheck} iconClassName="bg-amber-500" title="Autorizados" value="20" subtitle="Hoy"/>
        <CartaEstadistica icon={AlertTriangle} iconClassName="bg-rose-500" title="Incidentes" value="3" subtitle="Pendientes"/>
      </section>

      {/*Los paneles se apilan en teléfono y quedan lado a lado desde xl*/}
      <section className="grid grid-cols-1 gap-4 xl:grid-cols-2">
        <ActividadReciente/>
        <ProximasVisitas/>
      </section>
      <AccesoRapido/>
    </div>
  )
}