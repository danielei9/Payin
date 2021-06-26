using System.Security.Cryptography;

namespace System
{
	public static class StringExtension
	{
		#region ToHash
		public static string ToHash(this string that)
		{
			var hashAlgorithm = new SHA256CryptoServiceProvider();
			var byteValue = System.Text.Encoding.UTF8.GetBytes(that);
			var byteHash = hashAlgorithm.ComputeHash(byteValue);

			return Convert.ToBase64String(byteHash);
		}
		#endregion ToHash
	}
}
