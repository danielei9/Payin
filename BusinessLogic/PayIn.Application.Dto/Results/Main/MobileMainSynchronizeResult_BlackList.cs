using Xp.Common;

namespace PayIn.Application.Dto.Results
{
	public class MobileMainSynchronizeResult_BlackList
    {
        public int Id { get; set; }
        public long Uid { get; set; }
        public bool Rejection { get; set; }
        public bool Resolved { get; set; }
        public XpDateTime ResolutionDate { get; set; }
        public XpDateTime RegistrationDate { get; set; }
    }
}
