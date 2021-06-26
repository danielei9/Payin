using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xp.Application.Tsm.GlobalPlatform.Commands
{
	/// <summary>
	/// APDU Offsets
	/// </summary>
	public enum ISO7816
	{
		/// <summary>
		/// APDU command CLA : ISO 7816 = 0x00
		/// </summary>
		CLA_ISO7816 = 0x00,
		/// <summary>
		/// APDU command INS : EXTERNAL AUTHENTICATE = 0x82
		/// </summary>
		INS_EXTERNAL_AUTHENTICATE = 0x82,
		/// <summary>
		/// APDU command INS : SELECT = 0xA4
		/// </summary>
		INS_SELECT = 0xA4,
		/// <summary>
		/// APDU header offset : CLA = 0
		/// </summary>
		OFFSET_CLA = 0x00,
		/// <summary>
		///  APDU header offset : INS = 1
		/// </summary>
		OFFSET_INS = 0x01,
		/// <summary>
		/// APDU header offset : P1 = 2
		/// </summary>
		OFFSET_P1 = 0x02,
		/// <summary>
		/// APDU header offset : P1 = 3
		/// </summary>
		OFFSET_P2 = 0x03,
		/// <summary>
		/// APDU header offset : LC = 4
		/// </summary>
		OFFSET_LC = 0x04,
		/// <summary>
		/// APDU command data offset : CDATA = 5
		/// </summary>
		OFFSET_CDATA = 0x05,
		/// <summary>
		/// Response status : Applet selection failed
		/// </summary>
		SW_NO_ERROR = 0x9000,
		/// <summary>
		/// Response status : Response bytes remaining
		/// </summary>
		SW_BYTES_REMAINING_00 = 0x6100,
		/// <summary>
		/// Response status: More Data Available
		/// </summary>
		SW_MORE_DATA_AVAILABLE = 0x6310,
		/// <summary>
		/// Response status: Wrong length
		/// </summary>
		SW_WRONG_LENGTH = 0x6700,
		/// <summary>
		/// Response status: Security condition not satisfied
		/// </summary>
		SW_SECURITY_STATUS_NOT_SATISFIED = 0x6982,
		/// <summary>
		/// Response status: File invalid
		/// </summary>
		SW_FILE_INVALID = 0x6983,
		/// <summary>
		/// Response status: Data invalid
		/// </summary>
		SW_DATA_INVALID = 0x6984,
		/// <summary>
		/// Response status: Conditions of use not satisfied
		/// </summary>
		SW_CONDITIONS_NOT_SATISFIED = 0x6985,
		/// <summary>
		/// Response status: Command not allowed (no current EF)
		/// </summary>
		SW_COMMAND_NOT_ALLOWED = 0x6986,
		/// <summary>
		/// Response status: Applet selection failed
		/// </summary>
		SW_APPLET_SELECT_FAILED = 0x6999,
		/// <summary>
		/// Response status: Wrong data
		/// </summary>
		SW_WRONG_DATA = 0x6A80,
		/// <summary>
		/// Response status: File not found
		/// </summary>
		SW_FUNC_NOT_SUPPORTED = 0x6A81,
		/// <summary>
		/// Response status: Record not found
		/// </summary>
		SW_FILE_NOT_FOUND = 0x6A82,
		/// <summary>
		/// Response status: Record not found
		/// </summary>
		SW_RECORD_NOT_FOUND = 0x6A83,
		/// <summary>
		/// Response status: Incorrect parameters (P1,P2)
		/// </summary>
		SW_INCORRECT_P1P2 = 0x6A86,
		/// <summary>
		/// Response status: Reference Data Not Found
		/// </summary>
		SW_WRONG_REFERENCE_DATA_NOT_FOUND = 0x6A80,
		/// <summary>
		/// Response status: Incorrect parameters (P1,P2)
		/// </summary>
		SW_WRONG_P1P2 = 0x6B00,
		/// <summary>
		/// Response status: Correct Expected Length (Le)
		/// </summary>
		SW_CORRECT_LENGTH_00 = 0x6C00,
		/// <summary>
		/// Response status: INS value not supported
		/// </summary>
		SW_INS_NOT_SUPPORTED = 0x6D00,
		/// <summary>
		/// Response status: CLA value not supported
		/// </summary>
		SW_CLA_NOT_SUPPORTED = 0x6E00,
		/// <summary>
		/// Response status: No precise diagnosis
		/// </summary>
		SW_UNKNOWN = 0x6F00,
		/// <summary>
		/// Response status: Not enough memory space in the file
		/// </summary>
		SW_FILE_FULL = 0x6A84

		/*
		private int value;
		private ISO7816(int value)
		{
			if ((value & 0xFF00) != 0)
			{
				this.value = value & 0xFFFF;
			}
			else
			{
				this.value = value & 0x00FF;
			}
		}
		public int getValue()
		{
			return this.value;
		}
		*/
	}
}
