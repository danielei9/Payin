using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Promotion
{
	public class PromotionDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public PromotionDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
