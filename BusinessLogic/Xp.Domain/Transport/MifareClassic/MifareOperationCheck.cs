using System.Collections.Generic;
using Xp.Domain.Transport;
using Xp.Domain.Transport.MifareClassic;

namespace Xp.Application.Results
{
	public class MifareOperationCheck : IMifareRWOperation
	{
		public MifareOperationType Operation { get; set; }
		public byte Sector { get; set; }
		public byte Block { get; set; }
		public string Data { get; set; }

		#region Constructors
		public static MifareOperationCheck Create(byte sector, byte block, string data)
		{
			return new MifareOperationCheck
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
