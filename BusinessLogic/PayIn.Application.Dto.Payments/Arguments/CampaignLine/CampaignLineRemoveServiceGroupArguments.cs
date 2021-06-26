using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class CampaignLineRemoveServiceGroupArguments : IArgumentsBase
	{
		public int Id { get; set; }
		public int ServiceGroupId { get; set; }

		#region Constructors
		public CampaignLineRemoveServiceGroupArguments(int serviceGroupId)
		{
			ServiceGroupId = serviceGroupId;
		}
		#endregion Constructors
	}
}

