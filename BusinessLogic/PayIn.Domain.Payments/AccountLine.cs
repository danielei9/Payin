using PayIn.Common;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
    public class AccountLine : Entity
    {
        public AccountLineType Type { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public long? Uid { get; set; }
        public UidFormat? UidFormat { get; set; }

        #region Ticket
        public int TicketId { get; set; }
        [ForeignKey(nameof(AccountLine.TicketId))]
        public Ticket Ticket { get; set; }
        #endregion Ticket

        #region Concession
        public int? ConcessionId { get; set; }
        [ForeignKey(nameof(AccountLine.ConcessionId))]
        public PaymentConcession Concession { get; set; }
        #endregion Concession

        #region Liquidation
        public int? LiquidationId { get; set; }
        [ForeignKey(nameof(AccountLine.LiquidationId))]
        public Liquidation Liquidation { get; set; }
        #endregion Liquidation

        #region Constructors
        public AccountLine() { }
        public static AccountLine CreateServiceCard(string title, decimal amount, int concessionId, long uid, UidFormat uidFormat)
        {
            var result = new AccountLine
            {
                Type = AccountLineType.ServiceCard,
                Title = title,
                Amount = amount,
                ConcessionId = concessionId,
                Uid = uid,
                UidFormat = uidFormat
            };
            return result;
        }
        public static AccountLine CreateCash(string title, decimal amount, int concessionId)
        {
            var result = new AccountLine
            {
                Type = AccountLineType.Cash,
                Title = title,
                Amount = amount,
                ConcessionId = concessionId
            };
            return result;
        }
        public static AccountLine CreateProducts(string title, decimal amount, int concessionId)
        {
            var result = new AccountLine
            {
                Type = AccountLineType.Products,
                Title = title,
                Amount = amount,
                ConcessionId = concessionId
            };
            return result;
        }
        public static AccountLine CreateEntrances(string title, decimal amount, int concessionId)
        {
            var result = new AccountLine
            {
                Type = AccountLineType.Entrances,
                Title = title,
                Amount = amount,
                ConcessionId = concessionId
            };
            return result;
        }
        public static AccountLine Create(string title, decimal amount, int concessionId, long? uid, UidFormat? uidFormat)
        {
            var result = new AccountLine
            {
                Type = AccountLineType.Others,
                Title = title,
                Amount = amount,
                Uid = uid,
                UidFormat = uidFormat,
                ConcessionId = concessionId
            };
            return result;
        }
        #endregion Constructors
    }
}
