//Librerias para el enrutamiento de la aplicacion 
import { BrowserRouter, Routes, Route, Navigate, Outlet } from 'react-router-dom'
//Funciones de las paginas de la aplicacion
import Login from './pages/Login'
import DashboardLayout from './layouts/DashboardLayout'
import Usuarios from './pages/Usuarios'
import Citas from './pages/Citas'
import Historial from './pages/Historial'
import Incidentes from './pages/Incidentes'
import Perfil from './pages/Perfil'
import Inicio from './pages/Inicio'
import AuthLayout from './layouts/AuthLayout'
import DetalleCita from './pages/DetalleCita'
import { CitasProvider } from './contexts/CitasContext'
import { obtenerUsuarioSesion } from './utils/usuarioSesion'

function RequireSession() {
  return obtenerUsuarioSesion()
    ? <Outlet />
    : <Navigate to="/login" replace />
}

export default function App() {
  return (
    <BrowserRouter>
        <Routes>
          <Route path="/" element={<Navigate to={obtenerUsuarioSesion() ? '/inicio' : '/login'} replace />} />
          {/* Ruta de la pagina de inicio de sesion */}
          <Route element={<AuthLayout/>}>
             <Route path="/login" element={<Login />} />
          </Route>

          {/* Rutas de la pagina principal del dashboard */}
          <Route element={<RequireSession />}>
          <Route element={<DashboardLayout />}>
           <Route path="/inicio" element={<Inicio/>} />
          <Route path="/usuarios" element={<Usuarios />} />
          <Route element={<CitasProvider />}>
            <Route path="/eventos" element={<Citas />} />
            <Route path="/eventos/:id" element={<DetalleCita />} />
          </Route>
          <Route path="/historial" element={<Historial />} />
          <Route path="/incidentes" element={<Incidentes />} />
          <Route path="/perfil" element={<Perfil />} />
          </Route>
          </Route>
        </Routes>
    </BrowserRouter>
  )
}