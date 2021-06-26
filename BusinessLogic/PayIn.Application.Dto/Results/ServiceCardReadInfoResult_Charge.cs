using PayIn.Domain.Transport.Eige.Enums;
using System;

namespace PayIn.Application.Dto.Results
{
    public class ServiceCardReadInfoResult_Charge
    {
        public DateTime? Date { get; set; } // Se usa DateTime y no xpDateTime para que no le asigne franja horaria
        public string TypeName { get; set; }
        public string TitleOwnerName { get; set; }
        public string TitleName { get; set; }
        public EigeZonaEnum? TitleZone { get; set; }
        public decimal? Quantity { get; set; }
        public MeanTransportEnum? MeanTransport { get; set; }
    }
}
