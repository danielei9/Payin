using PayIn.Common;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Results.ServiceCity
{
	public partial class ServiceCityGetByZoneTypeResult
	{
		public partial class ServiceCityGetByZoneType_AddressResult
		{
			public int									Id					{ get; set; }
			public string								Name				{ get; set; }
			public ServiceAddressSide?	Side				{ get; set; }
			public int?									From				{ get; set; }
			public int?									Until				{ get; set; }
			public CalculateZone				Zone				{ get; set; }
		}

		public int																									Id				{ get; set; }
		public string																								Name			{ get; set; }
		public IEnumerable<ServiceCityGetByZoneType_AddressResult>	Addresses { get; set; }

		public ServiceCityGetByZoneTypeResult()
		{
			Addresses = new List<ServiceCityGetByZoneType_AddressResult>();
		}
	}
}
