using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
    public class MobileNoticeGetAllPublicResult
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string ShortDescription { get; set; }
		public string PhotoUrl { get; set; }
		public XpDateTime Start { get; set; }
	}
}
