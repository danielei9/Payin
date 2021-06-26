using PayIn.Common.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace PayIn.Application.Dto.Results.ServicePrice
{
	public partial class ServicePriceGetAllResult
	{
		                                                                                                           public int      Id				{ get; set; }
		[Display(ResourceType = typeof(ServicePriceResources), Name = "Time")]       [DataType(DataType.Time)]     public TimeSpan Time				{ get; set; }
		[Display(ResourceType = typeof(ServicePriceResources), Name = "Price")]      [DataType(DataType.Currency)] public decimal  Price            { get; set; }
		[Display(ResourceType = typeof(ServicePriceResources), Name = "Zone")]                                     public int      ZoneId			{ get; set; }
		                                                                                                           public string   ZoneName			{ get; set; }
		[Display(ResourceType = typeof(ServicePriceResources), Name = "Concession")]                               public int      ConcessionId		{ get; set; }
		                                                                                                           public string   ConcessionName	{ get; set; }
	}
}
