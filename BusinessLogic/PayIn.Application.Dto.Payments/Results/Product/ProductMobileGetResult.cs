namespace PayIn.Application.Dto.Payments.Results
{
	public partial class ProductMobileGetResult
	{
		public int Id						{ get; set; }
		public string Name					{ get; set; }
		public string Description			{ get; set; }
		public string PhotoUrl				{ get; set; }
		public decimal? Price				{ get; set; }
		public int? ConcessionId            { get; set; }
		public string ConcessionName        { get; set; }
	}
}
