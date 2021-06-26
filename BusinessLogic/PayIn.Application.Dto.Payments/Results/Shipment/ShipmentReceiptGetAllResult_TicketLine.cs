namespace PayIn.Application.Dto.Payments.Results.Shipment
{
	public class ShipmentReceiptGetAllResult_TicketLine
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public decimal Amount { get; set; }
		public decimal Quantity { get; set; }
	}
}
