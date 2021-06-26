using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Shipment
{
	public class ShipmentGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public ShipmentGetAllArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors
	}
}
