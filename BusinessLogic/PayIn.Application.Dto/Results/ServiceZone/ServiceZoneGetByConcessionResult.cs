using PayIn.Common;
using System;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Results.ServiceZone
{
	public partial class ServiceZoneGetByConcessionResult
	{
		#region ServiceAddressResult
		public partial class ServiceZoneGetByConcession_ServiceAddressResult
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public int? From { get; set; }
			public int? Until { get; set; }
			public ServiceAddressSide? Side { get; set; }
			public int CityId { get; set; }
			public int ZoneId { get; set; }
		}
		#endregion ServiceAddressResult

		#region ServicePriceResult
		public partial class ServiceZoneGetByConcession_ServicePriceResult
		{
			public int Id { get; set; }
			public decimal Price { get; set; }
			public TimeSpan Time { get; set; }
			public int ZoneId { get; set; }
		}
		#endregion ServicePriceResult

		#region ServiceTimeTableResult
		public partial class ServiceZoneGetByConcession_ServiceTimeTableResult
		{
			public int Id { get; set; }
			public DayOfWeek FromWeekday { get; set; }
			public DayOfWeek UntilWeekday { get; set; }
			public TimeSpan FromHour { get; set; }
			public TimeSpan DurationHour { get; set; }
			public int ZoneId { get; set; }
		}
		#endregion ServiceTimeTableResult

		public int ConcessionId { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public IEnumerable<ServiceZoneGetByConcession_ServiceAddressResult> Addresses { get; set; }
		public decimal? CancelationAmount { get; set; }
		public IEnumerable<ServiceZoneGetByConcession_ServicePriceResult> Prices { get; set; }
		public IEnumerable<ServiceZoneGetByConcession_ServiceTimeTableResult> TimeTables { get; set; }

		public ServiceZoneGetByConcessionResult()
		{
			Addresses = new List<ServiceZoneGetByConcession_ServiceAddressResult>();
			Prices = new List<ServiceZoneGetByConcession_ServicePriceResult>();
			TimeTables = new List<ServiceZoneGetByConcession_ServiceTimeTableResult>();
		}
	}
}
