using PayIn.Common;
using PayIn.Domain.Public;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Results
{
	public class ServiceUserGetAllResult
	{
		public class Card
		{
			public enum ResultType
			{
				NotEmitted = 0,
				Principal = 1,
				Secondary = 2,
				Anonymous = 3,
				Linked = 4
			}

			public int Id { get; set; }
			//public long Uid { get; set; }
			public string UidText { get; set; }
			//public int LastSeq { get; set; }
			//public string ConcessionName { get; set; }
			public ServiceCardState State { get; set; }
			public string Alias { get; set; }
			public int BlackListCount { get; set; }
			public ResultType Type { get; set; }
			public string OwnerName { get; set; }
			//public ServiceCardRelationType Relation { get; set; }
		}

        public int Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string Login { get; set; }
		public string LastName { get; set; }
		public string VatNumber { get; set; }
		public IEnumerable<Card> Cards { get; set; }
		public ServiceUserState State { get; set; }
		public int ServiceGroupsCount { get; set; }
		public bool IsRegistered { get; set; }
		public bool IsEmailConfirmed { get; set; }
        //public IEnumerable<string> LinkedCards { get; set; }
    }
}
