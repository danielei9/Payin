using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class TicketMobileGetDuesArguments : IArgumentsBase
	{
		public int Skip { get; set; }
		public int Top { get; set; }
	}
}
