using PayIn.Domain.Payments;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobileContactScanVisitorArguments : ICreateArgumentsBase<Contact>
	{
		public int VisitorEntranceId { get; set; }

		#region Constructor
		public MobileContactScanVisitorArguments(int visitorEntranceId)
		{
			VisitorEntranceId = visitorEntranceId;
		}
		#endregion Constructor
	}
}
