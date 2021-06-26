namespace Xp.Domain.Transport.MifareClassic
{
	public class MifareSumOperation : IMifareRWOperation
	{
		public MifareOperationType Operation { get; set; }
		public byte Sector { get; set; }
		public byte Block { get; set; }
		public string Data { get; set; }
		public int? From { get; set; }
		public int? To { get; set; }

		#region Constructors

		public static MifareSumOperation Create(byte sector, byte block, string data, int? from = null, int? to = null)
		{
			return new MifareSumOperation
			{
				Operation = MifareOperationType.Sum,
				Sector = sector,
				Block = block,
				Data = data,
				From = from,
				To = to
			};
		}
		#endregion Constructors
	}
}
