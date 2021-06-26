using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class MobileEntranceGetBySystemCardArguments : IArgumentsBase
    {
        public int SystemCardId { get; set; }
    }
}