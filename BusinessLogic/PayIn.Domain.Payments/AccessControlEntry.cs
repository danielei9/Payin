using System;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
    public class AccessControlEntry : Entity
	{
		public DateTime EntryDateTime { get; set; }
		public int PeopleEntry { get; set; }
		public int CapacityAfterEntrance { get; set; }
		public string CreatorLogin { get; set; }

		#region AccessControlEntrance

		public int AccessControlEntranceId { get; set; }
        [ForeignKey("AccessControlEntranceId")]
        public AccessControlEntrance AccessControlEntrance { get; set; }

        #endregion
	}
}
