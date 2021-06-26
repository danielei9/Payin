namespace Xp.Application.Dto.Tsm.GlobalPlatform
{
	public class TsmCryptograms
	{
		public byte[] CardCrypto { get; set; }
		public byte[] HostCrypto { get; set; }

		#region Constructors
		public TsmCryptograms()
		{
		}
		public TsmCryptograms(byte[] cardCrypto, byte[] hostCrypto)
		{
			CardCrypto = cardCrypto;
			HostCrypto = hostCrypto;
		}
		#endregion Constructors
	}
}
