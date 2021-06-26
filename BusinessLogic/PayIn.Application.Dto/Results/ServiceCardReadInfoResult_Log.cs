using PayIn.Domain.Transport.Eige.Enums;
using System;

namespace PayIn.Application.Dto.Results
{
    public class ServiceCardReadInfoResult_Log
    {
        public DateTime? Date { get; set; } // Se usa DateTime y no xpDateTime para que no le asigne franja horaria
        public string TypeName { get; set; }
        public string TitleName { get; set; }
        public string TitleOwnerName { get; set; }
        public EigeZonaEnum? TitleZone { get; set; }
        public int? Code { get; set; }
        public EigeZonaHistoricoEnum? Zone { get; set; }
        public decimal? Quantity { get; set; }
        public string QuantityUnits { get; set; }
        public bool HasBalance { get; set; }
        public string Place { get; set; }
        public string Operator { get; set; }
    }
}
