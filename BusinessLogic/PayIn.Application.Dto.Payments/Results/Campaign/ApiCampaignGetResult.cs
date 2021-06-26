using PayIn.Common;
using System;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class ApiCampaignGetResult
	{
		public int Id { set; get; }
		public string Title { set; get; }
		public string Description { set; get; }
		public XpDate Since { set; get; }
		public XpDate Until { set; get; }
		public int? NumberOfTimes { set; get; }
		public bool Started { set; get; }
		public bool Finished { get; set; }
		public string PhotoUrl { get; set; }
		public int? TargetConcessionId { get; set; }
		public string TargetConcessionName { get; set; }
		public int? TargetSystemCardId { get; set; }
		public string TargetSystemCardName { get; set; }
	}
}
