using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class CampaignLineRemoveProductFamilyArguments : IArgumentsBase
	{
		public int Id { get; set; }
		public int ProductFamilyId { get; set; }

		#region Constructors
		public CampaignLineRemoveProductFamilyArguments(int productFamilyId)
		{
			ProductFamilyId = productFamilyId;
		}
		#endregion Constructors
	}
}

