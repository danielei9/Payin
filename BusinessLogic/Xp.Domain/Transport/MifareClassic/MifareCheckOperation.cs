namespace Xp.Domain.Transport.MifareClassic
{
	public class MifareCheckOperation : IMifareRWOperation
	{
		public MifareOperationType Operation { get; set; }
		public byte Sector { get; set; }
		public byte Block { get; set; }
		public string Data { get; set; }
		public int? From { get; set; }
		public int? To { get; set; }

		#region Constructors

		public static MifareCheckOperation Create(byte sector, byte block, string data, int? from = null, int? to = null)
		{
			return new MifareCheckOperation
			{
				Operation = MifareOperationType.Check,
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
