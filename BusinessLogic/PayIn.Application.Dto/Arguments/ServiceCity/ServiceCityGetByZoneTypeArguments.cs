using PayIn.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceCity
{
	public partial class ServiceCityGetByZoneTypeArguments : IArgumentsBase
	{
		public ServiceType Type { get; set; }

		#region Constructors
		public ServiceCityGetByZoneTypeArguments(ServiceType type)
		{
			Type = type;
		}
		#endregion Constructors
	}
}
