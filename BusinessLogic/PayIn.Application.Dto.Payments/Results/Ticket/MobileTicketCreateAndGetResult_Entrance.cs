using PayIn.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class MobileTicketCreateAndGetResult_Entrance
	{
		public int Id { get; set; }
		public int EntranceTypeId { get; set; }
		public int EventId { get; set; }
		public string Code { set; get; }
		public string CodeText { get; set; }
		public EntranceSystemType CodeType { get; set; }
	}
}
