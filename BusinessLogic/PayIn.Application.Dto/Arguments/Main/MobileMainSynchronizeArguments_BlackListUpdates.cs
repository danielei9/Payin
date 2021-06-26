using Xp.Common;

namespace PayIn.Application.Dto.Arguments
{
    public class MobileMainSynchronizeArguments_BlackListUpdates
    {
        public long Uid { get; set; }
        public bool Resolved { get; set; }
        public XpDateTime RegistrationDate { get; set; }
    }
}
