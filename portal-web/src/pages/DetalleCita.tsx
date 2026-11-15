import { useEffect, useState, type FormEvent } from 'react'
import { Link, useParams, useSearchParams } from 'react-router-dom'
import { ArrowLeft, Pencil } from 'lucide-react'
import { QRCodeSVG } from 'qrcode.react'
import { useCitas } from '../contexts/CitasContext'
import type { Cita } from '../contexts/CitasContext'

function fechaVisible(fecha: string) {
	const [anio, mes, dia] = fecha.split('-')
	return `${dia} de ${new Date(Number(anio), Number(mes) - 1, Number(dia)).toLocaleDateString('es-MX', { month: 'long' })} de ${anio}`
}

export default function DetalleCita() {
	const { id } = useParams()
	const [searchParams, setSearchParams] = useSearchParams()
	const { citas, usuarios, actualizarCita, agregarParticipante, eliminarParticipante, generarQr } = useCitas()
	const cita = citas.find((acceso) => acceso.id === Number(id))
	const editando = searchParams.get('editar') === '1'
	const [borrador, setBorrador] = useState<Cita | null>(cita ?? null)
	const [guardando, setGuardando] = useState(false)
	const [error, setError] = useState('')
	const [usuariosSeleccionados, setUsuariosSeleccionados] = useState<number[]>([])

	useEffect(() => {
		if (cita) setBorrador({ ...cita })
	}, [cita, editando])

	async function guardarCambios(evento: FormEvent<HTMLFormElement>) {
		evento.preventDefault()
		if (!borrador) return
		setGuardando(true)
		setError('')
		try {
			await actualizarCita(borrador)
			setSearchParams({}, { replace: true })
		} catch (errorGuardado) {
			setError(errorGuardado instanceof Error ? errorGuardado.message : 'No se pudo actualizar el acceso.')
		} finally {
			setGuardando(false)
		}
	}

	async function regenerarQr(idDetalleEvento: number) {
		setGuardando(true)
		setError('')
		try {
			await generarQr(idDetalleEvento)
		} catch (errorQr) {
			setError(errorQr instanceof Error ? errorQr.message : 'No se pudo generar el QR.')
		} finally {
			setGuardando(false)
		}
	}

	async function agregarUsuariosAlEvento() {
		if (!cita || usuariosSeleccionados.length === 0) return
		setGuardando(true)
		setError('')
		try {
			for (const idUsuario of usuariosSeleccionados) {
				await agregarParticipante(cita.id, idUsuario)
			}
			setUsuariosSeleccionados([])
		} catch (errorAgregar) {
			setError(errorAgregar instanceof Error ? errorAgregar.message : 'No se pudieron agregar los participantes.')
		} finally {
			setGuardando(false)
		}
	}

	async function quitarParticipante(idDetalleEvento: number) {
		if (!cita) return
		setGuardando(true)
		setError('')
		try {
			await eliminarParticipante(cita.id, idDetalleEvento)
		} catch (errorEliminar) {
			setError(errorEliminar instanceof Error ? errorEliminar.message : 'No se pudo quitar el participante.')
		} finally {
			setGuardando(false)
		}
	}

	if (!cita || !borrador) {
		return (
			<section className="rounded-xl border border-slate-200 bg-white p-6 text-center">
				<h2 className="font-semibold text-slate-900">No se encontró este acceso</h2>
				<Link to="/eventos" className="mt-3 inline-block text-sm font-medium text-orange-600 hover:text-orange-700">
					Volver a citas / accesos
				</Link>
			</section>
		)
	}

	const campos = [
		{ etiqueta: 'Tipo de acceso', valor: cita.tipo },
		{ etiqueta: 'Fecha', valor: fechaVisible(cita.fecha) },
		{ etiqueta: 'Horario', valor: `${cita.hora} - ${cita.horaFin}` },
		{ etiqueta: 'Estado', valor: cita.estado },
	]

	const usuariosDisponibles = usuarios.filter((usuario) => !cita.participantes.some((participante) => participante.idUsuario === usuario.id))
	const inputClass = 'mt-1 h-10 w-full rounded-lg border border-slate-200 bg-white px-3 text-sm text-slate-800 focus:border-amber-400 focus:outline-none focus:ring-2 focus:ring-amber-100'

	return (
		<div className="mx-auto w-full max-w-[1600px]">
			<nav aria-label="Ruta de navegación" className="mb-4 flex flex-wrap items-center gap-2 text-sm text-slate-500">
				<Link to="/eventos" className="hover:text-orange-600">Citas / Accesos</Link>
				<span aria-hidden="true">/</span>
				<span className="text-slate-700">Detalle #{cita.id}</span>
			</nav>

			<section className="mb-5 flex flex-wrap items-center justify-between gap-4">
				<div className="flex flex-wrap items-center gap-3">
					<h2 className="text-xl font-semibold text-slate-900">Evento #{cita.id}</h2>
					<span className={`rounded-full px-2.5 py-1 text-xs font-semibold ${cita.estado === 'Activo' ? 'bg-emerald-100 text-emerald-700' : 'bg-blue-100 text-blue-700'}`}>
						{cita.estado}
					</span>
				</div>
				<div className="flex flex-wrap gap-2">
					<button
						type="button"
						onClick={() => setSearchParams(editando ? {} : { editar: '1' })}
						className="inline-flex items-center gap-2 rounded-lg border border-slate-200 bg-white px-3 py-2 text-sm text-slate-600 hover:bg-slate-50"
					>
						<Pencil size={16} /> {editando ? 'Cancelar edición' : 'Editar evento'}
					</button>
				</div>
			</section>

			{editando ? (
				<form onSubmit={guardarCambios} className="rounded-xl border border-slate-200 bg-white p-5 shadow-sm sm:p-7">
					<div className="mb-5 flex items-center gap-2 border-b border-slate-100 pb-4">
						<ArrowLeft size={18} className="text-slate-400" />
						<h3 className="font-semibold text-slate-800">Editar información del acceso</h3>
					</div>
					<div className="grid gap-4 sm:grid-cols-2">
						<label className="text-sm text-slate-600">Tipo de acceso
							<input required value={borrador.tipo} onChange={(evento) => setBorrador({ ...borrador, tipo: evento.target.value })} className={inputClass} />
						</label>
						<label className="text-sm text-slate-600">Fecha
							<input required type="date" value={borrador.fecha} onChange={(evento) => setBorrador({ ...borrador, fecha: evento.target.value })} className={inputClass} />
						</label>
						<div className="grid grid-cols-2 gap-3">
							<label className="text-sm text-slate-600">Hora inicio
								<input required type="time" value={borrador.hora} onChange={(evento) => setBorrador({ ...borrador, hora: evento.target.value })} className={inputClass} />
							</label>
							<label className="text-sm text-slate-600">Hora fin
								<input required type="time" value={borrador.horaFin} onChange={(evento) => setBorrador({ ...borrador, horaFin: evento.target.value })} className={inputClass} />
							</label>
						</div>
						<label className="text-sm text-slate-600">Estado
							<select value={borrador.estado} onChange={(evento) => setBorrador({ ...borrador, estado: evento.target.value as Cita['estado'] })} className={inputClass}>
								<option>Activo</option><option>Completado</option>
							</select>
						</label>
					</div>
					<div className="mt-6 flex flex-wrap justify-end gap-2">
						<button type="button" onClick={() => setSearchParams({}, { replace: true })} className="rounded-lg border border-slate-200 px-4 py-2 text-sm text-slate-600 hover:bg-slate-50">Cancelar</button>
						<button type="submit" disabled={guardando} className="rounded-lg bg-amber-500 px-4 py-2 text-sm font-semibold text-slate-950 hover:bg-amber-400 disabled:cursor-wait disabled:opacity-60">{guardando ? 'Guardando...' : 'Guardar cambios'}</button>
					</div>
					{error && <p role="alert" className="mt-4 text-sm text-rose-600">{error}</p>}
				</form>
			) : (
				<>
					<section className="grid gap-4 xl:grid-cols-2">
						<article className="rounded-xl border border-slate-200 bg-white p-5 shadow-sm sm:p-6">
							<h3 className="mb-4 border-b border-slate-100 pb-3 font-semibold text-slate-800">Información del evento</h3>
							<dl className="space-y-4">
								{campos.map((campo) => (
									<div key={campo.etiqueta} className="flex flex-wrap items-start justify-between gap-x-4 gap-y-1 text-sm">
										<dt className="text-slate-500">{campo.etiqueta}</dt>
										<dd className="text-right font-medium text-slate-800">{campo.valor}</dd>
									</div>
								))}
							</dl>
						</article>

					</section>

					<section className="mt-4 overflow-hidden rounded-xl border border-slate-200 bg-white shadow-sm">
						<div className="border-b border-slate-100 px-5 py-4">
							<h3 className="font-semibold text-slate-800">Participantes ({cita.participantes.length})</h3>
							<p className="mt-1 text-sm text-slate-500">Cada participante tiene un QR individual.</p>
						</div>
						<div className="grid gap-4 p-5">
							{cita.participantes.map((participante) => (
								<article key={participante.idDetalleEvento} className="rounded-lg border border-slate-200 p-4">
									<div className="flex flex-wrap items-start justify-between gap-3">
										<div>
											<p className="font-medium text-slate-800">{participante.nombre}</p>
											<p className="mt-1 text-xs text-slate-500">QR: {participante.qr}</p>
										</div>
										<div className="flex gap-2">
											{participante.qr === 'Pendiente' && <button type="button" disabled={guardando} onClick={() => void regenerarQr(participante.idDetalleEvento)} className="rounded-md bg-amber-500 px-3 py-2 text-sm font-semibold text-slate-950 disabled:opacity-60">{guardando ? 'Generando...' : 'Generar QR'}</button>}
											<button type="button" disabled={guardando} onClick={() => void quitarParticipante(participante.idDetalleEvento)} className="rounded-md border border-rose-200 px-3 py-2 text-sm text-rose-700 disabled:opacity-60">Quitar</button>
										</div>
									</div>
									{participante.qrToken && <div className="mt-3 flex flex-wrap items-center gap-4 border-t border-slate-100 pt-3"><QRCodeSVG value={participante.qrToken} size={140} level="M" includeMargin /><p className="break-all font-mono text-xs text-slate-500">{participante.qrToken}</p></div>}
									{participante.historial.length > 0 && <ul className="mt-3 space-y-1 border-t border-slate-100 pt-3 text-sm text-slate-600">{participante.historial.map((registro) => <li key={registro.id}>Acceso: {registro.fecha} {registro.hora} · {registro.resultado}</li>)}</ul>}
								</article>
							))}
							<label className="block text-sm text-slate-600">
								Agregar participantes
								<select multiple size={Math.min(Math.max(usuariosDisponibles.length, 2), 5)} value={usuariosSeleccionados.map(String)} onChange={(evento) => setUsuariosSeleccionados(Array.from(evento.target.selectedOptions, (opcion) => Number(opcion.value)))} className="mt-2 w-full rounded-lg border border-slate-200 bg-white p-2">
									{usuariosDisponibles.map((usuario) => <option key={usuario.id} value={usuario.id}>{usuario.nombre}</option>)}
								</select>
							</label>
							{error && <p role="alert" className="text-sm text-rose-600">{error}</p>}
							<div className="flex justify-end">
								<button type="button" disabled={guardando || usuariosSeleccionados.length === 0} onClick={() => void agregarUsuariosAlEvento()} className="rounded-lg bg-amber-500 px-4 py-2 text-sm font-semibold text-slate-950 disabled:cursor-not-allowed disabled:opacity-50">{guardando ? 'Guardando...' : 'Agregar seleccionados'}</button>
							</div>
						</div>
					</section>
				</>
			)}
		</div>
	)
}
