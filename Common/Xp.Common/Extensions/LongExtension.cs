namespace System
{
    public static class LongExtension
	{
		#region ToHexadecimal
		public static string ToHexadecimal(this long that)
		{
			var result = that.ToString("X");
			if (result.Length % 2 == 1)
				result = "0" + result;

			return result;
		}
		#endregion ToHexadecimal

		#region ToHexadecimalBE
		public static string ToHexadecimalBE(this long that)
		{
			var hexadecimal = that.ToHexadecimal();
			var result = "";
			for (var i = 0; i < hexadecimal.Length; i += 2 )
				result = hexadecimal.Substring(i, 2) + result;

			return result;
		}
		#endregion ToHexadecimalBE

		#region ToBase32
		public static string ToBase32(this long number)
		{
			var result = "";
			do
			{
				var remainder = (int)(number % 32);
				number = number / 32;

				result = StringExtension.SeedBase32[remainder] + result;

			} while (number > 0);

			return result;
		}
		#endregion ToBase32
	}
}
