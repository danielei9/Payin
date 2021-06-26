using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class TicketGetDetailsArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public TicketGetDetailsArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
