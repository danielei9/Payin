using Xp.Common;

namespace PayIn.Application.Dto.Arguments
{
	public class MobileMainSynchronizeArguments_ServiceCardReturned
    {
        public int? Id { get; set; }
        public long CardUid { get; set; }
        public int Seq { get; set; }
		public int? ESeq { get; set; }
		public XpDateTime Date { get; set; }
        public int? EventId { get; set; }
    }
}
