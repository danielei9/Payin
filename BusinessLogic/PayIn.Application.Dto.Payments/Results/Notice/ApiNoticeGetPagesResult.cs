using PayIn.Common;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Payments.Results
{
    public partial class ApiNoticeGetPagesResult
    {
        public int? SuperNoticeId { get; set; }
        public string SuperNoticeName { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ConcessionName { get; set; }
        public bool HasChild { get; set; }

        public string Index { get; set; }
        public int Level { get; set; }

        public NoticeState State { get; set; }
		public bool IsVisible { get; set; }
		public NoticeVisibility Visibility { get; set; }

        public IEnumerable<ApiNoticeGetPagesResult> SubNotices { get; set; }
    }
}
