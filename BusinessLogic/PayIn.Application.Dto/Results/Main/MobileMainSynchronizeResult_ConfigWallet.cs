namespace PayIn.Application.Dto.Results
{
	public class MobileMainSynchronizeResult_ConfigWallet
	{
		public int Id { get; set; }
		public int Slot { get; set; }
		public int ConcessionId { get; set; }
        public string Name { get; set; }
		public bool IsPayin { get; set; }
		public bool IsRechargable { get; set; }
    }
}
