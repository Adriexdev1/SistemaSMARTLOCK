export function crearContenidoQrPuerta(idDetalleEvento: number, codigoQr: string): string {
  return `id=${idDetalleEvento}&token=${encodeURIComponent(codigoQr)}`
}
