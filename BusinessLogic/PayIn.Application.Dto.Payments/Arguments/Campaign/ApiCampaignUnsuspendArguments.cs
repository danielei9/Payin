using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class ApiCampaignUnsuspendArguments : IArgumentsBase
	{
		public int Id { set; get; }

		#region Constructor
		public ApiCampaignUnsuspendArguments(int id)
		{
			Id = id;
		}
		#endregion Constructor
	}
}
