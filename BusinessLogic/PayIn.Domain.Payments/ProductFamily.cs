using PayIn.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class ProductFamily : Entity
	{
		[Required(AllowEmptyStrings = true)] public string Name					{ get; set; }
		[Required(AllowEmptyStrings = true)] public string Description			{ get; set; }
		[Required(AllowEmptyStrings = true)] public string PhotoUrl				{ get; set; }
											 public ProductFamilyState State	{ get; set; }
											 public bool IsVisible				{ get; set; }
        [Required(AllowEmptyStrings = true)] public string Code                 { get; set; }


        #region PaymentConcession
        public int? PaymentConcessionId { get; set; }
        [ForeignKey("PaymentConcessionId")]
        public PaymentConcession PaymentConcession { get; set; }
		#endregion PaymentConcession

		#region Products
		[InverseProperty("Family")]
		public ICollection<Product> Products { get; set; } = new List<Product>();
        #endregion Products

        #region SuperFamily
        public int? SuperFamilyId { get; set; }
        [ForeignKey("SuperFamilyId")]
        public ProductFamily SuperFamily { get; set; }
        #endregion SuperFamily

        #region SubFamilies
        [InverseProperty("SuperFamily")]
        public ICollection<ProductFamily> SubFamilies { get; set; } = new List<ProductFamily>();
		#endregion SubFamilies

		#region CampaignLine
		public ICollection<CampaignLine> CampaignLines { get; set; } = new List<CampaignLine>();
		#endregion CampaignLine
	}
}