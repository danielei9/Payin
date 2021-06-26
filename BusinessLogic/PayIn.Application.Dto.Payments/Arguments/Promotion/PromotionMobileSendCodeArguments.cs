using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Promotion
{
	public class PromotionMobileSendCodeArguments : IArgumentsBase
	{
		
		public class Promotions
		{
			public int Id { get; set; }
			public string Code { get; set; }
		}

		[Required(AllowEmptyStrings = false)]       public string Email { get; set; }

		public IEnumerable<Promotions> Promotion { get; set; }

	
		#region Constructor
		public PromotionMobileSendCodeArguments(string email, dynamic promotion)
		{
			Email = email;
			Promotion = promotion;
		}
		#endregion Constructor
	}
}