using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class LiquidationGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		public XpDateTime Since { get; set; }
		public XpDateTime Until { get; set; }

		public LiquidationGetAllArguments(string filter, XpDateTime since, XpDateTime until)
		{
			Filter = filter ?? "";
			Since = since;
			Until = until;
		}
	}
}
