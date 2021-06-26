namespace PayIn.Application.Dto.Payments.Results
{
	public class MobileEntranceTypeGetBuyableResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal ExtraPrice { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
		public int? MaxEntrancesPerCard { get; set; }
        public int PaymentConcessionId { get; set; }
    }
}
