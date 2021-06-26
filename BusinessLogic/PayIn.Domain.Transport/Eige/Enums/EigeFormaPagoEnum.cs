namespace PayIn.Domain.Transport.Eige.Enums
{
	public enum EigeFormaPagoEnum
	{
		Efectivo = 0x0,
		TarjetaDebito = 0x1,
		TarjetaCredito = 0x2,
		Cheque = 0x3,
		MonederoExterno = 0x4,
		Movil = 0x5,
		MonederoTarjeta = 0x6,
		Bonus = 0x7,
		Domiciliacion = 0x8,
		Internet = 0x9,
		PagosMixto = 0xA
	}
}
