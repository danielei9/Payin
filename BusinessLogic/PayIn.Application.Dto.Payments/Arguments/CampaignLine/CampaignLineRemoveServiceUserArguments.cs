using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class CampaignLineRemoveServiceUserArguments : IArgumentsBase
	{
		public int Id { get; set; }
		public int ServiceUserId { get; set; }

		#region Constructors
		public CampaignLineRemoveServiceUserArguments(int serviceUserId)
		{
			ServiceUserId = serviceUserId;
		}
		#endregion Constructors
	}
}

