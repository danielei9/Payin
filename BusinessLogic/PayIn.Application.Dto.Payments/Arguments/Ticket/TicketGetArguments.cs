using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class TicketGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public TicketGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
