using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xp.Application.Tsm.GlobalPlatform.SmartCardIo
{
	public class Cipher
	{
		public const int ENCRYPT_MODE = 0;
		public const int DECRYPT_MODE = 0;

		public static Cipher GetInstance(string tipo)
		{
			return new Cipher();
		}
		public static Cipher GetInstance(string tipo, string subtipo)
		{
			return new Cipher();
		}
		//public void Init(int mode, SecretKeySpec secret)
		//{
		//}
		//public void Init(int mode, SecretKeySpec secret, IvParameterSpec iv)
		//{
		//}
		public byte[] DoFinal(byte[] key, byte[] data, byte[] iv /*, int offset, int length*/)
		{
			byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(message);
			byte[] keyArray = CreateHash(key);
			byte[] vectorArray = CreateHash(token);
			TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
			//set the secret key for the tripleDES algorithm
			tdes.Key = keyArray;
			//mode of operation. there are other 4 modes. We choose ECB(Electronic code Book)
			tdes.Mode = CipherMode.ECB;
			//padding mode(if any extra byte added)
			tdes.Padding = PaddingMode.None;

			ICryptoTransform cTransform = tdes.CreateEncryptor(keyArray, vectorArray);
			//transform the specified region of bytes array to resultArray
			byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
			//Release resources held by TripleDes Encryptor
			tdes.Clear();
			//Return the encrypted data into unreadable string format
			return Convert.ToBase64String(resultArray, 0, resultArray.Length);

			ICryptoTransform cTransform = tdes.CreateEncryptor(key, token);

			return data;
		}
		public byte[] DoFinal(byte[] data)
		{
			return data;
		}
	}
}
