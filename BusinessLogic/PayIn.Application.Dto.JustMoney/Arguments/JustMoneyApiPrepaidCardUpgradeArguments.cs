using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.JustMoney.Arguments
{
	public class JustMoneyApiPrepaidCardUpgradeArguments : IArgumentsBase
	{
		           public int Id { get; set; }
		[Required] public string Password { get; set; }

		#region Constructors
		public JustMoneyApiPrepaidCardUpgradeArguments(string password)
		{
			Password = password;
		}
		#endregion Constructors
	}
}
