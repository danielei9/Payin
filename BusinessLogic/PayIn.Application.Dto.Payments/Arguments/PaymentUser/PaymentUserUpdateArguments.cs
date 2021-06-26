using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.PaymentUser
{
	public class PaymentUserUpdateArguments : IArgumentsBase
	{
		                                                                                                public int Id { get; set; }
		[Display(Name = "resources.paymentUser.login")]		[DataType(DataType.EmailAddress)]		public string Login { get; set; }
		[Display(Name = "resources.paymentUser.name")]		                                        public string Name { get; set; }

		#region Constructor
		public PaymentUserUpdateArguments(int id, string name, string login)
		{
			Id = id;
			Name = name;
			Login = login;
		}
		#endregion Constructor
	}
}