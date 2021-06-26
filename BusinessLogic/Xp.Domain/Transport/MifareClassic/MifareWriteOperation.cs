namespace Xp.Domain.Transport.MifareClassic
{
	public class MifareWriteOperation : IMifareRWOperation
	{
		public MifareOperationType Operation { get; set; }
		public byte Sector { get; set; }
		public byte Block { get; set; }
		public string Data { get; set; }
		public int? From { get; set; }
		public int? To { get; set; }

		#region Constructors

		public static MifareWriteOperation Create(byte sector, byte block, string data, int? from = null, int? to = null)
		{
			return new MifareWriteOperation
			{
				Operation = MifareOperationType.WriteBlock,
				Sector = sector,
				Block = block,
				Data = data,
				From = from,
				To = to
			};
		}

		public static MifareWriteOperation Check(byte sector, byte block, string data)
		{
			return new MifareWriteOperation
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
