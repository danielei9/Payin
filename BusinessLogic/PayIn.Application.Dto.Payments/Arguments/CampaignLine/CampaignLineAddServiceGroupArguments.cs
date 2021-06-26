using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class CampaignLineAddServiceGroupArguments : IArgumentsBase
	{
		[Display(Name = "resources.campaignLine.serviceGroups")]	public int ServiceGroupId { get; set; }
																	public int Id { get; set; }

		#region Constructors
		public CampaignLineAddServiceGroupArguments(int serviceGroupId)
		{
			ServiceGroupId = serviceGroupId;
		}
		#endregion Constructors
	}
}
