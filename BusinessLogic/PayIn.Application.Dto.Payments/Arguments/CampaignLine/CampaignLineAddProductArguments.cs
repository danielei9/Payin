using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class CampaignLineAddProductArguments : IArgumentsBase
	{
		[Display(Name = "resources.campaignLine.products")]		public int ProductId { get; set; }
																public int Id { get; set; }

		#region Constructors
		public CampaignLineAddProductArguments(int productId)
		{
			ProductId = productId;
		}
		#endregion Constructors
	}
}
