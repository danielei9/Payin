using PayIn.Domain.Transport.Eige.Enums;
using System;
using Xp.Common;

namespace PayIn.Application.Dto.Transport.Results.TransportOperation
{
	public class TransportOperationRevokeResult
	{
		public int? Code { get; set; }
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
		public decimal? BalanceAcumulated { get; set; }
		public string BalanceUnits { get; set; }
		// Temporal
		public bool IsTemporal { get; set; }
		public XpDateTime ExhaustedDate { get; set; } // Se usa DateTime y no xpDateTime para que no le asigne franja horaria
		public XpDateTime ActivatedDate { get; set; }
		public int? Ampliation { get; set; }
		public int? AmpliationQuantity { get; set; }
		public string AmpliationUnits { get; set; }
		public bool ReadAll { get; set; }
	}
}
