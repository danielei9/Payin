using PayIn.Common;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Results.ServiceAddress
{
	public partial class ServiceAddressGetAllResult
	{
		public partial class ServiceAddressGetAll_NamesResult
		{
			public int             Id { get; set; }
			public string          Name { get; set; }
			public ProviderMapType ProviderMap { get; set; }
		}

		public int                 Id { get; set; }
		public string              Name { get; set; }
		public ServiceAddressSide? Side { get; set; }
		public string              SideLabel { get; set; }
		public int?                From { get; set; }
		public int?                Until { get; set; }
		public int                 ZoneId { get; set; }
		public string              ZoneName { get; set; }
		public int                 ConcessionId { get; set; }
		public string              ConcessionName { get; set; }

		public IEnumerable<ServiceAddressGetAll_NamesResult> Names { get; set; }

		public ServiceAddressGetAllResult()
		{
			Names = new List<ServiceAddressGetAll_NamesResult>();
		}

	}
}
