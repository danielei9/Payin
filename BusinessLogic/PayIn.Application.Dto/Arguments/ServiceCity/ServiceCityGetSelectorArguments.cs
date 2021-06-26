using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceCity
{
	public partial class ServiceCityGetSelectorArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public ServiceCityGetSelectorArguments(string filter)
		{
			Filter = filter;
		}
		#endregion Constructors
	}
}
