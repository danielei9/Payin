using PayIn.Domain.Transport;
using PayIn.Domain.Transport.Eige.Enums;
using System;
using System.Collections.Generic;
using Xp.Application.Dto;
using Xp.Application.Results;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results.Campaign
{
	public class TransportCardReadInfoResultBase : ResultBase<TransportCardReadInfoResult>
	{
		public class Log
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
		public class Charge
		{
			public DateTime? Date { get; set; } // Se usa DateTime y no xpDateTime para que no le asigne franja horaria
			public string TypeName { get; set; }
			public string TitleOwnerName { get; set; }
			public string TitleName { get; set; }
			public EigeZonaEnum? TitleZone { get; set; }
			public decimal? Quantity { get; set; }
		}
		public class RechargePrice
		{
			public int Id { get; set; }
			public EigeZonaEnum? Zone { get; set; }
			public string ZoneName { get; set; }
			public decimal Price { get; set; }
		}
		public class RechargeTitle
		{
			public int Id { get; set; }
			public int Code { get; set; }
			public string Name { get; set; }
			public int PaymentConcessionId { get; set; }
			public string OwnerName { get; set; }
			public string OwnerCity { get; set; }
			public IEnumerable<RechargePrice> Prices { get; set; }
		}
		public string CardId { get; set; }
		public int? Owner { get; set; }
		public string OwnerName { get; set; }
		public string TypeName { get; set; }
		public XpDate ExpiredDate { get; set; }
		public bool InBlackList { get; set; }
		public bool IsExpired { get; set; }
		public bool IsRechargable { get; set; }
		public bool HasHourValidity { get; set; }
		public bool HasDayValidity { get; set; }
		public bool ApproximateValues { get; set; }
		public bool IsPersonalized { get; set; }
		public bool IsDamaged { get; set; }
		public IEnumerable<Log> Logs { get; set; }
		public IEnumerable<Charge> Charges { get; set; }
		public IEnumerable<RechargeTitle> RechargeTitles { get; set; }
		public IEnumerable<RechargeTitle> ChargeTitles { get; set; }
		public long? UserCode { get; set; }
		public string UserName { get; set; }
		public string UserSurname { get; set; }
		public string UserDni { get; set; }
		public DateTime? LastValidationDate { get; set; } // Se usa DateTime y no xpDateTime para que no le asigne franja horaria
		public string LastValidationTypeName { get; set; }
		public string LastValidationPlace { get; set; }
		public string LastValidationOperator { get; set; }
		public EigeZonaHistoricoEnum? LastValidationZone { get; set; }
		public string LastValidationTitleName { get; set; }
		public string LastValidationTitleOwnerName { get; set; }
		public EigeZonaEnum? LastValidationTitleZone { get; set; }
		public int? PeopleTraveling { get; set; }
		public int? PeopleInTransfer { get; set; }
		public int? MaxPeopleInTransfer { get; set; }
		public int? InternalTransfers { get; set; }
		public int? ExternalTransfers { get; set; }
		public int? MaxInternalTransfers { get; set; }
		public int? MaxExternalTransfers { get; set; }
		public int? Mode { get; set; }
		public EigeFechaPersonalizacion_DispositivoEnum? DeviceType { get; set; }
        public IEnumerable<ScriptResult> Scripts { get; set; }

    }
}
