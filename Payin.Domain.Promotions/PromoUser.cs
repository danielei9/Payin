using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Promotions
{
	public class PromoUser : Entity
	{
		public string Login { get; set; }
		public int Attemps { get; set; }
		public DateTime? LastChance { get; set; }
		public DateTime? NextChance { get; set; }

		#region Promotions
		[InverseProperty("PromoUser")]
		public ICollection<PromoExecution> PromoExecutions { get; set; }
		#endregion Promotions				

		#region Constructors
		public PromoUser()
		{
			PromoExecutions = new List<PromoExecution>();			
		}
		#endregion Constructors
	}
}
