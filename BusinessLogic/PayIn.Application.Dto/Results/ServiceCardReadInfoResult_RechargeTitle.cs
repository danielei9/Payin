using PayIn.Domain.Transport.Eige.Enums;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Results
{
    public class ServiceCardReadInfoResult_RechargeTitle
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public int PaymentConcessionId { get; set; }
        public int TransportConcession { get; set; }
        public string OwnerName { get; set; }
        public string OwnerCity { get; set; }
        public IEnumerable<ServiceCardReadInfoResult_RechargePrice> Prices { get; set; }
        public decimal? MaxQuantity { get; set; }
        public decimal? RechargeMax { get; set; }
        public decimal? RechargeMin { get; set; }
        public decimal? RechargeStep { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? QuantityInverse { get; set; }
        public bool AskQuantity { get; set; }
        public MeanTransportEnum? MeanTransport { get; set; }
    }
}
