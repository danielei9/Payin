using PayIn.Application.Dto.Results;
using PayIn.Domain.Transport.Eige.Enums;
using System;
using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Transport.Results.TransportOperation
{
	public class TransportOperationGetDetailsResult
	{
		public class Historic
		{
			public XpDateTime DateTime { get; set; }
			public TimeSpan Duration { get; set; }
			public string Error { get; set; }
			public string Login { get; set; }
			public string RelatedClass { get; set; }
			public long? RelatedId { get; set; }
			public string RelatedMethod { get; set; }
		}

		public dynamic Values { get; set; }
		public string ScriptRequest { get; set; }
		public dynamic TuiN { get; set; }			
		public bool IsExpired { get; set; }	
		public string TypeName { get; set; }
		public dynamic Historical { get; set; }		
		public string Titulo1 { get; set; }
		public string Titulo2 { get; set; }
		public int? Owner { get; set; }
		public string OwnerName { get; set; }
		public bool InBlackList { get; set; }
		public XpDate ExpiredDate { get; set; }
		public bool IsDamaged { get; set; }
		public bool IsRechargable { get; set; }
		public bool IsRevokable { get; set; }
		public bool IsTuiN { get; set; }
		public bool HasHourValidity { get; set; }
		public bool HasDayValidity { get; set; }
		public bool IsPersonalized { get; set; }
		public string UserName { get; set; }
		public string UserSurname { get; set; }
		public string UserDni { get; set; }
		public long? UserCode { get; set; }
		public int? PeopleTraveling { get; set; }
		public DateTime? LastValidationDate { get; set; }
		public string LastValidationTypeName { get; set; }
		public string LastValidationPlace { get; set; }
		public string LastValidationOperator { get; set; }
		public EigeZonaHistoricoEnum? LastValidationZone { get; set; }
		public string LastValidationTitleName { get; set; }
		public string LastValidationTitleOwnerName { get; set; }
		public EigeZonaEnum? LastValidationTitleZone { get; set; }
        public int? PeopleInTransfer { get; set; }
		public int? MaxPeopleInTransfer { get; set; }
		public int? InternalTransfers { get; set; }
		public int? ExternalTransfers { get; set; }
		public int? MaxInternalTransfers { get; set; }
		public int? MaxExternalTransfers { get; set; }
		public int? Mode { get; set; }
		public EigeFechaPersonalizacion_DispositivoEnum? DeviceType { get; set; }
		public IEnumerable<ServiceCardReadInfoResult_Charge> Charges { get; set; }
		public IEnumerable<TransportOperationGetDetailsResultBase.GreyLists> GreyList { get; set; }
		public IEnumerable<TransportOperationGetDetailsResultBase.BlackLists> BlackList { get; set; }
		public IEnumerable<string> Device { get;  set; }
	}
}
