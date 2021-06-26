using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class PaymentMedia : Entity
	{
		[Required(AllowEmptyStrings = true)]		public string Name { get; set; }
		[Required(AllowEmptyStrings = true)]		public string NumberHash { get; set; }
													public int? ExpirationMonth { get; set; }
													public int? ExpirationYear { get; set; }
													public int? VisualOrder { get; set; }
													public int? VisualOrderFavorite { get; set; }
		[Required(AllowEmptyStrings = false)]		public string Login { get; set; }
		                                            public PaymentMediaState State { get; set; }
		[Required(AllowEmptyStrings = true)]		public string BankEntity { get; set; }
		                                            public PaymentMediaType Type { get; set; }
		[Required(AllowEmptyStrings = false)]		public string UserName { get; set; }
		[Required(AllowEmptyStrings = false)]		public string UserLastName { get; set; }
													public DateTime? UserBirthday { get; set; }
		[Required(AllowEmptyStrings = true)]		public string UserTaxNumber { get; set; }
		[Required(AllowEmptyStrings = true)]		public string UserAddress { get; set; }
		[Required(AllowEmptyStrings = true)]		public string UserPhone { get; set; }
		[Required(AllowEmptyStrings = false)]		public string UserEmail { get; set; }
													public bool Default { get; set; }

		#region Purse
		public int? PurseId { get; set; }		
		public Purse Purse { get; set; }
		#endregion Purse

		#region Payments
		[InverseProperty("PaymentMedia")]
		public ICollection<Payment> Payments { get; set; }
		#endregion Payments

		#region Recharges
		[InverseProperty("PaymentMedia")]
		public ICollection<Recharge> Recharges { get; set; }
		#endregion Recharges

		#region PaymentConcession
		public int? PaymentConcessionId { get; set; }
		[ForeignKey("PaymentConcessionId")]
		public PaymentConcession PaymentConcession { get; set; }
		#endregion PaymentConcession

		#region Constructors
		public PaymentMedia()
		{
			Payments = new List<Payment>();
			Recharges = new List<Recharge>();
		}
		#endregion Constructors
	}
}