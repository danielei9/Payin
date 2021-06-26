using System;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Transport
{
	public class WhiteList : Entity
    {
        public enum WhiteListSourceType
        {
            SigAPunt = 1,
            Payin = 2,
            FgvValencia = 3,
            FgvAlacant = 4
        }
        public enum WhiteListStateType
        {
            Delete = 0,
            Active = 1
        }

        [Index("UidIndex")]
        public long Uid { get; set; }
        public WhiteListSourceType Source { get; set; }
        public WhiteListStateType State { get; set; }
        public WhiteListOperationType OperationType { get; set; }
		public int OperationNumber { get; set; }
		public WhiteListTitleType TitleType { get; set; }
		public decimal Amount { get; set; }
		public DateTime InclusionDate { get; set; }
		public DateTime? ExclusionDate { get; set; }
	}
}
