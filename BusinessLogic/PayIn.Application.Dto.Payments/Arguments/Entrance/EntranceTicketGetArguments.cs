using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class EntranceTicketGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public EntranceTicketGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
