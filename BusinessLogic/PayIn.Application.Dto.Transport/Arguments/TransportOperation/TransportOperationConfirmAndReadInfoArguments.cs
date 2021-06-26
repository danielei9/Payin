using PayIn.Domain.Transport.MifareClassic.Operations;
using System;
using Xp.Common.Dto.Arguments;
using Xp.Domain.Transport;

namespace PayIn.Application.Dto.Transport.Arguments.TransportOperation
{
	public class TransportOperationConfirmAndReadInfoArguments : TransportOperationReadInfoArguments, IArgumentsBase
	{
		public string MobileSerial { get; set; }

		#region Constructors
		public TransportOperationConfirmAndReadInfoArguments(
			string cardId,
            CardSystem? cardSystem,
            MifareOperationResultArguments[] script,
			string mobileSerial
#if DEBUG || TEST || HOMO
			, DateTime? now = null
#endif
			)
			: base(
				cardId,
                cardSystem,
				script
#if DEBUG || TEST || HOMO
				, now
#endif
			)
		{
			MobileSerial = mobileSerial;
		}
		#endregion Constructors
	}
}
