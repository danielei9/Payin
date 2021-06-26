using PayIn.Common;
using PayIn.Domain.Transport;
using System;
using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Transport.Results.BlackList
{
	public class BlackListGetAllResult
	{
		public int Id { get; set; }
		public long Uid { get; set; }
		public DateTime RegistrationDate { get; set; }
		public BlackListMachineType Machine { get; set; }
		public string MachineAlias { get; set; }
		public bool Resolved { get; set; }
		public DateTime? ResolutionDate { get; set; }
		public bool Rejection { get; set; }
		public BlackListServiceType Service { get; set; }
		public string ServiceAlias { get; set; }
	}
}
