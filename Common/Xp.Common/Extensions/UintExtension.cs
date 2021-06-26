namespace System
{
	public static class UIntExtension
	{
		#region ToByteArray
		public static byte[] ToByteArray(this uint that)
		{
			var value = BitConverter.GetBytes(that);
			return value;
		}
		public static byte[] ToByteArray(this uint? that)
		{
			if (that == null)
				return new byte[0];

			var value = that.Value.ToByteArray();
			return value;
		}
		#endregion ToByteArray

		#region ToHexadecimal
		public static string ToHexadecimal(this uint that, int? numBytes = null)
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
		public static string ToHexadecimalBE(this uint that)
		{
			var hexadecimal = that.ToHexadecimal();
			var result = "";
			for (var i = 0; i < hexadecimal.Length; i += 2)
				result = hexadecimal.Substring(i, 2) + result;

			return result;
		}
		#endregion ToHexadecimalBE
	}
}
