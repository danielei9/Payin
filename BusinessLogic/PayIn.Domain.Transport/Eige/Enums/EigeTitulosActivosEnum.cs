using System;

namespace PayIn.Domain.Transport.Eige.Enums
{
	[Flags]
	public enum EigeTitulosActivosEnum
	{
		Titulo1 = 0x1,
		Titulo2 = 0x2,
		Monedero = 0x4,
		Bonus = 0x8
	}
}
