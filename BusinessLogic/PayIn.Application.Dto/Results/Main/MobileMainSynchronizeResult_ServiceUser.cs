using System.Collections.Generic;

namespace PayIn.Application.Dto.Results
{
	public class MobileMainSynchronizeResult_ServiceUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
		public bool HasPrimaryCard { get; set; }
        public int? CardId { get; set; }
        public long? CardUid { get; set; }
        public string CardAlias { get; set; }

        public IEnumerable<MobileMainSynchronizeResult_ServiceGroup> Groups { get; set; }
        public IEnumerable<MobileMainSynchronizeResult_ServiceCardEmited> OwnerCards { get; set; }
	}
}
