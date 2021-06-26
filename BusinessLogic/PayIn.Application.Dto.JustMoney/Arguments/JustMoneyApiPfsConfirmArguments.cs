using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.JustMoney.Arguments
{
	public class JustMoneyApiPfsConfirmArguments : IArgumentsBase
	{
		public string Data { get; set; }

		#region Constructors
		public JustMoneyApiPfsConfirmArguments(string data)
		{
			Data = data;
		}
		#endregion Constructors
	}
}
