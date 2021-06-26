using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Shipment
{
	public partial class ShipmentTicketGetAllArguments : IArgumentsBase
	{
		public int Id { get; set; }
		

		#region Constructors
		public ShipmentTicketGetAllArguments(int id)
		{
			Id = id;			
		}
		public ShipmentTicketGetAllArguments()
		{
		}
		#endregion Constructors
	}
}
