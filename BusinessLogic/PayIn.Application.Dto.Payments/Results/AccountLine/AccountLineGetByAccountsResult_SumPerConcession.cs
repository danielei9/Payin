namespace PayIn.Application.Dto.Payments.Results
{
	public partial class AccountLineGetByAccountsResult_SumPerConcession
	{
        public decimal Amount { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
