using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PaymentGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		public XpDate Since { get; set; }
		public XpDate Until { get; set; }

		#region Constructors
		public PaymentGetAllArguments(string filter, XpDate since, XpDate until)
		{
			Filter = filter ?? "";
			Since = since;
			Until = until;
		}
		#endregion Constructors
	}
}
