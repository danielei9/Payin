using PayIn.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
    public class ServiceCardBatch : Entity
	{
        public string Name { get; set; }
        public ServiceCardBatchState State { get; set; }
        public UidFormat UidFormat { get; set; }

        #region Concession
        public int ConcessionId { get; set; }
        [ForeignKey("ConcessionId")]
		public ServiceConcession Concession { get; set; }
		#endregion Concession

		#region SystemCard
		public int SystemCardId { get; set; }
        [ForeignKey("SystemCardId")]
        public SystemCard SystemCard { get; set; }
		#endregion SystemCard

		#region Cards
		[InverseProperty("ServiceCardBatch")]
        public ICollection<ServiceCard> Cards { get; set; } = new List<ServiceCard>();
		#endregion Cards
	}
}
