using PayIn.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class MobileContactGetAllResult
	{
		public int Id { get; set; }
		public int? VisitorEntranceId { get; set; }
		public string VisitorName { get; set; }
		public int ExhibitorId { get; set; }
		public string ExhibitorName { get; set; }
		public int EventId { get; set; }
		public string EventName { get; set; }
		public string EventPhotoUrl { get; set; }
		public ContactState State { get; set; }
	}
}
