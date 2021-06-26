using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.CampaignLine
{
	public class CampaignLineDeleteArguments : IArgumentsBase
	{
		public int Id { set; get; }

		#region Constructor
		public CampaignLineDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructor
	}
}
