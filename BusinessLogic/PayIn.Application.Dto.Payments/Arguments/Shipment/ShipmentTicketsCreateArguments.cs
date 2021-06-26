using PayIn.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Shipment
{
	public class ShipmentTicketsCreateArguments : IArgumentsBase
	{
		public int ShipmentId { get; set; }
		public List<int> PaymentUserIds { get; set; }

		#region Constructors
		public ShipmentTicketsCreateArguments(int shipmentId,List<int> paymentUserIds)
		{
			ShipmentId = shipmentId;
			PaymentUserIds = paymentUserIds;
		}
		#endregion Constructors

	}
}
