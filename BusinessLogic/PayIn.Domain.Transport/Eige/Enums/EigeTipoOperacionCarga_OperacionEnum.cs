namespace PayIn.Domain.Transport.Eige.Enums
{
	public enum EigeTipoOperacionCarga_OperacionEnum
	{
		Carga = 0x0,
		Recarga = 0x1,
		AmpliacionTemporal = 0x2,
		AmpliacionCantidad = 0x3,
		CambioOperador = 0x4,
		CambioZona = 0x5,
		Anulacion = 0x6,
		Canje = 0x7
	}
}
