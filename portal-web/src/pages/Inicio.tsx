//Importacion de componentes internos reutilizables
import CartaEstadistica from '../components/componentesPaginas/CartaEstadistica'
import ProximasVisitas from '../components/componentesPaginas/ProximasVisitas'
import ActividadReciente from '../components/componentesPaginas/ActividadReciente'
import AccesoRapido from '../components/componentesPaginas/AccesoRapido'


export default function Inicio() {
  return (
    <div>
      {/*Seccion superior*/}
      <section>
        <h1>Bienvenido, Jesus Herrera</h1>
        <p>Administrador. Planta Industrial NL</p>
      </section>

      {/*Seccion de tarjetas con informacion relevante*/}
      <section className="grid grid-cols-1 gap-4 md:grid-cols-2 xl:grid-cols-4 px-4 py-6">
        <CartaEstadistica title="Empleados" value="100" subtitle="Activos"/>
        <CartaEstadistica title="Accesos hoy" value="8" subtitle="Programados"/>
        <CartaEstadistica title="Autorizados" value="20" subtitle="Hoy"/>
        <CartaEstadistica title="Incidentes" value="3" subtitle="Pendientes"/>
      </section>

      <section className="grid grid-cols-1 gap-4 xl:grid-cols-2">
        <ActividadReciente/>
        <ProximasVisitas/>
      </section>
      <AccesoRapido/>
    </div>
  )
}