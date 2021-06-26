using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xp.Application.Dto.Tsm.GlobalPlatform;
using Xp.Application.Dto.Tsm.GlobalPlatform.States;
using Xp.Application.Tsm.GlobalPlatform.Commands;

namespace System
{
	public static class ByteExtension
	{
		public class Response
		{
			public byte Command { get; set; }
			public Func<byte[], byte[]> Action { get; set; }
		}

		#region GP_ReadResponse
		public static byte[] GP_ReadResponse(this byte[] data, byte tag1, byte tag2, Func<byte[], byte[]> action)
		{
			if ((data.Length > 2) && (data[0] == tag1) && (data[1] == tag2))
			{
				var result = action(data.CloneArray(data[2], 3));
				if (result.Length > 0)
					throw new ApplicationException("Response interpretation not compleated " + result.ToHexadecimal());
				return data.Skip(data[2] + 3).ToArray();
			}
			else
				return data;
		}
		public static byte[] GP_ReadResponse(this byte[] data, byte tag1, byte tag2, Action<byte[]> action)
		{
			if ((data.Length > 2) && (data[0] == tag1) && (data[1] == tag2))
			{
				action(data.CloneArray(data[2], 3));
				return data.Skip(data[2] + 3).ToArray();
			}
			else
				return data;
		}
		public static byte[] GP_ReadResponse(this byte[] data, byte tag1, byte tag2, Func<byte[]> action)
		{
			if ((data.Length == 2) && (data[0] == tag1) && (data[1] == tag2))
			{
				var result = action();
				if (result.Length > 0)
					throw new ApplicationException("Response interpretation not compleated " + result.ToHexadecimal());
				return data.Skip(2).ToArray();
			}
			else
				return data;
		}
		public static byte[] GP_ReadResponse(this byte[] data, byte tag1, byte tag2, Action action)
		{
			if ((data.Length == 2) && (data[0] == tag1) && (data[1] == tag2))
			{
				action();
				return data.Skip(2).ToArray();
			}
			else
				return data;
		}
		public static byte[] GP_ReadResponse(this byte[] data, byte tag1, byte tag2)
		{
			if ((data.Length == 2) && (data[0] == tag1) && (data[1] == tag2))
				return data.Skip(2).ToArray();
			else
				return data;
		}

		public static byte[] GP_ReadResponse(this byte[] data, byte tag, Func<byte[], byte[]> action)
		{
			if ((data.Length > 1) && (data[0] == tag))
			{
				var result = action(data.CloneArray(data[1], 2));
				if (result.Length > 0)
					throw new ApplicationException("Response interpretation not compleated " + result.ToHexadecimal());
				return data.Skip(data[1] + 2).ToArray();
			}
			else
				return data;
		}
		public static byte[] GP_ReadResponse(this byte[] data, byte tag, Action<byte[]> action)
		{
			if ((data.Length > 1) && (data[0] == tag))
			{
				action(data.CloneArray(data[1], 2));
				return data.Skip(data[1] + 2).ToArray();
			}
			else
				return data;
		}
		public static byte[] GP_ReadResponse(this byte[] data, byte tag, Func<byte[]> action)
		{
			if ((data.Length == 1) && (data[0] == tag))
			{
				var result = action();
				if (result.Length > 0)
					throw new ApplicationException("Response interpretation not compleated " + result.ToHexadecimal());
				return data.Skip(1).ToArray();
			}
			else
				return data;
		}
		public static byte[] GP_ReadResponse(this byte[] data, byte tag, Action action)
		{
			if ((data.Length == 1) && (data[0] == tag))
			{
				action();
				return data.Skip(1).ToArray();
			}
			else
				return data;
		}
		public static byte[] GP_ReadResponse(this byte[] data, byte tag)
		{
			if ((data.Length == 1) && (data[0] == tag))
				return data.Skip(1).ToArray();
			else
				return data;
		}
		#endregion GP_ReadResponse

