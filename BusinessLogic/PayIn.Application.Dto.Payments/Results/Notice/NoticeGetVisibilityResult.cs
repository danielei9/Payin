using PayIn.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class NoticeGetVisibilityResult
	{
		public int Id { get; set; }
		public NoticeVisibility Visibility { get; set; }
	}
}
