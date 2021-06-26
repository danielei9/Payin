using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class CampaignRemoveEventArguments : IArgumentsBase
	{
		public int Id { get; set; }
		public int EventId { get; set; }

		#region Constructors
		public CampaignRemoveEventArguments(int eventId, int campaign)
		{
            EventId = eventId;
			Id = campaign;
		}
		#endregion Constructors
	}
}

