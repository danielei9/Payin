using System.Collections.Generic;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.Results.SystemCardMember
{
    public class SystemCardMemberGetAllResultBase : ResultBase<SystemCardMemberGetAllResult>
    {
        public int? SystemCardId { get; set; }
        public string SystemCardName { get; set; }
        public IEnumerable<SelectorResult> SystemCards { get; set; }
    }
}
