using Xp.Application.Dto;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class ApiCampaignLineGetByEntranceTypeResultBase : ResultBase<ApiCampaignLineGetByEntranceTypeResult>
	{
		public int CampaignId { get; set; }
		public string CampaignTitle { get; set; }
		public int CampaignLineId { get; set; }
	}
}
