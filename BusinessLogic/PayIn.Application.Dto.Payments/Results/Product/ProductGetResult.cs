using PayIn.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class ProductGetResult
	{
		public int Id							{ get; set; }
		public string Name						{ get; set; }
		public string Description				{ get; set; }
		public string PhotoUrl					{ get; set; }
		public decimal? Price					{ get; set; }
		public int? FamilyId					{ get; set; }
		public string FamilyName				{ get; set; }
		public bool IsVisible					{ get; set; }
        public string Code						{ get; set; }
		public ProductVisibility Visibility		{ get; set; }
		public bool SellableInTpv				{ get; set; }
		public bool SellableInWeb				{ get; set; }
	}
}
