using PayIn.Common;

namespace PayIn.Application.Dto.Payments.Results.PaymentUser
{
	public partial class PaymentUserGetResult
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Login { get; set; }
		public int ConcessionId { get; set; }
		public PaymentUserState State { get; set; }
	}
}
