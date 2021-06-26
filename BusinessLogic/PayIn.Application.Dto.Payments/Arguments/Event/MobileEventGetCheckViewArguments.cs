using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobileEventGetCheckViewArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public MobileEventGetCheckViewArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
