using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
    public class AccessControlEntrance : Entity
	{
		[Required(AllowEmptyStrings = false)]	public string Name { get; set; }

		#region AccessControl

		public int AccessControlId { get; set; }
        [ForeignKey("AccessControlId")]
        public AccessControl AccessControl { get; set; }

		#endregion

		#region Entries

		[InverseProperty("AccessControlEntrance")]
		public ICollection<AccessControlEntry> Entries { get; set; } = new List<AccessControlEntry>();

		#endregion
	}
}
