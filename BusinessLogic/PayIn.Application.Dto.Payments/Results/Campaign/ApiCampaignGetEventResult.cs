using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class ApiCampaignGetEventResult
	{
		public int Id { get; set; }
		public int CampaignId { get; set; }
		public XpDateTime SinceTime { get; set; }
		public XpDateTime UntilTime { get; set; }
        public string Name { get; set; }
	}
}
