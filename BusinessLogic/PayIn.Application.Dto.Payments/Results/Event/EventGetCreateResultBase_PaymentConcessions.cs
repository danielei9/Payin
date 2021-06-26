using System.Collections.Generic;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.Payments.Results
{
    public class EventGetCreateResultBase_PaymentConcessions : SelectorResult
    {
        public IEnumerable<SelectorResult> EntranceSystems { get; set; }
    }
}
