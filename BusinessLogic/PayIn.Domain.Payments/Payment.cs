using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class Payment : IEntity
	{
		                                     public int          Id                { get; set; }
		[Required]                           public decimal      Amount            { get; set; }
		                                     public decimal      Payin             { get; set; }
		                                     public PaymentState State             { get; set; }
		                                     public DateTime     Date              { get; set; }
		[Required(AllowEmptyStrings = true)] public string       UserName          { get; set; }
		[Required(AllowEmptyStrings = true)] public string       UserLogin         { get; set; }
		[Required(AllowEmptyStrings = true)] public string       ExternalLogin     { get; set; }
		                                     public long?        Uid               { get; set; }
		                                     public UidFormat?   UidFormat         { get; set; }
		                                     public int?         Seq               { get; set; }
		                                     public int?         Order             { get; set; }
		[Required(AllowEmptyStrings = true)] public string       ErrorCode         { get; set; }
		[Required(AllowEmptyStrings = true)] public string       ErrorText         { get; set; }
		[Required(AllowEmptyStrings = true)] public string       ErrorPublic       { get; set; }
		[Required(AllowEmptyStrings = true)] public string       AuthorizationCode { get; set; }
		[Required(AllowEmptyStrings = true)] public string       CommerceCode      { get; set; } = "";
		[Required(AllowEmptyStrings = true)] public string       TaxName           { get; set; }
		[Required(AllowEmptyStrings = true)] public string       TaxAddress        { get; set; }
		[Required(AllowEmptyStrings = true)] public string       TaxNumber         { get; set; }
        
		#region PaymentMedia
		public int? PaymentMediaId { get; set; }
		public PaymentMedia PaymentMedia { get; set; }
		#endregion PaymentMedia
        
		#region Ticket
		public int TicketId { get; set; }
		public Ticket Ticket { get; set; }
		#endregion Ticket

		#region Recharges
		[InverseProperty("Payment")]	
		public ICollection<Recharge> Recharges { get; set; } = new List<Recharge>();
        #endregion Recharges

        #region Liquidation
        public int? LiquidationId { get; set; }
		public Liquidation Liquidation { get; set; }
		#endregion Liquidation

		#region RefundFrom
		public int? RefundFromId { get; set; }
		public Payment RefundFrom { get; set; }
		#endregion RefundFrom

		#region RefundTo
		[InverseProperty("RefundFrom")]
		public ICollection<Payment> RefundTo { get; set; } = new List<Payment>();
        #endregion RefundTo

        #region Constructors
        public Payment()
		{
        }
        public Payment(
            Ticket ticket, 
            decimal amount, decimal payin, DateTime date,
            string userName = "", string userLogin = "",  string externalLogin = "",
            string taxNumber = "",  string taxName = "", string taxAddress = "",
            string errorCode = "", string errorText = "", string errorPublic = "",
            long? uid = null, UidFormat? uidFormat = null, int? seq = null, string authorizationCode = "", string commerceCode = "", int? order = null,
            PaymentMedia paymentMedia = null, Liquidation liquidation = null, Payment refundFrom = null)
        {
			Amount = amount;
            Payin = payin;
            State = PaymentState.Active;
            Date = date;
            UserName = userName;
            UserLogin = userLogin;
            ExternalLogin = externalLogin;
            TaxNumber = taxNumber;
            TaxName = taxName;
            TaxAddress = taxAddress;
            ErrorCode = errorCode;
            ErrorText = errorText;
            ErrorPublic = errorPublic;
            Uid = uid;
            UidFormat = uidFormat;
            Seq = seq;
            AuthorizationCode = authorizationCode;
			CommerceCode = commerceCode;
            Order = order;
            // Relations
            Ticket = ticket ?? throw new ArgumentNullException("Ticket");
            PaymentMedia = paymentMedia;
            Liquidation = liquidation;
            RefundFrom = refundFrom;
        }
        #endregion Constructors
    }
}
