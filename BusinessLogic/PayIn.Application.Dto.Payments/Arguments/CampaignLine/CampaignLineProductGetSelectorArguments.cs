using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class CampaignLineProductGetSelectorArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		public int Id { get; set; }

		#region Constructors
		public CampaignLineProductGetSelectorArguments(string filter, int id)
		{
			Filter = filter ?? "";
			Id = id;
		}
		#endregion Constructors
	}
}
