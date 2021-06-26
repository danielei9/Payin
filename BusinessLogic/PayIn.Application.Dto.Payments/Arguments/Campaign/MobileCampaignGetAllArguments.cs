using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobileCampaignGetAllArguments : IArgumentsBase
	{
		public int Skip { get; set; }
		public int Top { get; set; }
		public long Uid { get; set; }
	}
}
