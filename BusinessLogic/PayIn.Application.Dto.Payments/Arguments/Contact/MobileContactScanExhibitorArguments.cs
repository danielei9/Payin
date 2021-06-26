using PayIn.Domain.Payments;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobileContactScanExhibitorArguments : ICreateArgumentsBase<Contact>
	{
		public int ExhibitorId { get; set; }
		public int EventId { get; set; }

		#region Constructor
		public MobileContactScanExhibitorArguments(int exhibitorId, int eventId)
		{
			ExhibitorId = exhibitorId;
			EventId = eventId;
		}
		#endregion Constructor
	}
}
