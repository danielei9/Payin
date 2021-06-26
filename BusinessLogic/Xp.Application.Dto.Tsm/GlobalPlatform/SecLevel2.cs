using System;

namespace Xp.Application.Dto.Tsm.GlobalPlatform
{
	[Flags]
	public enum SecLevel2 : byte
	{
		/// <summary>
		/// MAC Authentication of each APDU
		/// </summary>
		C_MAC = 0x01,
		/// <summary>
		/// Encryption of each APDU
		/// </summary>
		C_ENC = 0x02,
		/// <summary>
		/// MAC Authentication of each response of APDU
		/// </summary>
		R_MAC = 0x10,
		/// <summary>
		/// Encryption of each response of APDU
		/// </summary>
		R_ENC = 0x20
	}
}
