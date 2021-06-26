using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class TicketMobileDeleteArguments : IDeleteArgumentsBase<PayIn.Domain.Payments.Ticket>
	{
		public int Id { get; set; }

		#region Constructors
		public TicketMobileDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
