using System;
using Xp.Common;

namespace PayIn.Application.Dto.Bus.Results
{
	public class BusMobileRequestGetByUserResult
	{
		public int Id { get; set; }

		// From
		public int FromId { get; set; }
		public string FromCode { get; set; }
		public string FromName { get; set; }
		public string FromLocation { get; set; }
		public TimeSpan? FromTime { get; set; }
		public XpDateTime FromVisitTimeStamp { get; set; }

		// To
		public int ToId { get; set; }
		public string ToCode { get; set; }
		public string ToName { get; set; }
		public string ToLocation { get; set; }
		public TimeSpan? ToTime { get; set; }
		public XpDateTime ToVisitTimeStamp { get; set; }
	}
}
