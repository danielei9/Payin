using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xp.Common;

namespace Xp.Application.Tsm.Mifare4Mobile.Commands
{
	public class M4mAddAndUpdateMdacCommand : M4mCommand
	{
		#region Constructors
		public M4mAddAndUpdateMdacCommand()
			:base(M4mCommand.AddAndUpdateMdacCommand)
		{
		}
		#endregion Constructors

		#region GetPassword
		public byte[] GetPassword()
		{
			var keyA =
				//"FFFFFFFFFFFF".FromHexadecimal();
				"AAAAAAAAAAAA".FromHexadecimal();
			var keyB =
				//"FFFFFFFFFFFF".FromHexadecimal();
				"313131313131".FromHexadecimal();
			//var accessConditions = "08778F00";

			return GetPassword(keyA, keyB);
		}
		public byte[] GetPassword(byte[] keyA, byte[] keyB)
		{
			// Derive key B
			var dKeyB = new byte[8];
			dKeyB[7] = (byte)((keyB[5] << 1) & 0xFF);
			dKeyB[6] = (byte)((keyB[4] << 1) & 0xFF);
			dKeyB[5] = (byte)((keyB[3] << 1) & 0xFF);
			dKeyB[4] = (byte)((keyB[2] << 1) & 0xFF);
			dKeyB[3] = (byte)((keyB[1] << 1) & 0xFF);
			dKeyB[2] = (byte)((keyB[0] << 1) & 0xFF);
			dKeyB[1] = (byte)(
				((keyB[5] & 0x80) >> 1) |
				((keyB[4] & 0x80) >> 2) |
				((keyB[3] & 0x80) >> 3) |
				((keyB[2] & 0x80) >> 4) |
				((keyB[1] & 0x80) >> 5) |
				((keyB[0] & 0x80) >> 6)
			);
			dKeyB[0] = 0x00; // 00 7E FE FE FE FE FE FE

			// Derive key A
			var dKeyA = new byte[8];
			dKeyA[0] = (byte)((keyA[0] << 1) & 0xFF);
			dKeyA[1] = (byte)((keyA[1] << 1) & 0xFF);
			dKeyA[2] = (byte)((keyA[2] << 1) & 0xFF);
			dKeyA[3] = (byte)((keyA[3] << 1) & 0xFF);
			dKeyA[4] = (byte)((keyA[4] << 1) & 0xFF);
			dKeyA[5] = (byte)((keyA[5] << 1) & 0xFF);
			dKeyA[6] = (byte)(
				((keyA[0] & 0x80) >> 1) |
				((keyA[1] & 0x80) >> 2) |
				((keyA[2] & 0x80) >> 3) |
				((keyA[3] & 0x80) >> 4) |
				((keyA[4] & 0x80) >> 5) |
				((keyA[5] & 0x80) >> 6)
			);
			dKeyA[7] = 0x00; // FE FE FE FE FE FE 7E 00

			// Join keys
			var dKey = new byte[16];
			dKey.Copy(dKeyB);
			dKey.Copy(dKeyA, targetOffset: 8);

			var password = XpSecurity.Cypher_DESede_ECB_NoPadding(dKey.Reverse(), new byte[8]).Reverse(); // 0B 54 57 07 45 FE 3A E7
			return password;
		}
		#endregion GetPassword

		#region ToHexadecimalAsync
		public override async Task<string> ToHexadecimalAsync()
		{
			return await Task.Run(() =>
			{
				var data = (
				    "D0 02" + // MDAC UNIQUE IDENTIFIER
				    "00 01" +
				    "D2 08" + // Password
					    GetPassword().ToHexadecimal() +
				    "D3 10" + // CMAC
					    "C3 13 8C D8 42 9E 2A 1E 36 C9 05 67 E6 82 DC 62"
			    )
			    .Replace(" ", "");

				var result =
					Command.ToHexadecimal(1) +
					(data.Length / 2).ToHexadecimal(1) +
					data;
				Debug.WriteLine("S << " + result);

				return result;
			});
		}
		#endregion ToHexadecimalAsync
	}
}
