using PayIn.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class ProductGetVisibilityResult
	{
		public int Id { get; set; }
		public ProductVisibility Visibility { get; set; }
	}
}
