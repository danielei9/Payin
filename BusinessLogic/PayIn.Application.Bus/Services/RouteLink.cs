using System;

namespace PayIn.Application.Bus.Services
{
	public class RouteLink
	{
		public decimal? Weight { get; set; }
		public decimal? Value { get; set; }
		public string FromCode { get; set; }
		public string ToCode { get; set; }
	}
}
