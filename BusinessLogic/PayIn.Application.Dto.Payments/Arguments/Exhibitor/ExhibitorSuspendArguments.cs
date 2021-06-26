using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class ExhibitorSuspendArguments : IArgumentsBase
	{
		public int Id { set; get; }

		#region Constructor
		public ExhibitorSuspendArguments(int id)
		{
			Id = id;
		}
		#endregion Constructor
	}
}
