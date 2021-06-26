using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class CampaignLine : Entity
	{
		                   public CampaignLineType Type { get; set; }
		                   public decimal Min { get; set; }
		                   public decimal Max { get; set; }
		[Precision(18, 6)] public decimal Quantity { get; set; }
		                   public DateTime? SinceTime { get; set; }
		                   public DateTime? UntilTime { get; set; }
		                   public CampaignLineState State { get; set; }
		                   public bool AllProduct { get; set; }
		                   public bool AllEntranceType { get; set; }

		#region Purse
		public int? PurseId { get; set; }
		[ForeignKey("PurseId")]
		public Purse Purse { get; set; }
		#endregion Purse

		#region Campaign
		public int CampaignId { get; set; }
		public Campaign Campaign { get; set; }
		#endregion Campaign

		#region Recharges
		[InverseProperty("CampaignLine")]
		public ICollection<Recharge> Recharges { get; set; }
		#endregion Recharges

		#region Constructors
		public CampaignLine()
		{
			Recharges = new List<Recharge>();
		}
		#endregion Constructors

		#region EntranceType
		[InverseProperty("CampaignLines")]
		public ICollection<EntranceType> EntranceTypes { get; set; } = new List<EntranceType>();
		#endregion EntranceType

		#region Products
		[InverseProperty("CampaignLines")]
		public ICollection<Product> Products { get; set; } = new List<Product>();
		#endregion Products

		#region ProductFamily
		[InverseProperty("CampaignLines")]
		public ICollection<ProductFamily> ProductFamilies { get; set; } = new List<ProductFamily>();
        #endregion ProductFamily

        #region ServiceUsers
        [InverseProperty("CampaignLine")]
        public ICollection<CampaignLineServiceUser> ServiceUsers { get; set; } = new List<CampaignLineServiceUser>();
        #endregion ServiceUsers

        #region ServiceGroups
        [InverseProperty("CampaignLine")]
        public ICollection<CampaignLineServiceGroup> ServiceGroups { get; set; } = new List<CampaignLineServiceGroup>();
        #endregion ServiceGroups

        #region TicketLines
        [InverseProperty("CampaignLine")]
		public ICollection<TicketLine> TicketLines { get; set; } = new List<TicketLine>();
		#endregion TicketLines
	}
}