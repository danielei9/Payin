using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class EntranceUnsuspendArguments : IArgumentsBase
	{
		public int Id { set; get; }

		#region Constructor
		public EntranceUnsuspendArguments(int id)
		{
			Id = id;
		}
		#endregion Constructor
	}
}
