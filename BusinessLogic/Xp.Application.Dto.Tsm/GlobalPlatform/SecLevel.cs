namespace Xp.Application.Dto.Tsm.GlobalPlatform
{
	public enum SecLevel : byte
	{
		/// <summary>
		/// No encryption or authentication
		/// </summary>
		NO_SECURITY_LEVEL = 0x00,
		/// <summary>
		/// MAC Authentication of each APDU
		/// </summary>
		C_MAC = 0x01,
		/// <summary>
		/// Encryption and MAC authentication of each APDU
		/// </summary>
		C_ENC_AND_MAC = 0x03,
		/// <summary>
		/// implemented, Not tested
		/// </summary>
		R_MAC = 0x10,
		/// <summary>
		/// implemented, Not tested
		/// </summary>
		C_MAC_AND_R_MAC = 0x11,
		/// <summary>
		/// implemented, Not tested
		/// </summary>
		C_ENC_AND_C_MAC_AND_R_MAC = 0x13,
		/// <summary>
		/// implemented, Not tested
		/// </summary>
		C_ENC_AND_R_ENC_AND_C_MAC_AND_R_MAC = 0x33,
		
		//C_MAC = 0x01,
		//C_ENC = 0x02,
		//R_MAC = 0x10,
		//R_ENC = 0x20,
	}
}
