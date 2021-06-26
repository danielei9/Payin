using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Promotion
{
	public class PromotionCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.promotion.name")] 		[Required(AllowEmptyStrings = false)]		public string Name { get; set; }
		[Display(Name = "resources.promotion.acumulative")]                                      		public int? Acumulative { get; set; }
		[Display(Name = "resources.promotion.title")]                                           		public int? Title { get; set; }
		[Display(Name = "resources.promotion.startDate")]	[Required]                           		public XpDateTime StartDate { get; set; }
		[Display(Name = "resources.promotion.endDate")]	            	                            	public XpDateTime EndDate { get; set; }
		[Display(Name = "resources.promotion.quantityCode")]	[Required]                         		public int Quantity { get; set; }
		                                                                                                public dynamic PromoConditions { get; set; }
		//  public dynamic PromoActions { get; set; }
		[Display(Name = "resources.promotion.promoActions")]       [Required]      	                    public int PromoActions { get; set; }
		[Display(Name = "resources.promotion.promoLaunchers")]                                     		public int PromoLaunchers { get; set; }
		                                                                                                public IEnumerable<PromotionCreateArguments_PromoPrice> PromoPrices { get; set; }
		[Display(Name = "resources.promotion.concession")]		         [Required]                		public int Concession { get; set; }

		#region Constructor
		public PromotionCreateArguments(string name, int? acumulative, XpDateTime startDate, XpDateTime endDate,int quantity, dynamic promoConditions, int promoActions, int promoLaunchers, IEnumerable<PromotionCreateArguments_PromoPrice> promoPrices,int concession)
		{
			Name = name;
			Acumulative = acumulative;			
			StartDate = startDate;
			EndDate = endDate ?? XpDateTime.MaxValue;
			Quantity = quantity;		
			PromoConditions = promoConditions;
			PromoActions = promoActions;
			PromoLaunchers = promoLaunchers;
			PromoPrices = promoPrices;
			Concession = concession;		
		}
		#endregion Constructor
	}
}
