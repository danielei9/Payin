using PayIn.Domain.Transport;
using PayIn.Domain.Transport.Eige.Enums;

namespace PayIn.Application.Dto.Results
{
    public class ServiceCardReadInfoResult_RechargePrice
    {
        public int Id { get; set; }
        public EigeZonaEnum? Zone { get; set; }
        public string ZoneName { get; set; }
        public decimal Price { get; set; }
        public decimal ChangePrice { get; set; }
        public RechargeType RechargeType { get; set; }
        public EigeTituloEnUsoEnum? Slot { get; set; }
    }
}
