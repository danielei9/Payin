using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xp.Application.Dto.Tsm.Mifare4Mobile;
using Xp.Application.Hsm;
using Xp.Domain.Transport.MifareClassic;

namespace Xp.Application.Tsm.Mifare4Mobile.Commands
{
	public class M4mReadMifareClassicSectorCommand : M4mCommand
	{
		public IMifare4MobileHsm Hsm { get; set; }
		public byte SectorNumber { get; set; }
		public BlockBitmap BlockBitmap { get; set; }

		//public byte[] AccessConditions { get; set; } = null;

		#region Constructors
		public M4mReadMifareClassicSectorCommand(IMifare4MobileHsm hsm, byte sectorNumber, BlockBitmap blockBitmap)
			:base(M4mCommand.ReadMifareClassicSector)
		{
			Hsm = hsm;
			SectorNumber = sectorNumber;
			BlockBitmap = blockBitmap;
		}
		#endregion Constructors

		#region ToHexadecimalAsync
		public override async Task<string> ToHexadecimalAsync()
		{
			var currentKeyA = await Hsm.GetKeyAsync(SectorNumber, MifareKeyType.A);
			var currentKeyB = await Hsm.GetKeyAsync(SectorNumber, MifareKeyType.B);

			var data =
				SectorNumber.ToHexadecimal() +
				((int)BlockBitmap).ToHexadecimal(2) +
				(await Hsm.GetPasswordAsync(currentKeyA, currentKeyB)).ToHexadecimal();

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
