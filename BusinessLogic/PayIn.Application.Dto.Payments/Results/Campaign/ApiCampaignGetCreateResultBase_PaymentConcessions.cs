using System.Collections.Generic;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.Payments.Results
{
    public class ApiCampaignGetCreateResultBase_PaymentConcessions : SelectorResult
    {
        public IEnumerable<SelectorResult> EntranceSystems { get; set; }
    }
}
