using Xp.Common;

namespace PayIn.Application.Dto.Results
{
	public class MobileMainSynchronizeResult_GreyList
    {
        public int Id { get; set; }
        public long CardUid { get; set; }
        public XpDateTime ResolutionDate { get; set; }
        public int Action { get; set; }
        public string Field { get; set; }
        public string NewValue { get; set; }
        public int? ObjectId { get; set; }
    }
}
