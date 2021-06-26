using PayIn.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class Product : Entity
	{
													public string Name						{ get; set; }
		[Required(AllowEmptyStrings = true)]		public string PhotoUrl					{ get; set; }
		[Required(AllowEmptyStrings = true)]		public string Description				{ get; set; }
													public decimal? Price					{ get; set; }
													public ProductState State				{ get; set; }
													public bool IsVisible					{ get; set; }
        [Required(AllowEmptyStrings = true)]        public string Code						{ get; set; }
													public ProductVisibility Visibility		{ get; set; }
													public bool SellableInTpv				{ get; set; }
													public bool SellableInWeb				{ get; set; }


        #region Family
        public int? FamilyId { get; set; }
		[ForeignKey("FamilyId")]
		public ProductFamily Family { get; set; }
		#endregion Family

		#region PaymentConcession
		public int? PaymentConcessionId { get; set; }
		public PaymentConcession PaymentConcession { get; set; }
		#endregion PaymentConcession

		#region TicketLines
		[InverseProperty("Product")]
		public ICollection<TicketLine> TicketLines { get; set; } = new List<TicketLine>();
		#endregion TicketLines

		#region CampaignLine
		public ICollection<CampaignLine> CampaignLines { get; set; } = new List<CampaignLine>();
		#endregion CampaignLine
	}
}
