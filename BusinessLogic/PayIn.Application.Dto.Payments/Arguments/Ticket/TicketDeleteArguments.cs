using PayIn.Domain.Payments;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class TicketDeleteArguments : IDeleteArgumentsBase<Ticket>
	{
		public int Id { get; set; }

		#region Constructors
		public TicketDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}