using PayIn.Common;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.Payments.Results
{
    public class AccountLineGetByLiquidationResultBase : ResultBase<AccountLineGetByLiquidationResult>
    {
        public decimal Total { get; set; }
        public LiquidationState? State { get; set; }
    }
}
