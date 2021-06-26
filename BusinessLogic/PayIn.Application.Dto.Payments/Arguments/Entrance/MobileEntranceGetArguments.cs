using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobileEntranceGetArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public MobileEntranceGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
