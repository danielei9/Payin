using System;

namespace Xp.Domain.Transport.MifareClassic
{
	public class MifareSuccessOperation : IMifareRWOperation
    {
        public MifareOperationType Operation { get; set; }
        public byte Sector { get; set; } = 0;
        public byte Block { get; set; } = 0;
        public string Data { get; set; }

        #region Constructors
        public static MifareSuccessOperation Create()
		{
			return new MifareSuccessOperation
			{
				Operation = MifareOperationType.Success
			};
		}
		#endregion Constructors
	}
}
