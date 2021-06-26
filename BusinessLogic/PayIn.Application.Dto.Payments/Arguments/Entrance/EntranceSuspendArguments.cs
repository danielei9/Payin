using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class EntranceSuspendArguments : IArgumentsBase
	{
		public int Id { set; get; }

		#region Constructor
		public EntranceSuspendArguments(int id)
		{
			Id = id;
		}
		#endregion Constructor
	}
}
