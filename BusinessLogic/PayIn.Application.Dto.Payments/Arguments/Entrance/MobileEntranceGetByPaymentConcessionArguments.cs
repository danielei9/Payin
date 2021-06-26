using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class MobileEntranceGetByPaymentConcessionArguments : IArgumentsBase
    {
        public int PaymentConcessionId { get; set; }
    }
}