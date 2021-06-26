using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class TicketLine : Entity
	{
		public string			 Title { get; set; }
		public decimal			 Amount { get; set; }
		public decimal			 Quantity { get; set; }
		public TicketLineType	 Type { get; set; }
		public CampaignLineType? CampaignLineType { get; set; }
		public decimal?          CampaignLineQuantity { get; set; }
        public long?             CampaignCode { get; set; }
        public long?             Uid { get; set; }
        
        #region Ticket
        public int TicketId { get; set; }
		public Ticket Ticket { get; set; }
		#endregion Ticket

		#region TransportPrice
		public int? TransportPriceId { get; set; }
		//public TransportPrice TransportPrice { get; set; }
		#endregion TransportPrice

		#region Entrances
		[InverseProperty("TicketLine")]
		public ICollection<Entrance> Entrances { get; set; } = new List<Entrance>();
		#endregion Entrances

		#region EntranceType
		public int? EntranceTypeId { get; set; }
		[ForeignKey("EntranceTypeId")]
		public EntranceType EntranceType { get; set; }
		#endregion EntranceType

		#region Product
		public int? ProductId { get; set; }
		[ForeignKey("ProductId")]
		public Product Product { get; set; }
        #endregion Product

        #region Campaign
        public int? CampaignId { get; set; }
        [ForeignKey("CampaignId")]
        public Campaign Campaign { get; set; }
        #endregion Campaign

        #region CampaignLine
        public int? CampaignLineId { get; set; }
		[ForeignKey("CampaignLineId")]
		public CampaignLine CampaignLine { get; set; }
		#endregion CampaignLine

		#region TicketLine From
		public int? FromId { get; set; }
		[ForeignKey("FromId")]
		public TicketLine From { get; set; }
		#endregion TicketLine From

		#region TicketLine To
		[InverseProperty("From")]
		public ICollection<TicketLine> To { get; set; } = new List<TicketLine>();
		#endregion TicketLine To

		#region Purse
		public int? PurseId { get; set; }
		[ForeignKey("PurseId")]
		public Purse Purse { get; set; }
		#endregion Purse
	}
}