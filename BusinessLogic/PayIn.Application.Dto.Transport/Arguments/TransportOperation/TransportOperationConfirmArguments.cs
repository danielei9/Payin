using System;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportOperation
{
	public class TransportOperationConfirmArguments : IArgumentsBase
    {
		public int Id { get; set; }
		public string MobileSerial { get; set; }
		public DateTime Now { get; set; }

		#region Constructors
		public TransportOperationConfirmArguments(
			int id,
			string mobileSerial
#if DEBUG || TEST || HOMO
			, DateTime? now
#endif
		)
		{
            Id = id;
			MobileSerial = mobileSerial;
#if DEBUG || TEST || HOMO
			Now = now.ToUTC() ?? DateTime.Now.ToUTC();
#else
			Now = DateTime.Now.ToUTC();
#endif
		}
		#endregion Constructors
	}
}
