using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xp.Application.Tsm.GlobalPlatform
{
    public class TsmError
    {
        public const int SW_NO_ERROR = 0x9000;
        public const int SW_BYTES_REMAINING_00 = 0x6100;
        public const int SW_MORE_DATA_AVAILABLE = 0x6310;
        public const int SW_WRONG_LENGTH = 0x6700;
        public const int SW_SECURITY_STATUS_NOT_SATISFIED = 0x6982;
        public const int SW_FILE_INVALID = 0x6983;
        public const int SW_DATA_INVALID = 0x6984;
        public const int SW_CONDITIONS_NOT_SATISFIED = 0x6985;
        public const int SW_COMMAND_NOT_ALLOWED = 0x6986;
        public const int SW_APPLET_SELECT_FAILED = 0x6999;
        public const int SW_WRONG_DATA = 0x6A80;
        public const int SW_FUNC_NOT_SUPPORTED = 0x6A81;
        public const int SW_FILE_NOT_FOUND = 0x6A82;
        public const int SW_RECORD_NOT_FOUND = 0x6A83;
        public const int SW_INCORRECT_P1P2 = 0x6A86;
        public const int SW_WRONG_REFERENCE_DATA_NOT_FOUND = 0x6A80;
        public const int SW_WRONG_P1P2 = 0x6B00;
        public const int SW_CORRECT_LENGTH_00 = 0x6C00;
        public const int SW_INS_NOT_SUPPORTED = 0x6D00;
        public const int SW_CLA_NOT_SUPPORTED = 0x6E00;
        public const int SW_UNKNOWN = 0x6F00;
        public const int SW_FILE_FULL = 0x6A84;

        public static byte[] CheckError(byte[] response)
        {
            var value = (response[response.Length - 2] << 8) + response[response.Length - 1];

            if (value == SW_NO_ERROR) return response.CloneArray(response.Length - 2); // Applet selection failed
            if (value == SW_BYTES_REMAINING_00) throw new ApplicationException("Response bytes remaining");
            if (value == SW_MORE_DATA_AVAILABLE) throw new ApplicationException("More Data Available");
            if (value == SW_WRONG_LENGTH) throw new ApplicationException("Wrong length");
            if (value == SW_SECURITY_STATUS_NOT_SATISFIED) throw new ApplicationException("Security condition not satisfied");
            if (value == SW_FILE_INVALID) throw new ApplicationException("File invalid");
            if (value == SW_DATA_INVALID) throw new ApplicationException("Data invalid");
            if (value == SW_CONDITIONS_NOT_SATISFIED) throw new ApplicationException("Conditions of use not satisfied");
            if (value == SW_COMMAND_NOT_ALLOWED) throw new ApplicationException("Command not allowed (no current EF)");
            if (value == SW_APPLET_SELECT_FAILED) throw new ApplicationException("Applet selection failed");
            if (value == SW_WRONG_DATA) throw new ApplicationException("Wrong data");
            if (value == SW_FUNC_NOT_SUPPORTED) throw new ApplicationException("File not found");
            if (value == SW_FILE_NOT_FOUND) throw new ApplicationException("Record not found");
            if (value == SW_RECORD_NOT_FOUND) throw new ApplicationException("Record not found");
            if (value == SW_INCORRECT_P1P2) throw new ApplicationException("Incorrect parameters (P1,P2)");
            if (value == SW_WRONG_REFERENCE_DATA_NOT_FOUND) throw new ApplicationException("Reference Data Not Found");
            if (value == SW_WRONG_P1P2) throw new ApplicationException("Incorrect parameters (P1,P2)");
            if (value == SW_CORRECT_LENGTH_00) throw new ApplicationException("Correct Expected Length (Le)");
            if (value == SW_INS_NOT_SUPPORTED) throw new ApplicationException("INS value not supported");
            if (value == SW_CLA_NOT_SUPPORTED) throw new ApplicationException("CLA value not supported");
            if (value == SW_UNKNOWN) throw new ApplicationException("No precise diagnosis");
            if (value == SW_FILE_FULL) throw new ApplicationException("Not enough memory space in the file");
			
			return null;
		}
    }
}
