using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceUserGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }
		[Display(Name = "")]
		public int? PaymentConcessionId { get; set; }

		#region Constructors
		public ServiceUserGetAllArguments(string filter, int? paymentConcessionId)
		{
			Filter = filter ?? "";
			PaymentConcessionId = paymentConcessionId;
		}
		#endregion Constructors 
	}
}
