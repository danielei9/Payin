using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceAddress
{
	public partial class ServiceAddressGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		public int? CityId { get; set; }

		#region Constructors
		public ServiceAddressGetAllArguments(string filter, int? cityId)
		{
			Filter = filter ?? "";
			CityId = cityId;
		}
		#endregion Constructors
	}
}
