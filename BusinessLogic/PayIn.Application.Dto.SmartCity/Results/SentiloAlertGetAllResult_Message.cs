using Xp.Common;

namespace PayIn.Application.Dto.SmartCity.Results
{
	public partial class SentiloAlertGetAllResult_Message
	{
		public string Message { get; set; }
		public XpDateTime Timestamp { get; set; }
		public string Sender { get; set; }
	}
}
