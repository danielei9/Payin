using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
    public class AccessControl : Entity
	{
		[Required(AllowEmptyStrings = false)]	public string Name { get; set; }
		[Required(AllowEmptyStrings = true)]	public string Schedule { get; set; }
												public string MapUrl { get; set; }
												public int CurrentCapacity { get; set; }
												public int MaxCapacity { get; set; }

		#region PaymentConcession

		public int PaymentConcessionId { get; set; }
        [ForeignKey("PaymentConcessionId")]
        public PaymentConcession PaymentConcession { get; set; }

		#endregion

		#region Entrances

		[InverseProperty("AccessControl")]
		public ICollection<AccessControlEntrance> Entrances { get; set; } = new List<AccessControlEntrance>();

		#endregion
	}
}
