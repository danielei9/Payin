using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class EventSuspendArguments : IArgumentsBase
	{
		public int Id { set; get; }

		#region Constructor
		public EventSuspendArguments(int id)
		{
			Id = id;
		}
		#endregion Constructor
	}
}
