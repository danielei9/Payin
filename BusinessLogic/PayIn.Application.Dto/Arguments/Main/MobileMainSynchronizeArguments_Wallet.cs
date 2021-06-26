namespace PayIn.Application.Dto.Arguments
{
	public class MobileMainSynchronizeArguments_Wallet
	{
		public int OwnerId { get; set; }
		public decimal Balance { get; set; }
		public bool isRechargable { get; set; }
		public int Slot { get; set; }
	}
}
