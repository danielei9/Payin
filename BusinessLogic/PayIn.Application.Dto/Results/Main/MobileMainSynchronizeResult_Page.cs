using System.Collections.Generic;
using Xp.Application.Dto;
using Xp.Common;

namespace PayIn.Application.Dto.Results
{
	public class MobileMainSynchronizeResult_Page
	{
		public int? SuperNoticeId { get; set; }
		public int Id { get; set; }
		public string PhotoUrl { get; set; }
		public string Name { get; set; }
		public string ShortDescription { get; set; }
		public string Place { get; set; }
		public string Description { get; set; }
		public decimal? Longitude { get; set; }
		public decimal? Latitude { get; set; }
		public XpDateTime Start { get; set; }

		public IList<MobileMainSynchronizeResult_Page> SubNotices { get; set; } = new List<MobileMainSynchronizeResult_Page>();
		public IList<PoiResult> Pois { get; set; } = new List<PoiResult>();
	}
}
