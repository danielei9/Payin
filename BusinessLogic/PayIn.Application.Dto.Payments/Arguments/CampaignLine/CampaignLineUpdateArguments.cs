using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.CampaignLine
{
	public class CampaignLineUpdateArguments : IArgumentsBase
	{

																		public int					Id					{ get; set; }
		[Display(Name = "resources.campaignLine.campaignLineType")]		public CampaignLineType		Type				{ get; set; }
																		public decimal				Min					{ get; set; }
																		public decimal				Max					{ get; set; }
		[Display(Name = "resources.campaignLine.quantity")]				public decimal				Quantity			{ get; set; }
		[Display(Name = "resources.campaignLine.sinceTime")]			public XpTime				SinceTime			{ get; set; }
		[Display(Name = "resources.campaignLine.untilTime")]			public XpTime				UntilTime			{ get; set; }
		[Display(Name = "resources.campaignLine.purse")]				public int?					PurseId				{ get; set; }
		[Display(Name = "resources.campaignLine.allProduct")]			public bool					AllProduct			{ get; set; }
		[Display(Name = "resources.campaignLine.allEntranceType")]		public bool					AllEntranceType		{ get; set; }
		[Display(Name = "resources.campaignLine.all")]					public bool					All					{ get; set; }

		#region Constructor
		public CampaignLineUpdateArguments(int id, CampaignLineType type, decimal min, decimal max, decimal quantity, XpTime sinceTime, XpTime untilTime, int? purseId, bool allProduct, bool allEntranceType, bool all)
		{
			Id = id;
			Type = type;
			Min = min;
			Max = max;
			Quantity = quantity;
			SinceTime = sinceTime;
			UntilTime = untilTime;
			PurseId = purseId;
			AllProduct = allProduct;
			AllEntranceType = allEntranceType;
			All = all;
		}
		#endregion Constructor
	}
}