using PayIn.Common.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServicePrice
{
	public partial class ServicePriceUpdateArguments : IArgumentsBase
	{
		                                                 public int      Id    { get; set; }
		[Display(Name = "resources.servicePrice.price")] public decimal  Price { get; private set; }
		[Display(Name = "resources.servicePrice.time")]  public TimeSpan Time  { get; private set; }

		#region Constructors
		public ServicePriceUpdateArguments(int id, TimeSpan time, decimal price)
		{
			Id = id;
			Price = price;
			Time = time;
		}
		#endregion Constructors
	}
}
