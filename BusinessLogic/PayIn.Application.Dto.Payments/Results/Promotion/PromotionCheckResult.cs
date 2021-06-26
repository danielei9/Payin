using PayIn.Domain.Transport.Eige.Enums;

namespace PayIn.Application.Dto.Payments.Results.Promotion
{
	public class PromotionCheckResult
	{
		public int Id { get; set; }
		public int Code { get; set; }
		public EigeZonaEnum? Zone { get; set; }
		public string Name { get; set; }
	}
}
