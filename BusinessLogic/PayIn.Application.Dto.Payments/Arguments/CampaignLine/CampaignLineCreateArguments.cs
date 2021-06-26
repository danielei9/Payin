using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.CampaignLine
{
	public class CampaignLineCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.campaignLine.campaignLineType")]   [Required]	public CampaignLineType Type { get; set; }
		[Display(Name = "resources.campaignLine.minText")]                          public decimal Min { get; set; }
		[Display(Name = "resources.campaignLine.maxText")]                          public decimal Max { get; set; }
		[Display(Name = "resources.campaignLine.quantity")]	                        public decimal Quantity { get; set; }
		[Display(Name = "resources.campaignLine.sinceTime")]                        public XpTime SinceTime { get; set; }
		[Display(Name = "resources.campaignLine.untilTime")]                        public XpTime UntilTime { get; set; }
																	            	public int CampaignId { get; set; }
		[Display(Name = "resources.campaignLine.purse")]						    public int? PurseId { get; set; }
		[Display(Name = "resources.campaignLine.allProduct")]						public bool AllProduct { get; set; }
		[Display(Name = "resources.campaignLine.allEntranceType")]					public bool AllEntranceType { get; set; }

		#region Constructors
		public CampaignLineCreateArguments(int campaignId, CampaignLineType type, decimal min, decimal max, decimal quantity, int? purseId, bool allProduct, bool allEntranceType)
		{
			CampaignId = campaignId;
			Type = type;
			Min = min;
			Max = max;
			Quantity = quantity;
			PurseId = purseId;
			AllProduct = allProduct;
			AllEntranceType = allEntranceType;
		}
		#endregion Constructors
	}
}
