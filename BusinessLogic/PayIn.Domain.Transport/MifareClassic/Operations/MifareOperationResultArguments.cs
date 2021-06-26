using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Domain.Transport.MifareClassic.Operations
{
	public class MifareOperationResultArguments :
		IMifareReadOperationsArguments
	{
		#region Operation
		public MifareOperationType Operation { get; set; }
		#endregion Operation

		#region Sector
		public byte Sector { get; set; }
		#endregion Sector

		#region Block
		public byte Block { get; set; }
		#endregion Block

		#region Data
		public string Data { get; set; }
		#endregion Data

		#region ToString
		public override string ToString()
		{
			return base.ToString();
		}
		#endregion ToString
	}
}
