using PayIn.Domain.Transport.Eige.Enums;

namespace PayIn.Application.Dto.Payments.Results
{
	public class MobilePaymentMediaGetAllResult_PromotionPrice
	{
		public int Id { get; set; }
		public int Code { get; set; }
		public EigeZonaEnum? Zone { get; set; }
		public bool HasZone { get; set; }
		public string Name { get; set; }
	}
}
