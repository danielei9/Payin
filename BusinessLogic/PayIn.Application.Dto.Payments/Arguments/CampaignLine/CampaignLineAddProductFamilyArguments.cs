using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class CampaignLineAddProductFamilyArguments : IArgumentsBase
	{
		[Display(Name = "resources.campaignLine.productFamilies")]		public int ProductFamilyId { get; set; }
																		public int Id { get; set; }

		#region Constructors
		public CampaignLineAddProductFamilyArguments(int productFamilyId)
		{
			ProductFamilyId = productFamilyId;
		}
		#endregion Constructors
	}
}
