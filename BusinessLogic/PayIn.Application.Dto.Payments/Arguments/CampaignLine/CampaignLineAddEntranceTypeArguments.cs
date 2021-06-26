using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class CampaignLineAddEntranceTypeArguments : IArgumentsBase
	{
		[Display(Name = "resources.campaignLine.entranceTypes")]
		public int EntranceTypeId { get; set; }
		public int Id { get; set; }

		#region Constructors
		public CampaignLineAddEntranceTypeArguments(int entranceTypeId)
		{
			EntranceTypeId = entranceTypeId;
		}
		#endregion Constructors
	}
}
