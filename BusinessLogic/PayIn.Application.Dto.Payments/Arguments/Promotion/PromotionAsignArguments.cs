using PayIn.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Promotion
{
	public class PromotionAsignArguments : IArgumentsBase
	{
		public string Code { get; set; }
		public PromotionExecutionType PromotionCodeType { get; set; }

		#region Constructors
		public PromotionAsignArguments(string code, PromotionExecutionType promotionCodeType)
		{
			Code = code;
			PromotionCodeType = promotionCodeType;
		}
		#endregion Constructors
	}
}
