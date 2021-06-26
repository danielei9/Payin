namespace Xp.Application.Tsm.GlobalPlatform.Config
{
	public enum KeyType : byte
	{
		// DES in ECB mode
		DES_ECB = 0x83,
		// DES in CBC mode
		DES_CBC = 0x84,
		// AES in CBC mode
		AES_CBC = 0x88,
		// mother key used for derivation
		MOTHER_KEY = 0x00

		//public byte Value { get; private set; }

		//private KeyType(byte val)
		//{
		//	Value = val;
		//}
	}
}
