namespace PayIn.Application.Dto.Transport.Arguments.TransportOperation
{
	public class TransportOperationRechargeArguments_PaymentMedia
	{
		public int Id { get; set; }
		public decimal Balance { get; set; }
		public int Order { get; set; }
	}
}
