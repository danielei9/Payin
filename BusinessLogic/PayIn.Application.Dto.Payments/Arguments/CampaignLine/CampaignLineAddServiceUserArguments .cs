using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class CampaignLineAddServiceUserArguments : IArgumentsBase
	{
		[Display(Name = "resources.campaignLine.serviceUsers")]	public int ServiceUserId { get; set; }
																public int Id { get; set; }

		#region Constructors
		public CampaignLineAddServiceUserArguments(int serviceUserId)
		{
			ServiceUserId = serviceUserId;
		}
		#endregion Constructors
	}
}
