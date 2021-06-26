using PayIn.Common;
using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Promotion
{
	public class PromotionAddCodeArguments : IArgumentsBase
	{
	
		[Display(Name = "resources.promotion.addCode")]		[Required] public int Quantity { get; set; }
			                                                [Required]		public int Id { get; set; }


		#region Constructor
		public PromotionAddCodeArguments(int quantity, int id)
		{
			Quantity = quantity;
			Id = id;
		}
		#endregion Constructor
	}
}
