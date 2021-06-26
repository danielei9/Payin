using System;

namespace PayIn.Application.Dto
{
  public class CalculatePrice
  {
		public int			Id		{ get; set; }
		public decimal	Price	{ get; set; }
		public TimeSpan	Time	{ get; set; }
	}
}
