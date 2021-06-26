using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xp.Application.Dto.Tsm.GlobalPlatform
{
	public enum ScpMode
	{
		/// <summary>
		/// used when SCP used by the card is not know (before exchanges with the card)
		/// </summary>
		SCP_UNDEFINED,
		/// Initiation mode explicit, C-MAC on modified APDU, ICV set to zero, no ICV encryption, 3 Secure Channel Keys
		SCP_01_05,
		/// <summary>
		/// Initiation mode explicit, C-MAC on modified APDU, ICV set to zero, ICV encryption, 3 Secure Channel Keys
		/// </summary>
		SCP_01_15,
		/// <summary>
		/// Explicit mode, C_MAC on modified APDU, ICV set to zero, no ICV encryption, 1 Secure Channel base key
		/// TODO: implemented, Not tested
		/// </summary>
		SCP_02_04,
		/// <summary>
		/// Explicit mode, C_MAC on modified APDU, ICV set to zero, no ICV encryption, 3 Secure Channel keys
		/// TODO: implemented, Not tested
		/// </summary>
		SCP_02_05,
		/// <summary>
		/// Implicit mode, C_MAC on unmodified APDU, ICV set to MAC over AID, no ICV encryption, 1 Secure Channel base key
		/// TODO: implemented, Not tested
		/// </summary>
		SCP_02_0A,
		/// <summary>
		///  Implicit mode, C_MAC on unmodified APDU, ICV set to MAC over AID, no ICV encryption, 3 Secure Channel keys
		///  TODO: implemented, Not tested
		/// </summary>
		SCP_02_0B,
		/// <summary>
		/// Explicit mode, C_MAC on modified APDU, ICV set to zero, ICV encryption for C_MAC, 1 Secure Channel base key
		/// TODO: implemented, Not tested
		/// </summary>
		SCP_02_14,
		/// <summary>
		/// Explicit mode, C_MAC on modified APDU, ICV set to zero, ICV encryption for C_MAC, 3 Secure Channel keys
		/// </summary>
		SCP_02_15,
		/// <summary>
		/// Implicit mode, C_MAC on unmodified APDU, ICV set to MAC over AID, ICV encryption for C_MAC session, 1 Secure Channel base key
		/// TODO: implemented, Not tested
		/// </summary>
		SCP_02_1A,
		/// <summary>
		/// Implicit mode, C_MAC on unmodified APDU, ICV set to MAC over AID, ICV encryption for C_MAC session, 3 Secure Channel keys
		/// TODO: implemented, Not tested
		/// </summary>
		SCP_02_1B,
		/// <summary>
		/// Initiation mode explicit, C-MAC on modified APDU, ICV set to zero, ICV encryption for C-MAC session, 3 Secure Channel Keys, well-known pseudo-random algorithm (card challenge)
		/// TODO: implemented, Not tested
		/// </summary>
		SCP_02_55,
		/// <summary>
		/// Initiation mode explicit, C-MAC on modified APDU, ICV set to zero, no ICV encryption, 3 Secure Channel Keys, well-known pseudo-random algorithm (card challenge)
		/// TODO: implemented, Not tested
		/// </summary>
		SCP_02_45,
		/// <summary>
		/// Initiation mode explicit, C-MAC on modified APDU, ICV set to zero, no ICV encryption, 1 Secure Channel Keys, well-known pseudo-random algorithm (card challenge)
		/// TODO: implemented, Not tested
		/// </summary>
		SCP_02_54,
		/// <summary>
		/// TODO: Not implemented
		/// </summary>
		SCP_10,
		/// <summary>
		/// TODO: implemented, Not tested
		/// </summary>
		SCP_03_65,
		/// <summary>
		/// TODO: implemented, Not tested
		/// </summary>
		SCP_03_6D,
		/// <summary>
		/// TODO: implemented, Not tested
		/// </summary>
		SCP_03_05,
		/// <summary>
		/// TODO: implemented, Not tested
		/// </summary>
		SCP_03_0D,
		/// <summary>
		/// TODO: implemented, Not tested
		/// </summary>
		SCP_03_2D,
		/// <summary>
		/// TODO: implemented, Not tested
		/// </summary>
		SCP_03_25
	}
}
