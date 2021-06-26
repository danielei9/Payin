using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Promotion
{
	public class PromotionUpdateArguments : IArgumentsBase
	{
																										public int Id { get; set; }
		[Display(Name = "resources.promotion.name")] 		[Required(AllowEmptyStrings = false)]		public string Name { get; set; }
		[Display(Name = "resources.promotion.acumulative")]                                     		public int Acumulative { get; set; }
		[Display(Name = "resources.promotion.title")]		                                    		public int Title { get; set; }
		[Display(Name = "resources.promotion.startDate")]	[Required]                            		public XpDateTime StartDate { get; set; }
		[Display(Name = "resources.promotion.endDate")]                                            		public XpDateTime EndDate { get; set; }
		[Display(Name = "resources.promotion.quantityCode")] [Required]                         		public int Quantity { get; set; }
																										public dynamic PromoConditions { get; set; }
		[Display(Name = "resources.promotion.promoActions")]		[Required]                     		public int PromoActions { get; set; }
		[Display(Name = "resources.promotion.promoLaunchers")]		                                    public dynamic PromoLaunchers { get; set; }
																										public IEnumerable<PromotionUpdateArguments_PromoPrice> PromoPrices { get; set; }
		[Display(Name = "resources.promotion.concession")]		[Required]								public int Concession { get; set; }
		[Display(Name = "resources.promotion.titleQuantity")]   [Required]								public int TitleQuantity { get; set; }
																										public bool isOwner { get; set; }
		#region Constructor
		public PromotionUpdateArguments(int id, string name, int acumulative, int title, XpDateTime startDate, XpDateTime endDate, dynamic promoConditions, int promoActions, dynamic promoLaunchers, int quantity, int concession, IEnumerable<PromotionUpdateArguments_PromoPrice> promoPrices, int titleQuantity, bool isOwner)
		{
			Id = id;
			Name = name;
			Acumulative = acumulative;
			Title = title;
			StartDate = startDate;
			EndDate = endDate;
			PromoConditions = promoConditions;
			PromoActions = promoActions;
			PromoLaunchers = promoLaunchers;
			Quantity = quantity;
			Concession = concession;
			PromoPrices = promoPrices;
			TitleQuantity = titleQuantity;
			this.isOwner = isOwner;
		}
		#endregion Constructor
	}
}