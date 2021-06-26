using PayIn.Common;
using PayIn.Domain.Public;
using System.Collections.Generic;
using Xp.Application.Dto;
using Xp.Common;

namespace PayIn.Application.Dto.Results.ControlPresence
{
	public class ControlPresencePublicGetResult
	{
		public PresenceType Type         { get; set; }
		public int          WorkerId     { get; set; }
		public string       WorkerName   { get; set; }
		public int          ItemId       { get; set; }
		public string       ItemName     { get; set; }
		public decimal?     Latitude     { get; set; }
		public decimal?     Longitude    { get; set; }
		public XpDateTime   Date         { get; set; }
	}
}
