using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.SmartCity.Arguments
{
	public class SentiloDataGetByProviderArguments : IArgumentsBase
	{
		public int ProviderId { get; set; }

		public XpDateTime From { get; set; }
		public XpDateTime To { get; set; }
		public int? Limit { get; set; }

		#region Constructors
		public SentiloDataGetByProviderArguments(XpDateTime from, XpDateTime to, int? limit)
		{
			From = from;
			To = to;
			Limit = limit;
		}
		#endregion Constructors
	}
}
