using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class CampaignLineRemoveEntranceTypeArguments : IArgumentsBase
	{
		public int Id { get; set; }
		public int EntranceTypeId { get; set; }

		#region Constructors
		public CampaignLineRemoveEntranceTypeArguments(int entranceTypeId)
		{
			EntranceTypeId = entranceTypeId;
		}
		#endregion Constructors
	}
}

