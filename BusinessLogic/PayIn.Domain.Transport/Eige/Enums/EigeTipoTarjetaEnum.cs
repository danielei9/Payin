namespace PayIn.Domain.Transport.Eige.Enums
{
	public enum EigeTipoTarjetaEnum
	{
		Viajero = 0x1,
		ViajeroPersonalizado = 0x2,
		Inspector = 0x3,
		Empleado = 0x4,
		Pase = 0x5,
		PasePersonalizado = 0x6,
		Expendedor = 0x7,
		ValenciaCardNoPerso = 0x8, // En Codificación General y de Titulos SIGA>Punt
		ValenciaCardPerso = 0x9, // En Codificación General y de Titulos SIGA>Punt
		TuiNTemporal = 0x10, // Verificar si es este el tipo de tarjeta
		TarifaEspecial = 0xA, // En Codificación General y de Titulos SIGA>Punt
		DispotivoMovil = 0xB, // En Codificación General y de Titulos SIGA>Punt
		TarjetaCiudadana = 0xF
	}
}
