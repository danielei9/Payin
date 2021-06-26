using PayIn.Common;
using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceAddress
{
	public partial class ServiceAddressCreateArguments : IArgumentsBase
	{
		//public int ConcessionId { get; private set; }
		[Display(Name = "resources.serviceCity.city")] public int                 CityId { get; private set; }
		[Display(Name = "resources.serviceZone.zone")] public int                 ZoneId { get; private set; }
		                                               public string              Name   { get; private set; }
		                                               public int?                From   { get; private set; }
		                                               public int?                Until  { get; private set; }
		                                               public ServiceAddressSide? Side   { get; private set; }

		#region Constructors
		public ServiceAddressCreateArguments(int cityId, int zoneId, string name, int? from, int? until, ServiceAddressSide? side)
		{
			CityId = cityId;
			//ConcessionId = concessionId;
			ZoneId = zoneId;
			Name = name;
			From = from;
			Until = until;
			Side = side;
		}
		#endregion Constructors
	}
}
