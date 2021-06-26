using System.Text;
using System.Linq;

namespace System
{
	public static class ByteExtension
	{
		#region GetLittleEndian
		public static byte[] GetLittleEndian(this byte[] that, int start, int end)
		{
			// B15 B14 B13 B12 B11 B10 B9 B8 B7 B6 B5 B4 B3 B2 B1 B0

			var startByte = start / 8;
			var endByte = end / 8;

			var rightCut = start % 8;

			var shiftMask = (-end + start - 1) % 8;
			if (shiftMask < 0)
				shiftMask += 8;

			var numberBytes = (end - start) / 8 + 1;
			var result = new byte[numberBytes];
			int temp = 0;
			for (var i = 0; i < numberBytes; i++)
			{
				var index = startByte + i;
				if (that.Length > index)
					 temp = that[index];
				else
					 temp = 0;

				// Cortar por la derecha
				if ((index < 15) && (index + 1 < that.Length))
					temp |= that[index + 1] << 8;

				// Cortar por la derecha
				temp >>= rightCut;

				// Cortar mascara y cortar por la izquierda
				var mask = 0xFF;
				if (i == numberBytes - 1) // Ultima iteracion
					mask >>= shiftMask;
				temp &= mask;

				// Guardar
				result[i] = (byte)(temp);
			}

			return result;
		}
		#endregion GetLittleEndian

		#region SetLittleEndian
		public static void SetLittleEndian(this byte[] that, int start, int end, byte[] value)
		{
			// B15 B14 B13 B12 B11 B10 B9 B8 B7 B6 B5 B4 B3 B2 B1 B0

			var startByte = start / 8;
			var endByte = end / 8;

			var rightCut = start % 8;
			var leftCut = (8 - (end + 1) % 8) % 8;
			
			var numberBytes2 = (end - start) / 8 + 1;
			var numberBytes = endByte - startByte + 1; // (end - start) / 8 + 1;
			for (var i = 0; i < numberBytes; i++)
			{
				var index = startByte + i;
				byte tempValue = value.Length > i ? value[i] : (byte)0;

				// Cortar mascara
				byte mask = 0xFF;
				if (i == 0)
					mask = (byte)((mask >> rightCut) << rightCut);
				if (i == numberBytes - 1)
					mask = (byte)(((byte)(mask << leftCut)) >> leftCut);

				// Mover valor
				if (i == 0)
					tempValue = (byte)(tempValue << rightCut);
				else
				{
					var temp = (tempValue << 8) | value[i - 1];
					tempValue = (byte)(temp >> (8 - rightCut));
				}

				that[index] = (byte)(that[index] & ~mask); // Limpiar
				that[index] = (byte)(that[index] | tempValue); // Set Value
			}
		}
		#endregion SetLittleEndian

		#region ToString
		public static string ToString(this byte[] that)
		{
			char[] chars = new char[that.Length / sizeof(char)];
			Buffer.BlockCopy(that, 0, chars, 0, that.Length);
			return new string(chars);
		}
		#endregion ToString

		#region ToHexadecimal
		public static string ToHexadecimal(this byte that)
		{
			return that.ToString("X2");
		}
		public static string ToHexadecimal(this byte[] that)
		{
			if (that == null)
				return "";

			var result = new StringBuilder();
			for (int i = 0; i < that.Length; i++)
			{
				var temp = Convert.ToString(that[i], 16).ToUpper();
				if (temp.Length == 1)
					temp = "0" + temp;
				result.Append(temp);
			}

			return result.ToString();
		}
        #endregion ToHexadecimal

        #region ToInt64
        public static long? ToInt64(this byte[] that)
		{
			if ((that == null) || that.Length != 8)
				return null;

			var value =
					(((long)that[7]) << 56) +
					(((long)that[6]) << 48) +
					(((long)that[5]) << 40) +
					(((long)that[4]) << 32) +
					(((long)that[3]) << 24) +
					(((long)that[2]) << 16) +
					(((long)that[1]) << 8) +
					that[0];

			return value;
		}
		#endregion ToInt64

		#region ToInt48
		public static long? ToInt48(this byte[] that)
		{
			if ((that == null) || that.Length != 6)
				return null;

			var value =
					(((long)that[5]) << 40) +
					(((long)that[4]) << 32) +
					(((long)that[3]) << 24) +
					(((long)that[2]) << 16) +
					(((long)that[1]) << 8) +
					that[0];

			return value;
		}
		#endregion ToInt48

		#region ToInt32
		public static long? ToInt32(this byte[] that)
		{
			if ((that == null) || that.Length != 4)
				return null;

			long value =
					 (((long)that[3]) << 24) +
					 (((long)that[2]) << 16) +
					 (((long)that[1]) << 8) +
					 that[0];

			return value;
		}
		#endregion ToInt32

		#region ToInt24
		public static long? ToInt24(this byte[] that)
		{
			if ((that == null) || that.Length != 3)
				return null;

			long value =
					 (((long)that[2]) << 16) +
					 (((long)that[1]) << 8) +
					 that[0];

			return value;
		}
		#endregion ToInt24

		#region ToInt16
		public static int? ToInt16(this byte[] that)
		{
			if ((that == null) || that.Length != 2)
				return null;

			var value =
					 (that[1] << 8) +
					 (that[0]);

			return value;
		}
		#endregion ToInt16

		#region ToInt8
		public static int? ToInt8(this byte[] that)
		{
			if ((that == null) || that.Length != 1)
				return null;

			var value =
					 (that[0]);

			return value;
		}
		#endregion ToInt8

		#region ToBase64
		public static string ToBase64(this byte[] that)
		{
			return Convert.ToBase64String(that);
		}
		#endregion ToBase64

		#region ToUtf8
		public static string ToUtf8(this byte[] that, int index = 0, int? count = null)
		{
			count = count ?? that.Length;
			var result = new UTF8Encoding().GetString(that, index, count.Value);
			return result;
		}
		#endregion ToUtf8

		#region Reverse
		public static byte[] Reverse(this byte[] that)
		{
			var result = new byte[that.Length];
			for (var i = 0; i < that.Length; i++)
			{
				result[i] = that[that.Length - i - 1];
			}

			return result;
		}
		#endregion Reverse

		#region SumBits
		public static int SumBits(this byte that)
		{
			return
				((that & 1) == 1 ? 1 : 0) +
				((that & 2) == 2 ? 1 : 0) +
				((that & 4) == 4 ? 1 : 0) +
				((that & 8) == 8 ? 1 : 0) +
				((that & 16) == 16 ? 1 : 0) +
				((that & 32) == 32 ? 1 : 0) +
				((that & 64) == 64 ? 1 : 0) +
				((that & 128) == 128 ? 1 : 0);
		}
		public static int SumBits(this byte[] that)
		{
			return that
				.Select(x => x.SumBits())
				.Sum();
		}
		#endregion SumBits
	}
}