using System;

namespace PayIn.Application.Dto.Payments.Results
{
    public partial class ApiAccessControlGetWeatherResult
	{
		public decimal Radiation		{ get; set; }
		public decimal UVIndex			{ get; set; }
		public decimal Temperature		{ get; set; }
		public decimal WindSpeed		{ get; set; }
		public decimal WindDirection	{ get; set; }
		public decimal Humidity			{ get; set; }
		public decimal Pressure			{ get; set; }
		public DateTime DateTime		{ get; set; }
	}
}