using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;
using Xp.Domain.Transport;

namespace PayIn.Domain.Public
{
	public class SystemCard : Entity
	{
		[Required(AllowEmptyStrings = true)]	public string				Name						{ get; set; }
												public CardType				CardType					{ get; set; }
												public NumberGenerationType	NumberGenerationType		{ get; set; }
		[Required(AllowEmptyStrings = true)]    public string               PhotoUrl                    { get; set; }
        [Required(AllowEmptyStrings = true)]    public string               AffiliationEmailBody        { get; set; } = "";
		                                        public TimeSpan?            SynchronizationInterval     { get; set; }
		[Required(AllowEmptyStrings = true)]    public string               ClientId                    { get; set; } = "";

		#region ConcessionOwner
		public int ConcessionOwnerId { get; set; }
		[ForeignKey(nameof(SystemCard.ConcessionOwnerId))]
		public ServiceConcession ConcessionOwner { get; set; }
        #endregion ConcessionOwner

        #region Cards
        [InverseProperty(nameof(ServiceCard.SystemCard))]
        public ICollection<ServiceCard> Cards { get; set; } = new List<ServiceCard>();
        #endregion Cards

        #region CardBatchs
        [InverseProperty("SystemCard")]
        public ICollection<ServiceCardBatch> CardBatchs { get; set; } = new List<ServiceCardBatch>();
        #endregion CardBatchs

        #region SystemCardMembers
        [InverseProperty("SystemCard")]
		public ICollection<SystemCardMember> SystemCardMembers { get; set; } = new List<SystemCardMember>();
		#endregion SystemCardMembers

		#region Profile
		public int? ProfileId { get; set; }
		[ForeignKey(nameof(SystemCard.ProfileId))]
		public Profile Profile { get; set; }
		#endregion Profile

		#region Documents
		[InverseProperty(nameof(ServiceDocument.SystemCard))]
		public ICollection<ServiceDocument> Documents { get; set; } = new List<ServiceDocument>();
		#endregion Documents

		// No se puede poner pq monta un ciclo
		//public ICollection<Purse> Purses { get; set; } = new List<Purse>();

		// No se puede poner pq monta un ciclo
		//public ICollection<TransportOperation> Operations { get; set; } = new List<TransportOperation>();
	}
}