		#region M4M_ReadResponse
		public static byte[] M4M_ReadResponse(this byte[] data, byte tag1, byte tag2, Func<byte[], byte[]> action)
		{
			if ((data.Length > 2) && (data[0] == tag1) && (data[1] == tag2))
			{
				var result = action(data.CloneArray(data[2], 3));
				if (result.Length > 0)
					throw new ApplicationException("Response interpretation not compleated " + result.ToHexadecimal());
				return data.Skip(data[2] + 3).ToArray();
			}
			else
				return data;
		}
		public static byte[] M4M_ReadResponse(this byte[] data, byte tag1, byte tag2, Action<byte[]> action)
		{
			if ((data.Length > 2) && (data[0] == tag1) && (data[1] == tag2))
			{
				action(data.CloneArray(data[2], 3));
				return data.Skip(data[2] + 3).ToArray();
			}
			else
				return data;
		}
		public static byte[] M4M_ReadResponse(this byte[] data, byte tag1, byte tag2, Func<byte[]> action)
		{
			if ((data.Length == 2) && (data[0] == tag1) && (data[1] == tag2))
			{
				var result = action();
				if (result.Length > 0)
					throw new ApplicationException("Response interpretation not compleated " + result.ToHexadecimal());
				return data.Skip(2).ToArray();
			}
			else
				return data;
		}
		public static byte[] M4M_ReadResponse(this byte[] data, byte tag1, byte tag2, Action action)
		{
			if ((data.Length == 2) && (data[0] == tag1) && (data[1] == tag2))
			{
				action();
				return data.Skip(2).ToArray();
			}
			else
				return data;
		}
		public static byte[] M4M_ReadResponse(this byte[] data, byte tag1, byte tag2)
		{
			if ((data.Length == 2) && (data[0] == tag1) && (data[1] == tag2))
				return data.Skip(2).ToArray();
			else
				return data;
		}

		public static byte[] M4M_ReadResponse(this byte[] data, byte tag, Func<byte[], byte[]> action)
		{
			if ((data.Length > 1) && (data[0] == tag))
			{
				var result = action(data.CloneArray(data[1], 2));
				if (result.Length > 0)
					throw new ApplicationException("Response interpretation not compleated " + result.ToHexadecimal());
				return data.Skip(data[1] + 2).ToArray();
			}
			else
				return data;
		}
		public static byte[] M4M_ReadResponse(this byte[] data, byte tag, Action<byte[]> action)
		{
			if ((data.Length > 1) && (data[0] == tag))
			{
				action(data.CloneArray(data[1], 2));
				return data.Skip(data[1] + 2).ToArray();
			}
			else
				return data;
		}
		public static byte[] M4M_ReadResponse(this byte[] data, byte tag, Func<byte[]> action)
		{
			if ((data.Length == 1) && (data[0] == tag))
			{
				var result = action();
				if (result.Length > 0)
					throw new ApplicationException("Response interpretation not compleated " + result.ToHexadecimal());
				return data.Skip(1).ToArray();
			}
			else
				return data;
		}
		public static byte[] M4M_ReadResponse(this byte[] data, byte tag, Action action)
		{
			if ((data.Length == 1) && (data[0] == tag))
			{
				action();
				return data.Skip(1).ToArray();
			}
			else
				return data;
		}
		public static byte[] M4M_ReadResponse(this byte[] data, byte tag)
		{
			if ((data.Length == 1) && (data[0] == tag))
				return data.Skip(1).ToArray();
			else
				return data;
		}
		#endregion M4M_ReadResponse

		#region GP_CheckWarning
		public static byte[] GP_CheckWarning(this byte[] data, byte sw1, byte sw2, string text)
		{
			if ((data.Length == 2) && (data[0] == sw1) && (data[1] == sw2))
			{
				Debug.WriteLine("GP Warning: " + text);
				return data.Skip(2).ToArray();
			}
			else
				return data;
		}
		#endregion GP_CheckWarning

		#region GP_CheckError
		public static byte[] GP_CheckError(this byte[] data, byte sw1, byte sw2, string text)
		{
			if ((data.Length == 2) && (data[0] == sw1) && (data[1] == sw2))
				throw new ApplicationException("GP Error: " + text);
			else
				return data;
		}
		#endregion GP_CheckError

		#region M4M_CheckError
		public static byte[] M4M_CheckError(this byte[] data, byte sw1, byte sw2, string text)
		{
			if ((data.Length == 3) && (data[0] == 0x02) && (data[1] == sw1) && (data[2] == sw2))
				throw new ApplicationException("M4M Error: " + text);
			else if ((data.Length == 2) && (data[0] == sw1) && (data[1] == sw2))
				throw new ApplicationException("M4M Error: " + text);
			else
				return data;
		}
		#endregion M4M_CheckError

