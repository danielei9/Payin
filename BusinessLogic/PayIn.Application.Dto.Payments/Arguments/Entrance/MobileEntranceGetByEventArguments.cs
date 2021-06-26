using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class MobileEntranceGetByEventArguments : IArgumentsBase
    {
        public int EventId { get; set; }
    }
}