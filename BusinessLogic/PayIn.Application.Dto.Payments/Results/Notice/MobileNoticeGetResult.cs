using System.Collections.Generic;
using Xp.Application.Dto;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
    public partial class MobileNoticeGetResult
	{
		public int Id { get; set; }
		public string ShortDescription { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string PhotoUrl { get; set; }
		public XpDateTime Start { get; set; }

        public IEnumerable<MobileNoticeGetResult_SubNotice> SubNotices { get; set; }
        public IEnumerable<PoiResult> Pois { get; set; }
    }
}
