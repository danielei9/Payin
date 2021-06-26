using Xp.Common;

namespace PayIn.Application.Dto.Arguments
{
	public class MobileMainSynchronizeArguments_GreyList
	{
        public int Id { get; set; }
        public long CardUid { get; set; }
        public string NewValue { get; set; } = "";
        public string OldValue { get; set; } = "";
        public XpDateTime ResolutionDate { get; set; }
	}
}