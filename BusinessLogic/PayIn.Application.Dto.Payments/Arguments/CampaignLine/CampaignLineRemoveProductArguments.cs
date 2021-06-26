using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class CampaignLineRemoveProductArguments : IArgumentsBase
	{
		public int Id { get; set; }
		public int ProductId { get; set; }

		#region Constructors
		public CampaignLineRemoveProductArguments(int productId)
		{
			ProductId = productId;
		}
		#endregion Constructors
	}
}

