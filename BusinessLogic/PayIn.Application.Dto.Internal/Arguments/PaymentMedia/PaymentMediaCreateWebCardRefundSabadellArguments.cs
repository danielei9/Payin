using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Internal.Arguments
{
	public class PaymentMediaCreateWebCardRefundSabadellArguments : IArgumentsBase
	{
		public string CommerceCode      { get; set; }
		public int PublicPaymentMediaId { get; set; }
		public int PublicTicketId       { get; set; }
		public int PublicPaymentId      { get; set; }
		public decimal Amount           { get; set; }
		public string Currency          { get; set; }
		public int OrderId              { get; set; }
		public int Terminal             { get; set; }

		#region Constructors
		public PaymentMediaCreateWebCardRefundSabadellArguments(string commerceCode, int publicPaymentMediaId, int publicTicketId, int publicPaymentId, decimal amount, string currency, int orderId, int terminal)
		{
			CommerceCode = commerceCode;
			PublicPaymentMediaId = publicPaymentMediaId;
			PublicTicketId = publicTicketId;
			PublicPaymentId = publicPaymentId;
			Amount = amount;
			Currency = currency;
			OrderId = orderId;
			Terminal = terminal;
		}
		#endregion Constructors
	}
}
