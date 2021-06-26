using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.PaymentUser	     
{
	public class PaymentUserResendNotificationArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public PaymentUserResendNotificationArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors

	}
}
