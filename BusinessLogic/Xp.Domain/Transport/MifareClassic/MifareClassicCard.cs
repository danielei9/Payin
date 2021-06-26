using System.Threading.Tasks;

namespace Xp.Domain.Transport.MifareClassic
{
	public abstract class MifareClassicCard : MifareClassicElement
	{
		#region Hsm
		public IMifareClassicHsmService Hsm { get; set; }
		#endregion Hsm

		#region Name
		public string Name { get; set; }
		#endregion Name

		#region Uid
		public byte[] Uid { get; set; }
		#endregion Uid

		#region Sectors
		public MifareClassicSector[] Sectors { get; set; }
		#endregion Sectors

		#region Constructors
		public MifareClassicCard(IMifareClassicHsmService hsm)
			: base(null)
		{
			Card = this;
			Hsm = hsm;

			Sectors = new MifareClassicSector[16];
			for (var i = 0; i < 16; i++)
				Sectors[i] = new MifareClassicSector(this, i);
		}
		#endregion Constructors

		#region CheckAsync
		public async Task<bool> CheckAsync(long uid)
		{
			foreach (var sector in Sectors)
				foreach (var block in sector.Blocks)
					if (!(await CheckAsync(uid, block)))
						return false;
			return true;
		}
		public abstract Task<bool> CheckAsync(long uid, MifareClassicBlock block);
		#endregion CheckAsync

		#region GetValueWithCrc
		public async Task<byte[]> GetValueWithCrcAsync(long uid, byte sector, byte block)
		{
			return await GetValueWithCrcAsync(uid, Sectors[sector].Blocks[block]);
		}
		public abstract Task<byte[]> GetValueWithCrcAsync(long uid, MifareClassicBlock block);
		#endregion GetValueWithCrc

		#region SetCrc
		public virtual void SetCrc(byte sector, byte block)
		{
		}
		#endregion SetCrc

		#region CheckImportantAsync
		public abstract Task<bool> CheckImportantAsync(MifareClassicBlock block);
		#endregion CheckImportantAsync

		#region BlockBackup
		public virtual void BlockBackup(byte sectorNumber, byte blockNumber)
		{
		}
		#endregion BlockBackup
	}
}
