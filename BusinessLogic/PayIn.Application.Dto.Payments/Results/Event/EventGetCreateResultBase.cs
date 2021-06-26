using System.Collections.Generic;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.Payments.Results
{
    public class EventGetCreateResultBase : ResultBase<EventGetCreateResult>
    {
        public IEnumerable<EventGetCreateResultBase_PaymentConcessions> PaymentConcessionId { get; set; }
        public IEnumerable<SelectorResult> EntranceSystemId { get; set; }
    }
}
