
namespace PayIn.Application.Dto.Results
{
	public partial class PaymentMediaGetAllResult
	{
		public int    Id          { get; set; }
		public string Title       { get; set; }
		public string Subtitle    { get; set; }
		public int    VisualOrder { get; set; }
		public string ImagePath   { get; set; }
	}
}
