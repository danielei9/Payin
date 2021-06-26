using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class ApiCampaignLineGetByEntranceTypeArguments : IArgumentsBase
	{
		public int CampaignLineId { get; set; }

		#region Constructors
		public ApiCampaignLineGetByEntranceTypeArguments(int id)
		{
			CampaignLineId = id;
		}
		#endregion Constructors
	}
}
