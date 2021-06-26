using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Promotion
{
	public class PromotionUnlinkCodeArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public PromotionUnlinkCodeArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
