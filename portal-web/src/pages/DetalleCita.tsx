import { useEffect, useState, type FormEvent } from 'react'
import { Link, useParams, useSearchParams } from 'react-router-dom'
import { ArrowLeft, Pencil, QrCode } from 'lucide-react'
import { QRCodeSVG } from 'qrcode.react'
import DataTable from '../components/TablaBase'
import { useCitas } from '../contexts/CitasContext'
import type { Cita } from '../contexts/CitasContext'

function fechaVisible(fecha: string) {
	const [anio, mes, dia] = fecha.split('-')
	return `${dia} de ${new Date(Number(anio), Number(mes) - 1, Number(dia)).toLocaleDateString('es-MX', { month: 'long' })} de ${anio}`
}

export default function DetalleCita() {
	const { id } = useParams()
	const [searchParams, setSearchParams] = useSearchParams()
	const { citas, actualizarCita } = useCitas()
	const cita = citas.find((acceso) => acceso.id === Number(id))
	const editando = searchParams.get('editar') === '1'
	const [borrador, setBorrador] = useState<Cita | null>(cita ?? null)

	useEffect(() => {
		if (cita) setBorrador({ ...cita })
	}, [cita, editando])

	function guardarCambios(evento: FormEvent<HTMLFormElement>) {
		evento.preventDefault()
		if (!borrador) return
		actualizarCita(borrador)
		setSearchParams({}, { replace: true })
	}

	function regenerarQr() {
		if (!cita) return
		actualizarCita({
			...cita,
			qr: 'Generado',
			qrToken: `SMARTLOCK-ACCESS-${cita.id}-${crypto.randomUUID()}`,
		})
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
		{ etiqueta: 'Visitante / Empleado', valor: cita.visitante },
		{ etiqueta: 'Tipo de acceso', valor: cita.tipo },
		{ etiqueta: 'Fecha', valor: fechaVisible(cita.fecha) },
		{ etiqueta: 'Horario', valor: `${cita.hora} - ${cita.horaFin}` },
		{ etiqueta: 'Estado', valor: cita.estado },
	]

	const columnasHistorial = [
		{ header: 'Fecha', width: '22%', render: (registro: Cita['historial'][number]) => registro.fecha },
		{ header: 'Hora', width: '18%', render: (registro: Cita['historial'][number]) => registro.hora },
		{ header: 'Resultado', width: '25%', render: (registro: Cita['historial'][number]) => registro.resultado },
	]

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
					<h2 className="text-xl font-semibold text-slate-900">Acceso #{cita.id}</h2>
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
						<Pencil size={16} /> {editando ? 'Cancelar edición' : 'Editar acceso'}
					</button>
					{!editando && (
						<button
							type="button"
							onClick={regenerarQr}
							className="inline-flex items-center gap-2 rounded-lg bg-amber-500 px-3 py-2 text-sm font-semibold text-slate-950 hover:bg-amber-400"
						>
							<QrCode size={16} /> Generar nuevo QR
						</button>
					)}
				</div>
			</section>

			{editando ? (
				<form onSubmit={guardarCambios} className="rounded-xl border border-slate-200 bg-white p-5 shadow-sm sm:p-7">
					<div className="mb-5 flex items-center gap-2 border-b border-slate-100 pb-4">
						<ArrowLeft size={18} className="text-slate-400" />
						<h3 className="font-semibold text-slate-800">Editar información del acceso</h3>
					</div>
					<div className="grid gap-4 sm:grid-cols-2">
						<label className="text-sm text-slate-600">Visitante / Empleado
							<input required value={borrador.visitante} onChange={(evento) => setBorrador({ ...borrador, visitante: evento.target.value })} className={inputClass} />
						</label>
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
						<button type="submit" className="rounded-lg bg-amber-500 px-4 py-2 text-sm font-semibold text-slate-950 hover:bg-amber-400">Guardar cambios</button>
					</div>
				</form>
			) : (
				<>
					<section className="grid gap-4 xl:grid-cols-2">
						<article className="rounded-xl border border-slate-200 bg-white p-5 shadow-sm sm:p-6">
							<h3 className="mb-4 border-b border-slate-100 pb-3 font-semibold text-slate-800">Información del acceso</h3>
							<dl className="space-y-4">
								{campos.map((campo) => (
									<div key={campo.etiqueta} className="flex flex-wrap items-start justify-between gap-x-4 gap-y-1 text-sm">
										<dt className="text-slate-500">{campo.etiqueta}</dt>
										<dd className="text-right font-medium text-slate-800">{campo.valor}</dd>
									</div>
								))}
							</dl>
						</article>

						<article className="rounded-xl border border-slate-200 bg-white p-5 shadow-sm sm:p-6">
							<h3 className="mb-4 font-semibold text-slate-800">Código de acceso (QR)</h3>
							{cita.qr === 'Generado' ? (
								<div className="flex flex-col items-center">
									<div className="rounded-xl border border-slate-200 p-3 shadow-sm">
										<QRCodeSVG value={cita.qrToken} size={220} level="M" includeMargin />
									</div>
									<p className="mt-3 text-sm text-slate-500">Estado: <span className="font-semibold text-emerald-600">VIGENTE</span></p>
									<p className="mt-1 text-sm text-slate-500">Acceso de un solo uso</p>
								</div>
							) : (
								<div className="flex min-h-64 flex-col items-center justify-center gap-3 rounded-lg bg-slate-50 p-5 text-center">
									<QrCode size={36} className="text-slate-400" />
									<p className="text-sm text-slate-600">Este acceso todavía no tiene un QR generado.</p>
									<button type="button" onClick={regenerarQr} className="rounded-lg bg-amber-500 px-4 py-2 text-sm font-semibold text-slate-950 hover:bg-amber-400">Generar QR</button>
								</div>
							)}
							<div className="mt-5 rounded-lg border border-amber-200 bg-amber-50 p-3 text-sm text-amber-800">
								<strong className="block">Importante</strong>
								Este QR es de un solo uso. Después de utilizarse quedará inválido automáticamente.
							</div>
						</article>
					</section>

					<section className="mt-4 overflow-hidden rounded-xl border border-slate-200 bg-white shadow-sm">
						<h3 className="border-b border-slate-100 px-5 py-4 font-semibold text-slate-800">Historial de acceso para este registro</h3>
						{cita.historial.length > 0 ? (
							<DataTable columns={columnasHistorial} rows={cita.historial} getRowKey={(registro) => String(registro.id)} />
						) : (
							<p className="px-5 py-8 text-center text-sm text-slate-500">Aún no hay registros de acceso para este código.</p>
						)}
					</section>
				</>
			)}
		</div>
	)
}
