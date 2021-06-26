using PayIn.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class PaymentConcessionPurseGetAllResult
	{
		public int Id { get; set; }
		public PaymentConcessionPurseState State { get; set; }
		public int ConcessionId { get; set; }
		public string Concession { get; set; }
		public bool IsOwner { get; set; }
	}
}
