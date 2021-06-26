using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.CampaignLine
{
	public partial class CampaignLineGetSelectorArguments : IArgumentsBase
	{
		public string Filter { get; private set; }

		#region Constructors
		public CampaignLineGetSelectorArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors
	}
}

