namespace PayIn.Application.Dto.Results
{
	public class MobileMainSynchronizeResult_ServiceCardEmited
    {
        public int Id { get; set; }
        public long Uid { get; set; }
        public string UidText { get; set; }
        public string Alias { get; set; }
        public int? OwnerUserId { get; set; }
        public int LastSeq { get; set; }
        public decimal LastBalance { get; set; }
		public decimal PendingBalance { get; set; }
    }
}