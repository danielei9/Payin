using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PaymentConcessionPurseCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.paymentConcessionPurse.login")]
		public string Login { get; set; }
		public int Id { get; set; }

		#region Constructors
		public PaymentConcessionPurseCreateArguments(string login, int id)
		{
			Login = login;
			Id = id;
		}

		#endregion Constructors
	}
}
