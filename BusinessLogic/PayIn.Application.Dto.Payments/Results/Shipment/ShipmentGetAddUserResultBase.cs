using System.Collections.Generic;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.Payments.Results.Shipment
{
	public class ShipmentGetAddUserResultBase : ResultBase<ShipmentGetAddUserResult>
	{
		public class User
		{
			public int Id { set; get; }
			public string Name { set; get; }
			public string Login { set; get; }
		}

		public IEnumerable<User> Users { get; set; }
	}
}
