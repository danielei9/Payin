using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class ApiCampaignGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ApiCampaignGetArguments(int id)
		{
			Id = id;
		}
		public ApiCampaignGetArguments()
		{
		}
		#endregion Constructors
	}
}
