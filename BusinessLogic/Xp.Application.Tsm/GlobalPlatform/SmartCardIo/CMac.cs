using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Xp.Application.Tsm.GlobalPlatform.SmartCardIo
{
	public class CMac
	{
		public byte[] Calculate(byte[] key, byte[] data, int blockSize = 128)
		{
			using (Aes aes = Aes.Create())
			{
				aes.Key = key;
				aes.KeySize = blockSize;
				//aes.IV = IV;

				// Create a decrytor to perform the stream transform.
				var encryptor = aes.CreateEncryptor();
				
				using (MemoryStream buffer = new MemoryStream())
				{
					using (CryptoStream stream = new CryptoStream(buffer, encryptor, CryptoStreamMode.Write))
					{
						using (StreamWriter writer = new StreamWriter(stream))
						{
							writer.Write(data);
						}
					}
					return buffer.ToArray();
				}
			}
		}
	}
}
