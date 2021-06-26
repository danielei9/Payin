using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceZone
{
	public partial class ServiceZoneGetSelectorArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public ServiceZoneGetSelectorArguments(string filter)
		{
			Filter = filter;
		}
		#endregion Constructors
	}
}
