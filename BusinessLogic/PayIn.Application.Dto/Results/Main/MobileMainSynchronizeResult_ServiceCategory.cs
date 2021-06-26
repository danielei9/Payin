using System.Collections.Generic;

namespace PayIn.Application.Dto.Results
{
	public class MobileMainSynchronizeResult_ServiceCategory
	{
        public int Id { get; set; }
        public string Name { get; set; }
		public bool AllMembersInSomeGroup { get; set; }
		public bool AMemberInOnlyOneGroup { get; set; }
		public bool AskWhenEmit { get; set; }
		public int? DefaultGroupWhenEmitId { get; set; }

		public IEnumerable<MobileMainSynchronizeResult_ServiceGroup> Groups { get; set; }
    }
}
