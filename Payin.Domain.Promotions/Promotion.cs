using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Promotions
{
	public class Promotion : Entity
	{
		public string Name { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int Acumulative { get; set; }
		public PromotionState State { get; set; }

		#region Concession
		public int? ConcessionId { get; set; }
		public PaymentConcession Concession { get; set; }
		#endregion Concession

		#region PromoConditions
		[InverseProperty("Promotion")]
		public ICollection<PromoCondition> PromoConditions { get; set; }
		#endregion PromoConditions

		#region PromoActions
		[InverseProperty("Promotion")]
		public ICollection<PromoAction> PromoActions { get; set; }
		#endregion PromoActions

		#region PromoLaunchers
		[InverseProperty("Promotion")]
		public ICollection<PromoLauncher> PromoLaunchers { get; set; }
		#endregion PromoLaunhers

		#region PromoExecutions
		[InverseProperty("Promotion")]
		public ICollection<PromoExecution> PromoExecutions { get; set; }
		#endregion PromoExecutions

		#region PromoPrice
		[InverseProperty("Promotion")]
		public ICollection<PromoPrice> PromoPrices { get; set; }
		#endregion PromoPrice


		#region Constructors
		public Promotion()
		{
			PromoConditions = new List<PromoCondition>();
			PromoActions = new List<PromoAction>();
			PromoLaunchers = new List<PromoLauncher>();
			PromoExecutions = new List<PromoExecution>();
			PromoPrices = new List<PromoPrice>();
		}
		#endregion Constructors
	}
}
