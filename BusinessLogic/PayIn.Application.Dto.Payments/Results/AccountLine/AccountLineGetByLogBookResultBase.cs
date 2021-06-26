using Xp.Application.Dto;

namespace PayIn.Application.Dto.Payments.Results
{
    public partial class AccountLineGetByLogBookResultBase: ResultBase<AccountLineGetByLogBookResult>
    {
        public decimal TotalCash { get; set; }
        public decimal TotalServiceCard { get; set; }
        public decimal TotalCreditCard { get; set; }
        public decimal TotalProducts { get; set; }
        public decimal TotalEntrances { get; set; }
        public decimal TotalOthers { get; set; }
    }
}
