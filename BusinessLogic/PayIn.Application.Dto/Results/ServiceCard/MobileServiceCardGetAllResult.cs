using PayIn.Common;

namespace PayIn.Application.Dto.Results
{
	public class MobileServiceCardGetAllResult
	{
        public enum ResultType
        {
            NotEmitted = 0,
            Principal = 1,
            Secondary = 2,
            Anonymous = 3,
			Linked = 4
        }

		public enum RelationType
		{
		    Linked = 0,
			Owner = 1
        }

		public int Id { get; set; }
		public long Uid { get; set; }
        public string UidText { get; set; }
        public string Name { get; set; }
		public string LastName { get; set; }
        public string Alias { get; set; }
        public MobileServiceCardGetAllResult.RelationType Relation { get; set; }
        public MobileServiceCardGetAllResult.ResultType Type { get; set; }
        public int LastSeq { get; set; }
		public decimal LastBalance { get; set; }
		public decimal PendingBalance { get; set; }
		public bool InBlackList { get; set; }
		public string PhotoUrl { get; set; }
		//public long SystemCardId { get; set; }
		public string SystemCardName { get; set; }
	}
}
