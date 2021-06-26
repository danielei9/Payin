using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class CampaignLineServiceUserGetSelectorArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		public int Id { get; set; }

		#region Constructors
		public CampaignLineServiceUserGetSelectorArguments(string filter, int id)
		{
			Filter = filter ?? "";
			Id = id;
		}
		#endregion Constructors
	}
}
