using Xp.Application.Dto;

namespace PayIn.Application.Dto.Payments.Results
{
    public class AccountLineGetAllUnliquidatedResultBase : ResultBase<AccountLineGetAllUnliquidatedResult>
    {
        public decimal TotalAmount { get; set; }
        public decimal TotalCommission { get; set; }
    }
}
