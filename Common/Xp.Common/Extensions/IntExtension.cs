using System.Text;

namespace System
{
	public static class IntExtension
	{
		#region ToByteArray
		public static byte[] ToByteArray(this int that)
		{
			var value = BitConverter.GetBytes(that);
			return value;
		}
		public static byte[] ToByteArray(this int? that)
		{
			if (that == null)
				return new byte[0];

			var value = that.Value.ToByteArray();
			return value;
		}
		#endregion ToByteArray

		#region ToHexadecimal
		public static string ToHexadecimal(this int that, int? numBytes = null)
		{
			if (numBytes == 1)
				return that.ToString("X2").RightError(2);
			if (numBytes == 2)
				return that.ToString("X4").RightError(4);
			if (numBytes == 3)
				return that.ToString("X6").RightError(6);
			if (numBytes == 4)
				return that.ToString("X8").RightError(8);

			return that.ToString("X2");
		}
		#endregion ToHexadecimal

		#region ToHexadecimalBE
		public static string ToHexadecimalBE(this int that)
		{
			var hexadecimal = that.ToHexadecimal();
			var result = "";
			for (var i = 0; i < hexadecimal.Length; i += 2)
				result = hexadecimal.Substring(i, 2) + result;

			return result;
		}
		#endregion ToHexadecimalBE

		#region ToBase32
		public static string ToBase32(this int number)
		{
			var result = "";
			do
			{
				var remainder = number % 32;
				number = number / 32;

				result = StringExtension.SeedBase32[remainder] + result;

			} while (number > 0);

			var res = result.FromBase32ToInt();

			return result;
		}
		#endregion ToBase32

		#region ToBase32
		public static string ToBase32(this int? number)
		{
			if (number == null)
				return "";
			return ToBase32(number.Value);
		}
		#endregion ToBase32
	}
}
