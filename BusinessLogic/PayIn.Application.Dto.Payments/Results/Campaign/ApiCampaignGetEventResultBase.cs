using Xp.Application.Dto;

namespace PayIn.Application.Dto.Payments.Results
{
	public class ApiCampaignGetEventResultBase : ResultBase<ApiCampaignGetEventResult>
	{
		public int Id { get; set; }
		public string Title { get; set; }
	}
}
