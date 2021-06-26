using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Shipment
{
	public class ShipmentDeleteArguments : IArgumentsBase
	{
		public int Id { set; get; }

		#region Constructor
		public ShipmentDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructor
	}
}
