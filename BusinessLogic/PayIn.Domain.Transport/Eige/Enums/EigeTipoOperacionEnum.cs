using System;

namespace PayIn.Domain.Transport.Eige.Enums
{
	[Flags]
	public enum EigeTipoOperacionEnum
	{
		LogotiposEmpresaPropietaria = 0x800,
		Fotografias = 0x400,
		Nombre = 0x200,
		EmpresaPropietaria = 0x100,
		FondoEmpresaPropietaria = 0x080,
		CodigoUsuario = 0x040,
		CodigoIdentificacionNoViajero = 0x020,
		CaducidadTarjeta = 0x010,
		FondoTitulo = 0x008,
		ImpresionTituloPersonalizado = 0x004,
		TituloNoPersonalizado = 0x002,
		CaducidadTitulo = 0x001
	}
}
