using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
    public class Liquidation : IEntity
	{
		public int                Id             { get; set; }
		public decimal            TotalQuantity  { get; set; } // Sobra
		public decimal            PayinQuantity  { get; set; } // Sobra
        public decimal            PaidQuantity   { get; set; } // Sobra
        public LiquidationState   State          { get; set; }
		public DateTime           Since          { get; set; }
		public DateTime           Until          { get; set; }
		public DateTime?          PaymentDate    { get; set; }
		public DateTime?          RequestDate    { get; set; }
		public bool               PaidBank       { get; set; }
		public bool               PaidTPV        { get; set; }

		#region Payments
		[InverseProperty(nameof(Payment.Liquidation))]
		public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        #endregion Payments

        #region Recharges
        [InverseProperty(nameof(Recharge.Liquidation))]
		public ICollection<Recharge> Recharges { get; set; } = new List<Recharge>();
        #endregion Recharges

        #region Concession
        public int ConcessionId { get; set; }
		public PaymentConcession Concession { get; set; }
        #endregion Concession

        #region LiquidationConcession
        public int LiquidationConcessionId { get; set; }
        [ForeignKey(nameof(Liquidation.LiquidationConcessionId))]
        public PaymentConcession LiquidationConcession { get; set; }
        #endregion LiquidationConcession

        #region AccountLines
        [InverseProperty(nameof(AccountLine.Liquidation))]
        public ICollection<AccountLine> AccountLines { get; set; } = new List<AccountLine>();
        #endregion AccountLines

        #region Constructors
        public Liquidation() { }
        public Liquidation(IEnumerable<AccountLine> accountLines, int liquidationConcessionId, DateTime now)
        {
            if (accountLines.Count() == 0)
                throw new ApplicationException("No se pueden liquidar 0 apuntes");

            var concessionIds = accountLines
                .Select(x => x.ConcessionId)
                .Distinct();
            if (concessionIds.Count() > 1)
                throw new ApplicationException("No se pueden liquidar apuntes de más de 1 empresa");
            var concessionId = concessionIds.FirstOrDefault() ??
                throw new ApplicationException("No se pueden liquidar apuntes sin empresa asignada");

            foreach (var accountline in accountLines)
                AccountLines.Add(accountline);

            State = LiquidationState.Opened;
            TotalQuantity = AccountLines
                .Sum(x => x.Amount);
            PayinQuantity = 0;
            PaidQuantity = 0;
            Since = AccountLines
                .Min(x => x.Ticket.Date);
            Until = AccountLines
                .Max(x => x.Ticket.Date);
            RequestDate = now;
            PaidBank = false;
            PaidTPV = false;
            ConcessionId = concessionId;
            LiquidationConcessionId = liquidationConcessionId;
        }
        #endregion Constructors

        #region AddAccountLines
        public void AddAccountLines(IEnumerable<AccountLine> accountLines, DateTime now)
        {
            if (State != LiquidationState.Opened)
                throw new ApplicationException("Solo se pueden añadir apuntes a una liquidación abierta");
            if (accountLines.Count() == 0)
                throw new ApplicationException("No se pueden añadir 0 apuntes a la liquidación");
            var concessionIds = accountLines
                .Select(x => x.ConcessionId)
                .Distinct();
            if (concessionIds.Count() > 1)
                throw new ApplicationException("No se pueden liquidar apuntes de más de 1 empresa");
            var concessionId = concessionIds.FirstOrDefault() ??
                throw new ApplicationException("No se pueden liquidar apuntes sin empresa asignada");
            if (concessionId != ConcessionId)
                throw new ApplicationException("No se pueden añadir apunts de una empresa a una liquidación de otra");

            foreach (var accountline in accountLines)
                AccountLines.Add(accountline);

            TotalQuantity = AccountLines
                .Sum(x => x.Amount);
            Since = AccountLines
                .Min(x => x.Ticket.Date);
            Until = AccountLines
                .Max(x => x.Ticket.Date);
        }
        #endregion AddAccountLines
    }
}
