namespace Xp.Domain.Transport.MifareClassic
{
	public class MifareReadOperation : IMifareRWOperation
	{
		public MifareOperationType Operation { get; set; }
		public byte Sector { get; set; }
		public byte Block { get; set; }
		public string Data { get; set; }

		#region Constructors
		public static MifareReadOperation Create(byte sector, byte block)
		{
			return new MifareReadOperation
			{
				Operation = MifareOperationType.ReadBlock,
				Sector = sector,
				Block = block
			};
		}

		public static MifareReadOperation Check(byte sector, byte block, string data)
		{
			return new MifareReadOperation
			{
				Operation = MifareOperationType.Check,
				Sector = sector,
				Block = block,
				Data = data
			};
		}
		#endregion Constructors
	}
}
