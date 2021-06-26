using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class Purse : Entity
	{
		public DateTime Validity { get; set; }
		public DateTime Expiration { get; set; }
		public string Name { get; set; }
		public PurseState State { get; set; }
		public string Image { get; set; }
		public int? Slot { get; set; }
		public bool IsPayin { get; set; } = false;
		public bool IsRechargable { get; set; } = false;

		#region Concession
		public int ConcessionId { get; set; }
		public PaymentConcession Concession { get; set; }
		#endregion Concession

		#region PaymentMedia		
		[InverseProperty("Purse")]
		public ICollection<PaymentMedia> PaymentMedias { get; set; } = new List<PaymentMedia>();
        #endregion PaymentMedia

        #region PaymentConcessionPurse
        [InverseProperty("Purse")]
		public ICollection<PaymentConcessionPurse> PaymentConcessionPurses { get; set; } = new List<PaymentConcessionPurse>(); 
		#endregion PaymentConcessionPurse

		#region CampaignLine
		[InverseProperty("Purse")]
		public ICollection<CampaignLine> CampaignLines { get; set; } = new List<CampaignLine>();
        #endregion CampaignLine

        // Monta ciclo
        //public ICollection<TransportOperationTitle> TransportOperationTitles { get; set; } = new List<TransportOperationTitle>();

        #region SystemCard
        public int? SystemCardId { get; set; }
        [ForeignKey("SystemCardId")]
        public SystemCard SystemCard { get; set; }
		#endregion SystemCard

		#region TicketLines
		[InverseProperty("Purse")]
		public ICollection<TicketLine> TicketLines { get; set; } = new List<TicketLine>();
		#endregion TicketLines

		#region PurseValues
		[InverseProperty("Purse")]
		public ICollection<PurseValue> PurseValues { get; set; } = new List<PurseValue>();
		#endregion PurseValues
	}
}