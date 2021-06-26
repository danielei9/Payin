using PayIn.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ServiceOperation : Entity
	{
		public ServiceOperationState State { get; set; }
		public DateTime Date { get; set; }
		public ServiceOperationType Type { get; set; }
		public int? Seq { get; set; }
		public int? ESeq { get; set; }

        //public Ticket Ticket { get; set; } // Bucle
        //public Event Event { get; set; } // Bucle
        //public ICollection<PurseValues> PurseValues { get; set; } = new List<PurseValues>();// Bucle

        #region Card
        public int CardId { get; set; }
		[ForeignKey("CardId")]
		public ServiceCard Card { get; set; }
        #endregion Card
        
        #region Constructors
        public ServiceOperation()
		{
		}
		public ServiceOperation(DateTime date, ServiceOperationType type, ServiceCard card, int? seq = null, int? eSeq = null)
		{
			State = ServiceOperationState.Active;
			Type = type;
			Date = date;
			Card = card;

			Seq = seq;
			ESeq = eSeq;
		}
		#endregion Constructors
	}
}
