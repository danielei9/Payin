using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.JustMoney.Arguments
{
	public class JustMoneyApiPrepaidCardRechargeCardArguments : IArgumentsBase
	{
		           public int Id { get; set; }
		[Required] public decimal Amount { get; set; }
		[Required] public string Password { get; set; }

		#region Constructors
		public JustMoneyApiPrepaidCardRechargeCardArguments(decimal amount, string password)
		{
			Amount = amount;
			Password = password;
		}
		#endregion Constructors
	}
}
