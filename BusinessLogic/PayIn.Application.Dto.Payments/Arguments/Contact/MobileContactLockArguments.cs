using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobileContactLockArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructor
		public MobileContactLockArguments(int id)
		{
			Id = id;
		}
		#endregion Constructor
	}
}
