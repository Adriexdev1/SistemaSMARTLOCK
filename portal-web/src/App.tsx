//Librerias para el enrutamiento de la aplicacion 
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
//Funciones de las paginas de la aplicacion
import Dashboard from './pages/Dashboard'
import Login from './pages/Login'
import DashboardLayout from './layouts/DashboardLayout'

export default function App() {
  return (
    <BrowserRouter>
        <Routes>
          {/* Ruta de la pagina de inicio de sesion */}
          <Route path="/login" element={<Login />} />

          {/* Rutas de la pagina principal del dashboard */}
          <Route element={<DashboardLayout />}>
           <Route path="/" element={<Navigate to="/dashboard" replace />} />
           <Route path="/dashboard" element={<Dashboard />} />
          <Route path="/usuarios" element={<div>Usuarios</div>} />
          <Route path="/eventos" element={<div>Eventos</div>} />
          <Route path="/accesos" element={<div>Historial</div>} />
          <Route path="/incidentes" element={<div>Incidentes</div>} />
          <Route path="/perfil" element={<div>Perfil</div>} />
          </Route>
        </Routes>
    </BrowserRouter>
  )
}