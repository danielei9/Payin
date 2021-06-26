using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Shipment
{
	public partial class ShipmentGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ShipmentGetArguments(int id)
		{
			Id = id;
		}
		public ShipmentGetArguments()
		{
		}
		#endregion Constructors
	}
}
