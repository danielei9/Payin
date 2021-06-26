using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Transport
{
	public class GreyList : Entity
	{
		[Flags]
		public enum MachineType
		{
			Validation = 1,
			Charge = 2,
			ValidationAndRecharge = 3,
			Inspection = 4,
			InformationPosts = 8,
			All = 15,
			Read = 999
		}
		public enum ActionType
		{
			IncreaseBalance = 100,
			DiscountBalance = 101,
			ModifyBalance = 102,
			IncreaseUnities = 103,
			DiscountUnities = 104,
			ModifyStartDate = 105,
			ModifyExtensionBit = 106,
			ModifyField = 107,
			ModifyBlock = 108,
			EmitCard = 109,
			UnPersonalizeCard = 998,
			ReadAll = 999
		}
		public enum GreyListSourceType
		{
			SigAPunt = 1,
			Payin = 2,
            FgvValencia = 3,
            FgvAlacant = 4,
            PayFalles = 5
        }
		public enum GreyListStateType
		{
			Delete = 0,
			Active = 1
		}

		[Index("UidIndex")]
		public long Uid { get; set; }
		public int OperationNumber { get; set; }
		public DateTime RegistrationDate { get; set; }
		public ActionType Action { get; set; }
		public string Field { get; set; }
		public string NewValue { get; set; }
		public bool Resolved { get; set; }
		public DateTime? ResolutionDate { get; set; }
		public MachineType Machine { get; set; }
		public bool IsConfirmed { get; set; }
		public string CodeReturned { get; set; }
		public string OldValue { get; set; }
		public GreyListSourceType Source { get; set; }
		public GreyListStateType State { get; set; }
        public int? ObjectId { get; set; }

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
