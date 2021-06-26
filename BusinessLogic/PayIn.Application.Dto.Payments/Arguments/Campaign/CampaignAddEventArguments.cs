using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class CampaignAddEventArguments : IArgumentsBase
	{
		[Display(Name = "resources.campaign.event")]		public int EventId { get; set; }
														    public int Id { get; set; }

		#region Constructors
		public CampaignAddEventArguments(int eventId)
		{
			EventId = eventId;
		}
		#endregion Constructors
	}
}
