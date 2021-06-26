using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class MobileCampaignGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public MobileCampaignGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
