using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class ApiEventGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		[Display(Name = "")]
		public int? PaymentConcessionId { get; set; }

		#region Constructors
		public ApiEventGetAllArguments (string filter, int? paymentConcessionId)
		{
			Filter = filter ?? "";
			PaymentConcessionId = paymentConcessionId;
		}
		#endregion Constructors
	}
}
