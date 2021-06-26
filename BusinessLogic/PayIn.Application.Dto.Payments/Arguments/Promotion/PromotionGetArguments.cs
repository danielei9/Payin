using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Promotion
{
	public partial class PromotionGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public PromotionGetArguments(int id)
		{
			Id = id;
		}
		public PromotionGetArguments()
		{
		}
		#endregion Constructors
	}
}
