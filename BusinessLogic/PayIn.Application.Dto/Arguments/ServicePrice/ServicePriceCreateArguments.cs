using PayIn.Common.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServicePrice
{
	public partial class ServicePriceCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.servicePrice.price")] [DataType(DataType.Currency)] public decimal  Price  { get; private set; }
		[Display(Name = "resources.servicePrice.zone")]                                public int      ZoneId { get; private set; }
		[Display(Name = "resources.servicePrice.time")]                                public TimeSpan Time   { get; private set; }

		#region Constructors
		public ServicePriceCreateArguments(int zoneId, TimeSpan time, decimal price)
		{
			Price = price;
			Time = time;
			ZoneId = zoneId;
		}
		#endregion Constructors
	}
}
