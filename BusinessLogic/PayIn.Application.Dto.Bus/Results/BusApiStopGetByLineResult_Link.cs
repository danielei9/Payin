using PayIn.Domain.Bus.Enums;
using System;

namespace PayIn.Application.Dto.Bus.Results
{
	public class BusApiStopGetByLineResult_Link
	{
		public int Id { get; set; }
		public int Weight { get; set; }
		public TimeSpan Time { get; set; }
		public string ToCode { get; set; }
		public string ToName { get; set; }
		public RouteSense Sense { get; set; }
	}
}
