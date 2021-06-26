using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class TicketMobileGetPayArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public TicketMobileGetPayArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
