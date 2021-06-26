using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class UserMobileHasPaymentArguments : IArgumentsBase
	{
		[Required(AllowEmptyStrings = false)] public string Login { get; set; }

		#region Constructors
		public UserMobileHasPaymentArguments(string login)
		{
			Login = login;
		}
		#endregion Constructors
	}
}
