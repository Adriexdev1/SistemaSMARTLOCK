export type UsuarioSesion = {
  idUsuario: number
  nombreUsuario: string
  correoUsuario: string
  telefonoUsuario: string | null
  rol: string | null
  permisoRol: number | null
  tieneTarjetaRfidAsignada: boolean
  contrasenaCaracteres?: number
}

export type NivelPermiso = 0 | 1 | 2

const claveSesion = 'smartlock.usuario'

function esNivelPermiso(valor: unknown): valor is NivelPermiso {
  return valor === 0 || valor === 1 || valor === 2
}

export function obtenerUsuarioSesion(): UsuarioSesion | null {
  const valor = sessionStorage.getItem(claveSesion)
  if (!valor) return null

  try {
    const usuario = JSON.parse(valor) as UsuarioSesion
    return typeof usuario.idUsuario === 'number' &&
      typeof usuario.nombreUsuario === 'string' &&
      typeof usuario.correoUsuario === 'string' &&
      esNivelPermiso(usuario.permisoRol)
      ? usuario
      : null
  } catch {
    return null
  }
}

export function tieneNivelPermiso(
  usuario: UsuarioSesion | null,
  niveles: readonly NivelPermiso[],
): boolean {
  return usuario !== null && esNivelPermiso(usuario.permisoRol) &&
    niveles.includes(usuario.permisoRol)
}

export function rutaInicialUsuario(usuario: UsuarioSesion | null): string {
  if (tieneNivelPermiso(usuario, [2])) return '/inicio'
  if (tieneNivelPermiso(usuario, [1])) return '/inicio'
  if (tieneNivelPermiso(usuario, [0])) return '/perfil'
  return '/login'
}

export function guardarUsuarioSesion(usuario: UsuarioSesion) {
  sessionStorage.setItem(claveSesion, JSON.stringify(usuario))
  window.dispatchEvent(new Event('smartlock.usuario.actualizado'))
}

export function actualizarEstadoTarjetaRfidSesion(tieneTarjetaRfidAsignada: boolean) {
  const usuario = obtenerUsuarioSesion()
  if (usuario) guardarUsuarioSesion({ ...usuario, tieneTarjetaRfidAsignada })
}

export function actualizarLongitudContrasenaSesion(contrasenaCaracteres: number) {
  const usuario = obtenerUsuarioSesion()
  if (usuario) guardarUsuarioSesion({ ...usuario, contrasenaCaracteres })
}

export function cerrarSesion() {
  sessionStorage.removeItem(claveSesion)
  window.dispatchEvent(new Event('smartlock.usuario.actualizado'))
}