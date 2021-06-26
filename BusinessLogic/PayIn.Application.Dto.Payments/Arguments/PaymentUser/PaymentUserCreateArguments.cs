using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;


namespace PayIn.Application.Dto.Payments.Arguments.PaymentUser
{
	public class PaymentUserCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.paymentUser.login")] [Required(AllowEmptyStrings = false)] [DataType(DataType.EmailAddress)] public string Login { get; set; }
		[Display(Name = "resources.paymentUser.name")]	[Required(AllowEmptyStrings = false)]									public string Name { get; set; }

		#region Constructor
		public PaymentUserCreateArguments(string login, string name)
		{
			Login = login;
			Name = name;		
		}
		#endregion Constructor
	}
}
