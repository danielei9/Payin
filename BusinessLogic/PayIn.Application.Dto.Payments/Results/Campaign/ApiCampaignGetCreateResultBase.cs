using System.Collections.Generic;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.Payments.Results
{
    public class ApiCampaignGetCreateResultBase : ResultBase<ApiCampaignGetCreateResult>
    {
        public IEnumerable<SelectorResult> PaymentConcessionId { get; set; }
		public IEnumerable<SelectorResult> TargetSystemCardId { get; set; }
	}
}
