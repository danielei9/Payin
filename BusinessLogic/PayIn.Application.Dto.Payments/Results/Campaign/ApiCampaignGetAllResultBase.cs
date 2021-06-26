using System.Collections.Generic;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.Payments.Results
{
	public class ApiCampaignGetAllResultBase : ResultBase<ApiCampaignGetAllResult>
	{
		public IEnumerable<SelectorResult> PaymentConcessions { get; set; }
		public int? PaymentConcessionId { get; set; }
		public string PaymentConcessionName { get; set; }
	}
}
