using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public class PublicCampaignGetAllWithLinesResult
	{
		public int Id { set; get; }
        public string Title { set; get; }
		public XpDateTime Since { set; get; }
		public XpDateTime Until { set; get; }

        public IEnumerable<PublicCampaignGetAllWithLinesResult_Line> Lines { get; set; }
	}
}