		#region GP_CheckGeneralErrors
		public static byte[] GP_CheckGeneralErrors(this byte[] data)
		{
			if ((data.Length == 2) && (data[0] == 0x64) && (data[1] == 0x00))
				throw new ApplicationException("GP Error: No specific diagnosis");
			if ((data.Length == 2) && (data[0] == 0x67) && (data[1] == 0x00))
				throw new ApplicationException("GP Error: Wrong length in Lc");
			if ((data.Length == 2) && (data[0] == 0x68) && (data[1] == 0x81))
				throw new ApplicationException("GP Error: Logical channel not supported or is not active");
			if ((data.Length == 2) && (data[0] == 0x69) && (data[1] == 0x82))
				throw new ApplicationException("GP Error: Security status not satisfied");
			if ((data.Length == 2) && (data[0] == 0x69) && (data[1] == 0x85))
				throw new ApplicationException("GP Error: Conditions of use not satisfied");
			if ((data.Length == 2) && (data[0] == 0x6A) && (data[1] == 0x86))
				throw new ApplicationException("GP Error: Incorrect P1 P2");
			if ((data.Length == 2) && (data[0] == 0x6D) && (data[1] == 0x00))
				throw new ApplicationException("GP Error: Invalid instruction");
			if ((data.Length == 2) && (data[0] == 0x6E) && (data[1] == 0x00))
				throw new ApplicationException("GP Error: Invalid class");

			return data;
		}
		#endregion GP_CheckGeneralErrors

		#region M4M_CheckGeneralErrors
		public static byte[] M4M_CheckGeneralErrors(this byte[] data)
		{
			if ((data.Length == 2) && (data[0] == 0x63) && (data[1] == 0x10))
				throw new ApplicationException("M4M Error: More data available");
			if ((data.Length == 2) && (data[0] == 0x69) && (data[1] == 0x82))
				throw new ApplicationException("M4M Error: Security status not satisfied");
			if ((data.Length == 2) && (data[0] == 0x69) && (data[1] == 0x84))
				throw new ApplicationException("M4M Error: Reference data not usable");
			if ((data.Length == 2) && (data[0] == 0x69) && (data[1] == 0x85))
				throw new ApplicationException("M4M Error: Command cannot be executed in the current state of the VCM/SM");
			if ((data.Length == 2) && (data[0] == 0x64) && (data[1] == 0x00))
				throw new ApplicationException("M4M Error: Execution error");
			if ((data.Length == 2) && (data[0] == 0x6A) && (data[1] == 0x82))
				throw new ApplicationException("M4M Error: File or Application not supported");
			if ((data.Length == 2) && (data[0] == 0x6A) && (data[1] == 0x84))
				throw new ApplicationException("M4M Error: Not enough memory space");
			if ((data.Length == 2) && (data[0] == 0x6A) && (data[1] == 0x86))
				throw new ApplicationException("M4M Error: Incorrect parameter value");
			if ((data.Length == 2) && (data[0] == 0x6D) && (data[1] == 0x00))
				throw new ApplicationException("M4M Error: M4M command not supported");
			if ((data.Length == 2) && (data[0] == 0x65) && (data[1] == 0xF1))
				throw new ApplicationException("M4M Error: The MIFARE command has been rejected");
			if ((data.Length == 2) && (data[0] == 0x65) && (data[1] == 0xF2))
				throw new ApplicationException("M4M Error: Wrong password");
			if ((data.Length == 2) && (data[0] == 0x65) && (data[1] == 0xF3))
				throw new ApplicationException("M4M Error: The type of the VC does not allow the execution of the command");
			if ((data.Length == 2) && (data[0] == 0x65) && (data[1] == 0xF5))
				throw new ApplicationException("M4M Error: The option is not supported");
			if ((data.Length == 2) && (data[0] == 0x63) && (data[1] == 0xE6))
				throw new ApplicationException("M4M Error: There is no memory space in the MIFARE Implementation or the targeted VC entry number is already in used");
			if ((data.Length == 2) && (data[0] == 0x63) && (data[1] == 0xD4))
				throw new ApplicationException("M4M Error: Error in passed parameter(s)");
			if ((data.Length == 2) && (data[0] == 0x63) && (data[1] == 0xE1))
				throw new ApplicationException("M4M Error: No Virtual Card has been created in the given VC entry or the given VC entry is out of the range supported by the MIFARE Implementation");
			if ((data.Length == 2) && (data[0] == 0x63) && (data[1] == 0xE2))
				throw new ApplicationException("M4M Error: The given command cannot be executed on the targeted VC entry");
			if ((data.Length == 2) && (data[0] == 0x63) && (data[1] == 0xE3))
				throw new ApplicationException("M4M Error: The Virtual Card is swapped out of the MIFARE Implementation The state of the Virtual Card is SwappedOut_Enabled or SwappedOut_Disabled. The VC states are described in [VC Management]");
			if ((data.Length == 2) && (data[0] == 0x63) && (data[1] == 0xEC))
				throw new ApplicationException("M4M Error: The given command cannot be executed because the swap counter of the targeted VC entry reached its maximum");
			if ((data.Length == 2) && (data[0] == 0x63) && (data[1] == 0xE5))
				throw new ApplicationException("M4M Error: The given emulation type or size is not supported by this implementation");
			if ((data.Length == 2) && (data[0] == 0x63) && (data[1] == 0xE7))
				throw new ApplicationException("M4M Error: The given MasterUID is incorrect");
			if ((data.Length == 2) && (data[0] == 0x63) && (data[1] == 0xE8))
				throw new ApplicationException("M4M Error: The given VCUID is not supported or incorrect");
			if ((data.Length == 2) && (data[0] == 0x63) && (data[1] == 0xD3))
				throw new ApplicationException("M4M Error: An ATS is passed while this is not supported by the targeted ImplementationType (in case of MIFARE Classic)");
			if ((data.Length == 2) && (data[0] == 0x63) && (data[1] == 0xE9))
				throw new ApplicationException("M4M Error: The given command is not allowed due to invalid MAC");
			if ((data.Length == 2) && (data[0] == 0x63) && (data[1] == 0xEA))
				throw new ApplicationException("M4M Error: The given command is not allowed as it is not covered by the wholesale license");
			if ((data.Length == 2) && (data[0] == 0x63) && (data[1] == 0xF3))
				throw new ApplicationException("M4M Error: A contactless session is on going. The rules about the CONTACTLESS_SESSION are defined in [VC Management]");
			if ((data.Length == 2) && (data[0] == 0x63) && (data[1] == 0xCA))
				throw new ApplicationException("M4M Error: General error further detailed in the commands");

			return data;
		}
		#endregion M4M_CheckGeneralErrors

