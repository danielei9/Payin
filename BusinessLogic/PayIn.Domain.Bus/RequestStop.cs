using PayIn.Domain.Bus.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Bus
{
	public class RequestStop : Entity
	{
		public RequestNodeState State { get; set; } = RequestNodeState.Active;
		public DateTime? VisitTimeStamp { get; set; }

		#region Stop
		public int StopId { get; set; }
		[ForeignKey(nameof(StopId))]
		public Stop Stop { get; set; }
		#endregion Stop

		#region RequestStarts
		[InverseProperty(nameof(Request.To))]
		public ICollection<Request> RequestStarts { get; set; } = new List<Request>();
		#endregion RequestStarts

		#region RequestEnds
		[InverseProperty(nameof(Request.From))]
		public ICollection<Request> RequestEnds { get; set; } = new List<Request>();
		#endregion RequestEnds
	}
}