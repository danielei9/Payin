using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobileContactUnlockArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructor
		public MobileContactUnlockArguments(int id)
		{
			Id = id;
		}
		#endregion Constructor
	}
}
