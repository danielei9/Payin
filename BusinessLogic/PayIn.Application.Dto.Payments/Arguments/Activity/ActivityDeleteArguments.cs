using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class ActivityDeleteArguments : IArgumentsBase
	{
		public int Id { set; get; }

		#region Constructor
		public ActivityDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructor
	}
}
