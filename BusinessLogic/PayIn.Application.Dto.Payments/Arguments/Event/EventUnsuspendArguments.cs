using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class EventUnsuspendArguments : IArgumentsBase
	{
		public int Id { set; get; }

		#region Constructor
		public EventUnsuspendArguments(int id)
		{
			Id = id;
		}
		#endregion Constructor
	}
}
