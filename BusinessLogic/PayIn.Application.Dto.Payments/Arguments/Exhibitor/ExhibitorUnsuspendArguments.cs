using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class ExhibitorUnsuspendArguments : IArgumentsBase
	{
		public int Id { set; get; }

		#region Constructor
		public ExhibitorUnsuspendArguments(int id)
		{
			Id = id;
		}
		#endregion Constructor
	}
}
