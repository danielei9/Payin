using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class EntranceTypeDeleteArguments : IArgumentsBase
	{
		public int Id { set; get; }

		#region Constructor
		public EntranceTypeDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructor
	}
}
