using PayIn.Domain.Transport;
using PayIn.Domain.Transport.Eige.Enums;
using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Results
{
	public class ServiceCardReadInfoResult
    {
        public static string MaxQuantity = "MaxQuantity";
        public static string MaxAmpliation = "MaxAmpliation";
        public static string CardInBlackList = "CardInBlackList";
        public static string CardExpired = "CardExpired";
        public static string CardDamaged = "CardDamaged";
        public static string NotAllowed = "NotAllowed";

        public int? Code { get; set; }
		public SlotEnum Slot { get; set; }
		public string Name { get; set; }
		public string OwnerName { get; set; }
		public EigeZonaEnum? Zone { get; set; }
		public XpDate Caducity { get; set; }
		public bool IsRechargable { get; set; }
		public bool HasTariff { get; set; }
		public bool IsExhausted { get; set; }
		public bool IsExpired { get; set; }
		// Balance
		public bool HasBalance { get; set; }
		public decimal? Balance { get; set; }
        public decimal? BalanceAcumulated {get; set; }
		public string BalanceUnits { get; set; }
        // Temporal
        public bool IsTemporal { get; set; }
		public XpDateTime ExhaustedDate { get; set; } // Se usa DateTime y no xpDateTime para que no le asigne franja horaria
		public XpDateTime ActivatedDate { get; set; }
		public int? Ampliation { get; set; }
		public int? AmpliationQuantity { get; set; }
		public string AmpliationUnits { get; set; }
        // Recharges
        public ServiceCardReadInfoResult_RechargeTitle RechargeTitle { get; set; }
        public List<string> RechargeImpediments { get; set; }
        // Others
        public bool ReadAll { get; set; }
		public int? LastRechargeOperationId { get; set; }
		public dynamic Operation { get; set; }
		public MeanTransportEnum? MeanTransport { get; set; }
	}
}
