using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Shipment
{
	public class ShipmentGetAddUserArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ShipmentGetAddUserArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
