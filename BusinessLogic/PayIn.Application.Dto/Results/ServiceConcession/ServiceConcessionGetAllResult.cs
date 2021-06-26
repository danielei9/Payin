using PayIn.Common;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Results.ServiceConcession
{
	public partial class ServiceConcessionGetAllResult
	{
		#region ServiceZoneResult
		public partial class Zone
		{
			public int      Id                { get; set; }
			public string   Name              { get; set; }
			public decimal? CancelationAmount { get; set; }
		}
		#endregion ServiceZoneResult

		public int               Id         { get; set; }
		public string            Name       { get; set; }
		public ServiceType       Type       { get; set; }
		public IEnumerable<Zone> Zones      { get; set; }
		public int               SupplierId { get; set; }
		public ConcessionState   State      { get; set; }

		#region Constructors
		public ServiceConcessionGetAllResult()
		{
			Zones = new List<Zone>();
		}
		#endregion Constructors
	}
}
