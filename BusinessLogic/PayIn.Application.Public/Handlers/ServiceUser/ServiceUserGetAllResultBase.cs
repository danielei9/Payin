using System.Collections.Generic;
using PayIn.Application.Dto.Results;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.Payments.Results
{
	public class ServiceUserGetAllResultBase : ResultBase<ServiceUserGetAllResult>
	{
		public IEnumerable<SelectorResult> PaymentConcessions { get; set; }
		public int? PaymentConcessionId { get; set; }
		public string PaymentConcessionName { get; set; }
	}
}
