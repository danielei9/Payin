using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PaymentGetGraphArguments : IArgumentsBase
	{
		public XpDate Since { get; set; }
		public XpDate Until { get; set; }

		#region Constructors
		public PaymentGetGraphArguments(XpDate since, XpDate until) 
		{
			Since = since;
			Until = until;
		}
		#endregion Constructors
	}
}
