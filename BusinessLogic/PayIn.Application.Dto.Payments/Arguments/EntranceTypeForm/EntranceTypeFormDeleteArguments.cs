using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class EntranceTypeFormDeleteArguments : IArgumentsBase
	{
		public int Id { set; get; }

		#region Constructor
		public EntranceTypeFormDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructor
	}
}
