export type UsuarioSesion = {
  idUsuario: number
  nombreUsuario: string
  correoUsuario: string
  telefonoUsuario: string | null
  rol: string | null
  tienePinConfigurado: boolean
  contrasenaCaracteres?: number
}

const claveSesion = 'smartlock.usuario'

export function obtenerUsuarioSesion(): UsuarioSesion | null {
  const valor = sessionStorage.getItem(claveSesion)
  if (!valor) return null

  try {
    const usuario = JSON.parse(valor) as UsuarioSesion
    return typeof usuario.idUsuario === 'number' &&
      typeof usuario.nombreUsuario === 'string' &&
      typeof usuario.correoUsuario === 'string'
      ? usuario
      : null
  } catch {
    return null
  }
}

export function guardarUsuarioSesion(usuario: UsuarioSesion) {
  sessionStorage.setItem(claveSesion, JSON.stringify(usuario))
  window.dispatchEvent(new Event('smartlock.usuario.actualizado'))
}

export function actualizarEstadoPinSesion(tienePinConfigurado: boolean) {
  const usuario = obtenerUsuarioSesion()
  if (usuario) guardarUsuarioSesion({ ...usuario, tienePinConfigurado })
}

export function actualizarLongitudContrasenaSesion(contrasenaCaracteres: number) {
  const usuario = obtenerUsuarioSesion()
  if (usuario) guardarUsuarioSesion({ ...usuario, contrasenaCaracteres })
}

export function cerrarSesion() {
  sessionStorage.removeItem(claveSesion)
  window.dispatchEvent(new Event('smartlock.usuario.actualizado'))
}