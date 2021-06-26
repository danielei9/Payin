using PayIn.Common;
using System.Collections.Generic;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.Results.Main
{
	public partial class MainMobileGetEntailmentsResultBase : ResultBase<MainMobileGetEntailmentsResult>
	{
		public class PaymentUser
		{
			public int Id { get; set; }
			public PaymentUserState State { get; set; }
			public string ConcessionName { get; set; }
		}
		public class Campaign
		{
			public int Id { get; set; }
			public PaymentConcessionCampaignState State { get; set; }
			public string CampaignName { get; set; }
			public string ConcessionName { get; set; }
        }
		public class Purse
		{
			public int Id { get; set; }
			public PaymentConcessionPurseState State { get; set; }
			public string ConcessionName { get; set; }
			public string PurseName { get; set; }
        }

		public IEnumerable<PaymentUser> PaymentUsers { get; set; }
		public IEnumerable<Campaign> Campaigns { get; set; }
		public IEnumerable<Purse> Purses { get; set; }
	}
}
