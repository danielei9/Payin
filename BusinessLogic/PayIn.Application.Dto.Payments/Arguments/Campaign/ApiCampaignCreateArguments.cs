using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class ApiCampaignCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.campaign.paymentConcession")] [Required] public int PaymentConcessionId { get; set; }
		[Display(Name = "resources.campaign.title")]             [Required] public string Title { get; set; }
		[Display(Name = "resources.campaign.description")]                  public string Description { get; set; }
		[Display(Name = "resources.campaign.since")]             [Required] public XpDate Since { get; set; }
		[Display(Name = "resources.campaign.until")]                        public XpDate Until { get; set; }
		[Display(Name = "resources.campaign.numberOfType")] 	            public int? NumberOfTimes { get; set; }
		[Display(Name = "resources.campaign.systemCard")]                   public int? TargetSystemCardId { get; set; }
		[Display(Name = "resources.campaign.serviceConcession")]            public int? TargetConcessionId { get; set; }

		#region Constructor
		public ApiCampaignCreateArguments(int paymentConcessionId, string title, string description, XpDate since, XpDate until, int? numberOfTimes, int? targetSystemCardId, int? targetConcessionId)
		{
			PaymentConcessionId = paymentConcessionId;
			Title = title;
			Description = description ?? "";
			Since = since;
			Until = until ?? XpDate.MaxValue;
			NumberOfTimes = numberOfTimes;
			TargetSystemCardId = targetSystemCardId;
			TargetConcessionId = targetConcessionId;
		}
		#endregion Constructor
	}
}
