using PayIn.Domain.Transport.Eige.Enums;
using System;
using System.Collections.Generic;
using Xp.Application.Dto;
using Xp.Application.Results;
using Xp.Common;

namespace PayIn.Application.Dto.Results
{
    public class ServiceCardReadInfoResultBase : ResultBase<ServiceCardReadInfoResult>
	{
        public string CardId { get; set; }
		public int? Owner { get; set; }
		public string OwnerName { get; set; } = "";
		public string TypeName { get; set; } = "";
		public string Alias { get; set; } = "";
		public XpDate ExpiredDate { get; set; }
		public bool InBlackList { get; set; }
		public bool IsExpired { get; set; }
		public bool IsRechargable { get; set; }
		public bool IsRevokable { get; set; }
		public decimal? RevokablePrice { get; set; }
		public bool HasHourValidity { get; set; }
		public bool HasDayValidity { get; set; }
		public bool ApproximateValues { get; set; }
		public bool IsPersonalized { get; set; }
		public bool IsDamaged { get; set; }
		public IEnumerable<ServiceCardReadInfoResult_Log> Logs { get; set; }
		public IEnumerable<ServiceCardReadInfoResult_Charge> Charges { get; set; }
		public IEnumerable<ServiceCardReadInfoResult_RechargeTitle> RechargeTitles { get; set; }
		public IEnumerable<ServiceCardReadInfoResult_RechargeTitle> ChargeTitles { get; set; }
        public IEnumerable<ServiceCardReadInfoResult_Group> Groups { get; set; }
        public IEnumerable<ServiceCardReadInfoResult_Entrance> Entrances { get; set; }
        public IEnumerable<ServiceCardReadInfoResult_Operation> Operations { get; set; }
        public long? UserCode { get; set; }
		public string UserName { get; set; } = "";
		public string UserSurname { get; set; } = "";
		public string UserDni { get; set; } = "";
		public string UserPhoto { get; set; } = "";
		public DateTime? LastValidationDate { get; set; } // Se usa DateTime y no xpDateTime para que no le asigne franja horaria
		public string LastValidationTypeName { get; set; } = "";
		public string LastValidationPlace { get; set; } = "";
		public string LastValidationOperator { get; set; } = "";
		public EigeZonaHistoricoEnum? LastValidationZone { get; set; }
		public string LastValidationTitleName { get; set; } = "";
		public string LastValidationTitleOwnerName { get; set; } = "";
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
		public int? LastRechargeOperationId { get; set; }
		public dynamic Operation { get; set; }
		public IEnumerable<ServiceCardReadInfoResult_Promotion> Promotions { get; set; }
		public IEnumerable<ServiceCardReadInfoResult_Concession> Concessions { get; set; }
		public MeanTransportEnum? MeanTransport { get; set; }

        public IEnumerable<ScriptResult> Scripts { get; set; }
    }
}
