namespace PayIn.Application.Dto.Payments.Results
{
    public partial class ProductUpdateGetResult
	{
		public int Id				{ get; set; }
		public string Name			{ get; set; }
        public decimal? Price		{ get; set; }
        public string Description	{ get; set; }
        public int? ParentId		{ get; set; }
        public string PhotoUrl		{ get; set; }
        public bool SellableInTpv	{ get; set; }
        public bool SellableInWeb	{ get; set; }
}
}
