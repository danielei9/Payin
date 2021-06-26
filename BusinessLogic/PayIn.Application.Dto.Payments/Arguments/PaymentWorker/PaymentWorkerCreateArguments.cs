using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PaymentWorkerCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.paymentWorker.name")]  [Required(AllowEmptyStrings = false)] public string Name { get; set; }
		[Display(Name = "resources.paymentWorker.login")] [Required(AllowEmptyStrings = false)] public string Login { get; set; }

		#region Constructors
		public PaymentWorkerCreateArguments(string name, string login)
		{
			Name = name;
			Login = login;
		}
		#endregion Constructors
	}
}
