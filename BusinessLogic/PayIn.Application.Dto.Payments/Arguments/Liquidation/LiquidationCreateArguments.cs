using PayIn.Application.Dto.Payments.Arguments.Payment;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class LiquidationCreateArguments : AccountLineGetAllUnliquidatedArguments
	{
		#region Constructures
		public LiquidationCreateArguments(string filter, XpDate since, XpDate until, bool filterByEvent, int? concessionId, int? eventId)
			: base(filter, since, until, filterByEvent, concessionId, eventId)
		{
		}
		#endregion Constructures
	}
}
