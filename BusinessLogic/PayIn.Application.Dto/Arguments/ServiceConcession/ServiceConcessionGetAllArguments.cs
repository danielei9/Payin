using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceConcession
{
	public partial class ServiceConcessionGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		[Display(Name = "")]
		public int? PaymentConcessionId { get; set; }

		#region Constructors
		public ServiceConcessionGetAllArguments(string filter, int? paymentConcessionId)
		{
			Filter = filter ?? "";
			PaymentConcessionId = paymentConcessionId;
		}
		#endregion Constructors
	}
}
