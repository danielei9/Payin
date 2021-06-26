using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class MobileEventGetAllCheckViewResult
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Place { get; set; }
		public string PhotoUrl { get; set; }
		public XpDateTime EventStart { get; set; }
		public XpDateTime EventEnd { get; set; }
		public decimal MinPrice { get; set; }
		public decimal MaxPrice { get; set; }
	}
}
