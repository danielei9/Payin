using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class CampaignLineProductFamilyGetSelectorArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		public int Id { get; set; }

		#region Constructors
		public CampaignLineProductFamilyGetSelectorArguments(string filter, int id)
		{
			Filter = filter ?? "";
			Id = id;
		}
		#endregion Constructors
	}
}
