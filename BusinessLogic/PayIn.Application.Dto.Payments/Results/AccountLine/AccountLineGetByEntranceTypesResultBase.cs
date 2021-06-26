using Xp.Application.Dto;

namespace PayIn.Application.Dto.Payments.Results
{
    public partial class AccountLineGetByEntranceTypesResultBase : ResultBase<AccountLineGetByEntranceTypesResult>
    {
        public decimal TotalRecharge { get; set; }
        public decimal TotalBuy { get; set; }
        public decimal TotalReturnCard { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalEntrance { get; set; }
        public decimal TotalExtraPrice { get; set; }
        public decimal TotalProduct { get; set; }
        public decimal Total { get; set; }
    }
}
