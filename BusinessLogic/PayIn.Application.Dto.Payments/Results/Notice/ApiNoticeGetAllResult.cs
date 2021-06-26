using PayIn.Common;
using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class ApiNoticeGetAllResult
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public NoticeState State { get; set; }
		public bool IsVisible { get; set; }
		public NoticeVisibility Visibility { get; set; }
		public XpDateTime Start { get; set; }
        
        public int? SuperNoticeId { get; set; }
        public string EventName { get; set; }
        public string ConcessionName { get; set; }

        public IEnumerable<ApiNoticeGetAllResult> SubNotices { get; set; }
	}
}
