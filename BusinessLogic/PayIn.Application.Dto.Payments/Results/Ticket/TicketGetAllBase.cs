using Xp.Application.Dto;

namespace PayIn.Application.Dto.Payments.Results
{
    public class TicketGetAllResultBase : ResultBase<TicketGetAllResult>
	{
		public decimal TotalCharges { get; set; }
        public decimal TotalPaidCharges { get; set; }
        public decimal TotalRecharges { get; set; }
        public decimal TotalPaidRecharges { get; set; }
        public decimal TotalOrders { get; set; }
        public decimal TotalPaidOrders { get; set; }
        public decimal TotalShipments { get; set; }
        public decimal TotalPaidShipments { get; set; }
        public decimal TotalOthers { get; set; }
        public decimal TotalPaidOthers { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal TotalPaidAmount { get; set; }
    }
}
