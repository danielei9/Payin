using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto.Tsm.Mifare4Mobile;
using Xp.Application.Hsm;
using Xp.Domain.Transport.MifareClassic;

namespace Xp.Application.Tsm.Mifare4Mobile.Commands
{
	public class M4mUpdateMifareClassicSectorCommand : M4mCommand
	{
		public IMifare4MobileHsm Hsm { get; set; }
		public byte SectorNumber { get; set; }

		public byte[] Block0 { get; set; } = null;
		public byte[] Block1 { get; set; } = null;
		public byte[] Block2 { get; set; } = null;
		public byte[] AccessConditions { get; set; } = null;

		#region Constructors
		public M4mUpdateMifareClassicSectorCommand(IMifare4MobileHsm hsm, byte sectorNumber)
			:base(M4mCommand.UpdateMifareClassicSector)
		{
			Hsm = hsm;
			SectorNumber = sectorNumber;
		}
		#endregion Constructors

		#region ToHexadecimalAsync
		public override async Task<string> ToHexadecimalAsync()
		{
#if DEBUG
			{
				var desiredPassword = "0B54570745FE3AE7".FromHexadecimal();
				var password = await Hsm.GetPasswordAsync(
					"FFFFFFFFFFFF".FromHexadecimalBE(),
					"FFFFFFFFFFFF".FromHexadecimalBE()
				);
				if (!password.SequenceEqual(desiredPassword))
					throw new ApplicationException("Se esperaba el M_Password {0} y se ha generado {1}".FormatString(desiredPassword, password));
			}
			{
				var desiredPassword = "8C7F46D76CE01266".FromHexadecimal();
				var password = await Hsm.GetPasswordAsync(
					"A0A1A2A3A4A5".FromHexadecimal(),
					"B0B1B2B3B4B5".FromHexadecimal()
				);
				if (!password.SequenceEqual(desiredPassword))
					throw new ApplicationException("Se esperaba el M_Password {0} y se ha generado {1}".FormatString(desiredPassword, password));
			}

			{
				var desiredPassword = "856FC8F8817ECCCF".FromHexadecimal();
				var password = await Hsm.GetPasswordAsync(
					"AAAAAAAAAAAA".FromHexadecimal(),
					"313131313131".FromHexadecimal()
				);
				if (!password.SequenceEqual(desiredPassword))
					throw new ApplicationException("Se esperaba el M_Password {0} y se ha generado {1}".FormatString(desiredPassword, password));
			}
#endif // DEBUG

			var blockBitmap =
				(Block0 != null ? (int)BlockBitmap.Block0 : 0) +
				(Block1 != null ? (int)BlockBitmap.Block1 : 0) +
				(Block2 != null ? (int)BlockBitmap.Block2 : 0) +
				(AccessConditions != null ? (int)BlockBitmap.Block3 : 0);

			// Si se guardsa tb la accessConditions se supone que son las claves iniciales
			var currentKeyA = AccessConditions != null ?
				await Hsm.GetKeyAsync(MifareKeyType.A) :
				await Hsm.GetKeyAsync(SectorNumber, MifareKeyType.A);
			var currentKeyB = AccessConditions != null ?
				await Hsm.GetKeyAsync(MifareKeyType.B) :
				await Hsm.GetKeyAsync(SectorNumber, MifareKeyType.B);

			var data =
				SectorNumber.ToHexadecimal() + // SectorNumber
				blockBitmap.ToHexadecimal(2) +  // BlockBitmap
				(await Hsm.GetPasswordAsync(currentKeyA, currentKeyB)).ToHexadecimal() + // Password
				Block0.ToHexadecimal() +
				Block1.ToHexadecimal() +
				Block2.ToHexadecimal() +
				(AccessConditions == null ? "" :
					(await Hsm.GetKeyAsync(SectorNumber, MifareKeyType.A)).ToHexadecimal() +
					AccessConditions.ToHexadecimal() +
					(await Hsm.GetKeyAsync(SectorNumber, MifareKeyType.B)).ToHexadecimal()
				);

			data = data.Replace(" ", "");

			var result =
				Command.ToHexadecimal(1) +
				(data.Length / 2).ToHexadecimal(1) +
				data;
			Debug.WriteLine("S << " + result);

			return result;
		}
		#endregion ToHexadecimalAsync
	}
}
