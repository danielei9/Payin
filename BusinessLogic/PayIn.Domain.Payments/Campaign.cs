using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class Campaign : Entity
	{
		[Required(AllowEmptyStrings = false)]
		public string Title { get; set; }
		[Required(AllowEmptyStrings = true)]
		public string Description { get; set; }
		public DateTime Since { get; set; }
		public DateTime Until { get; set; }
		public int? NumberOfTimes { get; set; }
		public CampaignState State { get; set; }
		public string PhotoUrl { get; set; }

		#region CampaignLines
		[InverseProperty("Campaign")]
		public ICollection<CampaignLine> CampaignLines { get; set; } = new List<CampaignLine>();
		#endregion CampaignLines

		#region Concession
		/// <summary>
		/// Id del dueño de la campaña
		/// </summary>
		public int ConcessionId { get; set; }
		/// <summary>
		/// Dueño de la campaña
		/// </summary>
		public PaymentConcession Concession { get; set; }
		#endregion Concession
		
		#region PaymentConcessionCampaigns
		/// <summary>
		/// Donde se puede pagar la campaña
		/// </summary>
		[InverseProperty("Campaign")]
		public ICollection<PaymentConcessionCampaign> PaymentConcessionCampaigns { get; set; } = new List<PaymentConcessionCampaign>();
		#endregion PaymentConcessionCampaigns

		#region TargetSystemCard
		public int? TargetSystemCardId { get; set; }
		[ForeignKey("TargetSystemCardId")]
		public SystemCard TargetSystemCard { get; set; }
		#endregion TargetSystemCard

		#region TargetConcession
		public int? TargetConcessionId { get; set; }
		[ForeignKey("TargetConcessionId")]
		public ServiceConcession TargetConcession { get; set; }
		#endregion TargetConcession

		#region TargetEvents
		public ICollection<Event> TargetEvents { get; set; } = new List<Event>();
		#endregion TargetEvents

		#region CampaignCodes
		public ICollection<CampaignCode> CampaignCodes { get; set; } = new List<CampaignCode>();
        #endregion CampaignCodes

        #region TicketLines
        [InverseProperty("Campaign")]
        public ICollection<TicketLine> TicketLines { get; set; } = new List<TicketLine>();
        #endregion TicketLines
    }
}

