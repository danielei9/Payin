using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using PayIn.Domain.Transport.Eige.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;
using Xp.Domain.Transport;

namespace PayIn.Domain.Transport
{
	public class TransportOperation : Entity
	{
		[Index("UidOperationDateIndex", Order = 1)]                               public long? Uid { get; set; }
		                                                                          public OperationType OperationType { get; set; }
		                                                                          public RechargeType? RechargeType { get; set; }
		[Index("OperationDateIndex")] [Index("UidOperationDateIndex", Order = 2)] public DateTime OperationDate { get; set; }
		                                                                          public DateTime? ConfirmationDate { get; set; }
		                                                                          public DateTime? DateTimeValue { get; set; }
		                                                                          public DateTime? DateTimeValueOld { get; set; }
		                                                                          public DateTime? DateTimeEventError { get; set; }
		                                                                          public decimal? QuantityValue { get; set; }
		                                                                          public decimal? QuantityValueOld { get; set; }
		                                                                          public string QuantityType { get; set; }
		[Required(AllowEmptyStrings = true)]                                      public string StringValue { get; set; } = "";
		[Required(AllowEmptyStrings = true)]                                      public string StringValueOld { get; set; } = "";
		[Required(AllowEmptyStrings = true)]                                      public string Error { get; set; } = "";
		[Required(AllowEmptyStrings = true)]                                      public string ScriptPrevious{ get; set; } = "";
		[Required(AllowEmptyStrings = true)]                                      public string Script { get; set; } = "";
		[Required(AllowEmptyStrings = true)]                                      public string ScriptResponse { get; set; } = "";
		                                                                          public string Login { get; set; } = "";
		                                                                          public string Device { get; set; } = "";
		                                                                          public int? TitleCodeErasedSlot1 { get; set; }
		                                                                          public int? TitleCodeErasedSlot2 { get; set; }
		                                                                          public bool GreyListUnmarked { get; set; } = false;
		[Required(AllowEmptyStrings = true)]                                      public string GreyListUnmarkedResult { get; set; } = "";
		                                                                          public bool BlackListUnmarked { get; set; } = false;
		[Required(AllowEmptyStrings = true)]                                      public string BlackListUnmarkedResult { get; set; } = "";
		                                                                          public EigeTituloEnUsoEnum? Slot { get; set; }
		                                                                          public bool? MobilisAmpliationBit { get; set; }
		                                                                          public EigeCodigoEntornoTarjetaEnum? MobilisEnvironment { get; set; }

		#region TransportPrice
		public int? TransportPriceId { get; set; }
		public TransportPrice Price { get; set; }
		#endregion TransportPrice

		#region Ticket	
		public int? TicketId { get; set; }
		public Ticket Ticket { get; set; }
		#endregion Ticket

		#region PromoExecution
		public int? PromoExecutionId { get; set; }
		#endregion PromoExecution

		#region GreyList
		public virtual ICollection<GreyList> GreyList { get; set; } = new List<GreyList>();
		#endregion GreyList

		#region BlackList		
		public virtual ICollection<BlackList> BlackList { get; set; } = new List<BlackList>();
		#endregion BlackList		

		#region OperationTitles
		[InverseProperty("Operation")]
		public ICollection<TransportOperationTitle> OperationTitles { get; set; } = new List<TransportOperationTitle>();
        #endregion OperationTitles

        #region SystemCard
        public int? SystemCardId { get; set; }
        [ForeignKey("SystemCardId")]
        public SystemCard SystemCard { get; set; }
        #endregion SystemCard

        #region Constructors
        public TransportOperation() { }
        public TransportOperation(Ticket ticket, long? uid, OperationType operationType, RechargeType? rechargeType, DateTime operationDate, DateTime? confirmationDate, string login, string device)
        {
            Ticket = ticket;
            Uid = uid;
            OperationType = operationType;
            RechargeType = rechargeType;
            OperationDate = operationDate;
            ConfirmationDate = confirmationDate;
            Login = login;
            Device = device;
        }
        #endregion Constructors
    }
}
