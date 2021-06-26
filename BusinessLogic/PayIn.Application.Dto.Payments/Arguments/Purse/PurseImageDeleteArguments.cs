using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.Purse
{
	public class PurseImageDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructor
		public PurseImageDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructor
	}
}
