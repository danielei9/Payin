using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Purse
{
	public class PurseDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructor
		public PurseDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructor
	}
}
