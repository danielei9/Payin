using PayIn.Common;
using PayIn.Domain.Transport;
using System;
using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Transport.Results.GreyList
{
	public class GreyListGetAllResult
	{
		public int Id { get; set; }
		public long Uid { get; set; }
		public DateTime RegistrationDate { get; set; }
		public PayIn.Domain.Transport.GreyList.ActionType Action { get; set; }
		public string ActionAlias { get; set; }
		public string Field { get; set; }
		public string NewValue { get; set; }
		public bool Resolved { get; set; }
		public DateTime? ResolutionDate { get; set; }
		public PayIn.Domain.Transport.GreyList.MachineType Machine { get; set; }
		public string MachineAlias { get; set; }
		public PayIn.Domain.Transport.GreyList.GreyListSourceType Source { get; set; }
		public string SourceAlias { get; set; }
		public int OperationNumber { get; set; }	
	}
}
