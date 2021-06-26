using PayIn.Domain.Transport;
using System;
using System.Collections.Generic;
using Xp.Application.Dto;
using Xp.Common;
using static PayIn.Domain.Transport.BlackList;
using static PayIn.Domain.Transport.GreyList;

namespace PayIn.Application.Dto.Transport.Results.TransportOperation
{
	public class TransportOperationGetDetailsResultBase : ResultBase<TransportOperationGetDetailsResult>
	{
		public int Id { get; set; }
		public long? Uid { get; set; }
		public XpDateTime Date { get; set; }
		public Xp.Domain.Transport.OperationType Action { get; set; }		
		public Dictionary<string, object> Raw { get; set; }
		public string Hexadecimal { get; set; }
		public IEnumerable<dynamic> Device { get; set; }
		public bool? TicketPay { get; set; }
		public string TicketPayError { get; set; }

		public class GreyLists
		{
			public int Id { get; set; }
			public DateTime RegistrationDate { get; set; }
			public ActionType Action { get; set; }
			public string Field { get; set; }
			public string NewValue { get; set; }
			public bool Resolved { get; set; }
			public DateTime? ResolutionDate { get; set; }
			public GreyListSourceType Source { get; set; }
			public int OperationNumber { get; set; }
			public long Uid { get; set; }
			public GreyListStateType State { get; set; }
		}

		public class BlackLists {
			public int Id { get; set; }
			public DateTime RegistrationDate { get; set; }
			public DateTime? ResolutionDate { get; set; }
			public bool Resolved { get; set; }
			public BlackListMachineType Machine { get; set; }
			public bool Rejection { get; set; }
			public BlackListServiceType Service { get; set; }
			public bool IsConfirmed { get; set; }
			public string CodeReturned { get; set; }
			public int Concession { get; set; }
			public BlackListSourceType Source { get; set; }

		}

		// RUBEN: De aquí para abajo sobra
		public string ScriptRequest { get; set; }	
	}
}
