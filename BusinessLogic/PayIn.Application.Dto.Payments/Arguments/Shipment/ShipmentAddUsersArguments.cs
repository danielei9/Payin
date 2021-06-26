using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;


namespace PayIn.Application.Dto.Payments.Arguments.Shipment
{
	public class ShipmentAddUsersArguments : IArgumentsBase
	{
		[Display(Name = "resources.shipment.name")]
		public string Name { get; set; }
		[Display(Name = "resources.shipment.amount")]
		public decimal Amount { get; set; }
		[Display(Name = "resources.shipment.since")]
		public XpDateTime Since { get; set; }
		[Display(Name = "resources.shipment.until")]
		public XpDateTime Until { get; set; }


		#region Constructor
		public ShipmentAddUsersArguments(string name, decimal amount, XpDateTime since, XpDateTime until)
		{
			Name = name;
			Amount = amount;
			Since = since;
			Until = until;

		}
		#endregion Constructor
	}
}