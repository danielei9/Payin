using System;

namespace PayIn.Domain.Transport.Eige.Enums
{
	[Flags]
	public enum EigeZonaEnum
	{
		A = 0x01,
		B = 0x02,
		C = 0x04,
		D = 0x08,
        E = 0x10,
        F = 0x20
	}
}
