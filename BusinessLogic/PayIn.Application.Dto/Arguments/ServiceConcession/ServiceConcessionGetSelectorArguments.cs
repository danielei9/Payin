using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceConcession
{
	public partial class ServiceConcessionGetSelectorArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		public int? SystemCardId { get; set; }

		#region Constructors
		public ServiceConcessionGetSelectorArguments(string filter, int? systemCardId)
		{
			Filter = filter;
			SystemCardId = systemCardId;
		}
		#endregion Constructors
	}
}
