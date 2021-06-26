using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
    public partial class ApiNoticeGetEdictsResult
	{
		public int Id { get; set; }
		public string Name { get; set; }
        public string ConcessionName { get; set; }

        public NoticeState State { get; set; }
        public XpDateTime Start { get; set; }
        public bool IsVisible { get; set; }
		public NoticeVisibility Visibility { get; set; }
	}
}
