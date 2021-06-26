using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class TicketMobileGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public TicketMobileGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
