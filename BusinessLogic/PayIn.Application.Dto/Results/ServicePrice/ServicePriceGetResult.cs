using System;

namespace PayIn.Application.Dto.Results.ServicePrice
{
	public partial class ServicePriceGetResult
	{
		public int		Id		{ get; set; }
		public TimeSpan	Time	{ get; set; }
		public decimal	Price	{ get; set; }
	}
}
