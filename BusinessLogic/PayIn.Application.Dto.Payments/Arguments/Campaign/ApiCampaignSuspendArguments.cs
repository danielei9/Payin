using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class ApiCampaignSuspendArguments : IArgumentsBase
	{
		public int Id { set; get; }

		#region Constructor
		public ApiCampaignSuspendArguments(int id)
		{
			Id = id;
		}
		#endregion Constructor
	}
}