		#region GP_CheckSuccess
		public static void GP_CheckSuccess(this byte[] data)
		{
			if (
				(data.Length == 2) &&
				(data[data.Length - 2] == 0x90) &&
				(data[data.Length - 1] == 0x00)
			)
				return;
			else
				throw new ApplicationException("GP Error: No success code " + data.ToHexadecimal());
		}
		public static byte[] GP_CheckSuccess(this byte[] data, bool finishWithEmptyString)
		{
			if (finishWithEmptyString)
			{
				GP_CheckSuccess(data);
				return new byte[0];
			}
			else if (
				(data.Length >= 2) &&
				(data[data.Length - 2] == 0x90) &&
				(data[data.Length - 1] == 0x00)
			)
				return data.Take(data.Length - 2).ToArray();
			else
				throw new ApplicationException("GP Error: No success code " + data.ToHexadecimal());
		}
		#endregion GP_CheckSuccess

		#region M4M_CheckSuccess
		public static void M4M_CheckSuccess(this byte[] data)
		{
			if (
				(data.Length == 2) &&
				(data[data.Length - 2] == 0x90) &&
				(data[data.Length - 1] == 0x00)
			)
				return;
			else
				throw new ApplicationException("GP Error: No success code " + data.ToHexadecimal());
		}
		public static byte[] M4M_CheckSuccess(this byte[] data, bool finishWithEmptyString)
		{
			if (finishWithEmptyString)
			{
				M4M_CheckSuccess(data);
				return new byte[0];
			}
			else if (
				(data.Length >= 2) &&
				(data[0] == 0x90) &&
				(data[1] == 0x00)
			)
				return data.Skip(2).ToArray();
			else
				throw new ApplicationException("GP Error: No success code " + data.ToHexadecimal());
		}
		#endregion M4M_CheckSuccess

		#region GP_CheckMac
		public static byte[] GP_CheckMac(this byte[] data, TsmState state, SecLevel secLevel, ScpMode scp)
		{
			var result = ApduCommand.Decrypt(data, state, secLevel, scp);
			return result;
		}
		#endregion GP_CheckMac
	}
}
