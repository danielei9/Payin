using System.Collections.Generic;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.Payments.Results
{
    public partial class EventGetResultBase : ResultBase<EventGetResult>
    {
        public IEnumerable<SelectorResult> EntranceSystemId { get; set; }
    }
}
