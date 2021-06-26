using PayIn.Common;
using Xp.Application.Dto;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results.CampaignLine
{
	public class ApiCampaignLineGetAllResultBase : ResultBase<ApiCampaignLineGetAllResult>
	{
		public string Title { get; set; }
	}
}
