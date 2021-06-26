using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class MobileExhibitorGetVisitorResult_Notification
	{
		public int Id { get; set; }
		public XpDateTime CreatedAt { get; set; }
		public string Message { get; set; }
		public string SenderLogin { get; set; }
		public string ReceiverLogin { get; set; }
		public NotificationState State { get; set; }
	}
}
