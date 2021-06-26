using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

namespace Xp.Common
{
	/// <summary>
	/// Security class helpers
	/// https://bouncycastle.org/specifications.html
	/// </summary>
	public class XpSecurity
	{
		/// <summary>
		/// Cifrado DESede/CBC/NoPadding
		/// </summary>
		/// <param name="key"></param>
		/// <param name="data"></param>
		/// <param name="iv"></param>
		/// <returns></returns>
		public static byte[] Cypher_DESede_CBC_NoPadding(byte[] key, byte[] data, byte[] iv = null)
		{
			return Cypher(new CbcBlockCipher(new DesEdeEngine()), null, key, data, iv);
		}
		public static byte[] Cypher_DESede_ECB_NoPadding(byte[] key, byte[] data, byte[] iv = null)
		{
			return Cypher(new DesEdeEngine(), null, key, data, iv);
		}
		public static byte[] Cypher_DES_CBC_NoPadding(byte[] key, byte[] data, byte[] iv = null)
		{
			return Cypher(new CbcBlockCipher(new DesEngine()), null, key, data, iv);
		}
		public static byte[] Cypher_AES_CBC_NoPadding(byte[] key, byte[] data, byte[] iv = null)
		{
			return Cypher(new CbcBlockCipher(new AesEngine()), null, key, data, iv);
		}
		private static byte[] Cypher(IBlockCipher blockCipher, IBlockCipherPadding padding, byte[] key, byte[] data, byte[] iv = null)
		{
			var cipher = (padding == null) ?
				new BufferedBlockCipher(blockCipher) :
				new PaddedBufferedBlockCipher(blockCipher, padding);

			if (iv != null)
				cipher.Init(true, new ParametersWithIV(new KeyParameter(key), iv));
			else
				cipher.Init(true, new KeyParameter(key));

			byte[] outputBytes = new byte[cipher.GetOutputSize(data.Length)];
			int length = cipher.ProcessBytes(data, outputBytes, 0);
			cipher.DoFinal(outputBytes, length);

			return outputBytes;
		}

		
		public static byte[] Decrypt_AES_CBC_NoPadding(byte[] key, byte[] data, byte[] iv = null)
		{
			return Decrypt(new CbcBlockCipher(new AesEngine()), null, key, data, iv);
		}
		private static byte[] Decrypt(IBlockCipher blockCipher, IBlockCipherPadding padding, byte[] key, byte[] data, byte[] iv = null)
		{
			var cipher = (padding == null) ?
				new BufferedBlockCipher(blockCipher) :
				new PaddedBufferedBlockCipher(blockCipher, padding);

			if (iv != null)
				cipher.Init(false, new ParametersWithIV(new KeyParameter(key), iv));
			else
				cipher.Init(false, new KeyParameter(key));

			byte[] outputBytes = new byte[cipher.GetOutputSize(data.Length)];
			int length = cipher.ProcessBytes(data, outputBytes, 0);
			cipher.DoFinal(outputBytes, length);

			return outputBytes;
		}

		public static byte[] Mac_AES(byte[] key, byte[] data, byte[] iv = null, int? macSizeInBits = null)
		{
			return Mac(new AesEngine(), key, data, iv, macSizeInBits);
		}
		private static byte[] Mac(IBlockCipher engine, byte[] key, byte[] data, byte[] iv = null, int? macSizeInBits = null)
		{
			var macSizeInBits2 = macSizeInBits ?? (key.Length * 8);

			var cipher = new CMac(engine, macSizeInBits2);
			if (iv != null)
				cipher.Init(new ParametersWithIV(new KeyParameter(key), iv));
			else
				cipher.Init(new KeyParameter(key));

			byte[] outputBytes = new byte[cipher.GetMacSize()];
			cipher.BlockUpdate(data, 0, data.Length);
			cipher.DoFinal(outputBytes, 0);

			return outputBytes;
		}
	}
}
