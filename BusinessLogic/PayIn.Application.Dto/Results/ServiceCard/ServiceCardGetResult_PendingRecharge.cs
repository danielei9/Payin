using Xp.Common;

namespace PayIn.Application.Dto.Results
{
	public class ServiceCardGetResult_PendingRecharge
	{
		public XpDateTime DateTime { get; set; }
		public decimal Amount { get; set; }
	}
}
