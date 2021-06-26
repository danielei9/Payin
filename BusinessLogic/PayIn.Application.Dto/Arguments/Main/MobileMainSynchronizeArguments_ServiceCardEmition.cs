using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Arguments
{
	public class MobileMainSynchronizeArguments_ServiceCardEmition
    {
        public bool IsPrincipal { get; set; }
        public string Alias { get; set; } = "";
		public XpDateTime Date { get; set; }

        public int? OwnerUserId { get; set; }
        public int? Id { get; set; }
        public int? EventId { get; set; }
        public long CardUid { get; set; }
        public int LastSeq { get; set; }
        public IEnumerable<MobileMainSynchronizeArguments_Group> Groups { get; set; }
    }
}
