using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.JustMoney.Arguments
{
	public class JustMoneyApiPrepaidCardShareFundsArguments : IArgumentsBase
	{
		           public int Id { get; set; }
		[Required] public decimal Amount { get; set; }
		           public string TargetCardHolderId { get; set; }
		           public string Password { get; set; }

		#region Constructors
		public JustMoneyApiPrepaidCardShareFundsArguments(decimal amount, string targetCardHolderId, string password)
		{
			Amount = amount;
			TargetCardHolderId = targetCardHolderId ?? "";
			Password = password;
		}
		#endregion Constructors
	}
}
