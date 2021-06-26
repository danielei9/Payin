using System.Collections.Generic;

namespace PayIn.Application.Dto.Payments.Results
{
	public class MobileExhibitorGetVisitorResult
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string Login { get; set; }
		public string EventName { get; set; }
		public int? EntranceId { get; set; }

		public IEnumerable<MobileExhibitorGetVisitorResult_Notification> Notifications { get; set; }
	}
}
