using PayIn.Domain.Transport.Eige.Enums;

namespace PayIn.Application.Dto.Results
{
    public class ServiceCardReadInfoResult_PromotionPrice
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Code { get; set; }
        public EigeZonaEnum? Zone { get; set; }
        public bool HasZone { get; set; }
    }
}
