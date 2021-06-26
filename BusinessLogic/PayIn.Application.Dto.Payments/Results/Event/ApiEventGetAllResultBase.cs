using System.Collections.Generic;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class ApiEventGetAllResultBase : ResultBase<ApiEventGetAllResult>
	{
		public IEnumerable<SelectorResult> PaymentConcessions { get; set; }
		public int? PaymentConcessionId { get; set; }
		public string PaymentConcessionName { get; set; }
	}
}