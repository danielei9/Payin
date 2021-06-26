namespace Xp.Application.Dto.Tsm.GlobalPlatform
{
	public class TsmSession
	{
		public byte[] SessEnc { get; set; }
		public byte[] SessMac { get; set; }
		public byte[] SessRMac { get; set; }
		//public byte[] SessKek { get; set; }

		public ISessionState State { get; set; }

		public byte[] MacIcv { get; set; }
		public byte CEncCounter { get; set; } = 0x01;
		public byte REncCounter { get; set; } = 0x01;
	}
}
