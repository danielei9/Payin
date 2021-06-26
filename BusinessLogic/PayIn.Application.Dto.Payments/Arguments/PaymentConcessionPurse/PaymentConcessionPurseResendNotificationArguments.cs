using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PaymentConcessionPurseResendNotificationArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public PaymentConcessionPurseResendNotificationArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors

	}
}
