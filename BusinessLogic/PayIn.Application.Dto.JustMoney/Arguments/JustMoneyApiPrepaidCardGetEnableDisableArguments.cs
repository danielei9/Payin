using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.JustMoney.Arguments
{
	public class JustMoneyApiPrepaidCardGetEnableDisableArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public JustMoneyApiPrepaidCardGetEnableDisableArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
