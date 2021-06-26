using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Transport
{
	public class BlackList : Entity
	{
		public enum BlackListSourceType
		{
			SigAPunt = 1,
			Payin = 2,
            FgvValencia = 3,
            FgvAlacant = 4,
            PayFalles = 5,
            PayVilamarxant = 6
        }
		public enum BlackListStateType
		{
			Delete = 0,
			Active = 1
		}

		[Index("UidIndex")]
		public long Uid { get; set; }
		public DateTime RegistrationDate { get; set; }
		public bool Resolved { get; set; }
		public DateTime? ResolutionDate { get; set; }
		public BlackListMachineType Machine { get; set; }
		public bool Rejection { get; set; }
		public int Concession { get; set; }
		public BlackListServiceType Service { get; set; }
		public bool IsConfirmed { get; set; }
		public string CodeReturned { get; set; }
		public BlackListSourceType Source { get; set; }
		public BlackListStateType State { get; set; }

        #region TransportOperation
        public virtual ICollection<TransportOperation> TransportOperation { get; set; } = new List<TransportOperation>();
        #endregion TransportOperation

        #region SystemCard
        public int? SystemCardId { get; set; }
        [ForeignKey("SystemCardId")]
        public SystemCard SystemCard { get; set; }
        #endregion SystemCard
    }
}