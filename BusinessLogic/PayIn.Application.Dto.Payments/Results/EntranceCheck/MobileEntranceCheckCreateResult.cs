using PayIn.Common;
using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class MobileEntranceCheckCreateResult
	{
		public int Count { get; set; }
		public int Id { get; set; }
		public long Code { get; set; }
		public string VatNumber { get; set; }
		public string UserName { get; set; }
		public string LastName { get; set; }
		public XpDateTime TimeStamp { get; set; }
		public CheckInType Type { get; set; }
		// Para Razzmatazz
		public int EventId { get; set; }
		public string EventName { get; set; }
		public XpDateTime EventStart { get; set; }
		public XpDateTime EventEnd { get; set; }
		public int EntranceTypeId { get; set; }
		public string EntranceTypeName { get; set; }
		public string EntranceTypeCode { get; set; }
		public IEnumerable<string> Errors { get; set; }
		public XpDateTime LastCheckTimeStamp { get; set; }
		public CheckInType? LastCheckType { get; set; }
	}
}
