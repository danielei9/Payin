namespace PayIn.Application.Dto.Payments.Results
{
	public class ExhibitorMobileGetResult_Product
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string PhotoUrl { get; set; }
		public decimal? Price { get; set; }
		public ExhibitorMobileGetResult_ProductTypeEnum Type { get; set; }
		public int? FamilyId { get; set; }
	}
}
