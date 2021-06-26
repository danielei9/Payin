using System.Collections.Generic;
using Xp.Application.Results;

namespace PayIn.Application.Dto.Internal.Results
{
	public partial class PaymentMediaCreateWebCardResult : IdResult
	{
		public int TicketId { get; set; }
		public string Request { get; set; }

		public string Verb { get; set; }
		public string Url { get; set; }
		public IDictionary<string, string> Arguments { get; set; }
	}
}
