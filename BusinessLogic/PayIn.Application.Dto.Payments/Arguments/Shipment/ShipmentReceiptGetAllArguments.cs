using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Shipment
{
	public class ShipmentReceiptGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		public XpDate Since { get; set; }
		public XpDate Until { get; set; }

		#region Constructors
		public ShipmentReceiptGetAllArguments(string filter,XpDate since,XpDate until)
		{
			Filter = filter ?? "";
			Since = since;
			Until = until;
		}
		#endregion Constructors
	}
}
