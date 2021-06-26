using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class ApiCampaignDeleteArguments : IArgumentsBase
	{
		public int Id { set; get; }

		#region Constructor
		public ApiCampaignDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructor
	}
}
