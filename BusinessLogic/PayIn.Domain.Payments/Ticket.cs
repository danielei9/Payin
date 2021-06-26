using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class Ticket : Entity
	{
		[Required(AllowEmptyStrings = true)]  public string          Reference		{ get; set; }
		[Required(AllowEmptyStrings = true)]  public string          ExternalLogin	{ get; set; } = "";
		[Required(AllowEmptyStrings = true)]  public string          Observations	{ get; set; } = "";
		                                      public DateTime        Date			{ get; set; }
		                                      public decimal         Amount			{ get; set; }
		                                      public decimal         Vat			{ get; set; }
		                                      public decimal?        IncomeTax		{ get; set; }
		[Required(AllowEmptyStrings = false)] public string          SupplierName	{ get; set; }
		[Required(AllowEmptyStrings = false)] public string          TaxName		{ get; set; }
		[Required(AllowEmptyStrings = true)]  public string          TaxAddress		{ get; set; }
		[Required(AllowEmptyStrings = true)]  public string          TaxNumber		{ get; set; }
		                                      public TicketStateType State			{ get; set; }
		                                      public TicketType      Type			{ get; set; }
											  public DateTime        Since			{ get; set; }
											  public DateTime        Until			{ get; set; }
		[Required(AllowEmptyStrings = true)]  public string          TextUrl		{ get; set; }
		[Required(AllowEmptyStrings = true)]  public string          PdfUrl			{ get; set; }

		#region Payments
		[InverseProperty("Ticket")]
		public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        #endregion Payments

        #region Concession
        public int ConcessionId { get; set; }
		public PaymentConcession Concession { get; set; }
        #endregion Concession

        #region Event
        public int? EventId { get; set; }
        [ForeignKey(nameof(Ticket.EventId))]
        public Event Event { get; set; }
        #endregion Event

        #region PaymentUser
        public int? PaymentUserId { get; set; }
		public PaymentUser PaymentUser { get; set; }
		#endregion PaymentUser

		#region LiquidationConcession
		public int? LiquidationConcessionId { get; set; }
		public PaymentConcession LiquidationConcession { get; set; }
		#endregion LiquidationConcession

		#region Shipment
		public int? ShipmentId { get; set; }
		public Shipment Shipment { get; set; }
		#endregion Shipment

		#region PaymentWorker
		public int? PaymentWorkerId { get; set; }
		public PaymentWorker PaymentWorker { get; set; }
		#endregion PaymentWorker

		#region Template
		public int? TemplateId { get; set; }
		public TicketTemplate Template { get; set; }
		#endregion Template

		#region Lines
		[InverseProperty("Ticket")]
		public ICollection<TicketLine> Lines { get; set; } = new List<TicketLine>();
        #endregion Lines

        #region Recharges
        [InverseProperty("Ticket")]
		public ICollection<Recharge> Recharges { get; set; } = new List<Recharge>();
        #endregion Recharges

        #region ServiceOperations
        //[InverseProperty(nameof(ServiceOperation.Ticket))] // Bucle
        public ICollection<ServiceOperation> ServiceOperations { get; set; } = new List<ServiceOperation>();
        #endregion ServiceOperations

        #region AccountLines
        [InverseProperty(nameof(AccountLine.Ticket))]
        public ICollection<AccountLine> AccountLines { get; set; } = new List<AccountLine>();
        #endregion AccountLines

        #region Constructors
        public Ticket() { }
        public Ticket(
            int paymentConcessionId, string supplierName, string taxNumber, string taxName, string taxAddress,
            DateTime date, decimal amount, DateTime since, DateTime? until, TicketType type,
            string externalLogin = "", string reference = "", string observations = "",
            int? paymentWorkerId = null, int? liquidationConcessionId = null, int? eventId = null)
        {
            Date = date.ToUTC();
            Amount = amount;
            Since = since;
            Until = until ?? XpDateTime.MaxValue;
            Type = type;
            State =
                type == TicketType.Order ? TicketStateType.Created :
                TicketStateType.Active;
            TextUrl = "";
            PdfUrl = "";

            PaymentWorkerId = paymentWorkerId;
            ConcessionId = paymentConcessionId;
            SupplierName = supplierName;
            TaxName = taxName;
            TaxNumber = taxNumber;
            TaxAddress = taxAddress;
            
            ExternalLogin = externalLogin ?? "";
            Reference = reference ?? "";
            Observations = observations ?? "";

            LiquidationConcessionId = liquidationConcessionId ?? paymentConcessionId;
            EventId = eventId;
        }
        #endregion Constructors
    }
}