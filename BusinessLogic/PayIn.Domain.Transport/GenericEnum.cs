using System;
using System.Linq;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Domain.Transport
{
	public class GenericEnum<T> : GenericType<T>
	{
		#region Value
		public override T Value
		{
			get
			{
				var val = 0;
				for (var i = Raw.Length - 1; i >= 0; i--)
					val = (val << 8) + Raw[i];

				if (typeof(T).GetCustomAttributes(typeof(FlagsAttribute), false).Any()) // Flags
				{
					var result = 0L;
					foreach (var value in Enum.GetValues(typeof(T)))
						if (Enum.GetUnderlyingType(typeof(T)) == typeof(int) && (val & (int)value) != 0)
							result = result + (int)value;
						else if (Enum.GetUnderlyingType(typeof(T)) == typeof(long) && (val & (long)value) != 0)
							result = result + (long)value;

					return (T)Enum.ToObject(typeof(T), result);
				}
				else // No Flags
				{
					foreach (var value in Enum.GetValues(typeof(T)))
						if (Enum.GetUnderlyingType(typeof(T)) == typeof(int) && (int)value == val)
							return (T)value;
						else if (Enum.GetUnderlyingType(typeof(T)) == typeof(long) && (long)value == val)
							return (T)value;
				}

				return default(T);
			}
		}
		#endregion region Value

		#region Constructors
		public GenericEnum(byte[] value, int length)
			: base(value, length)
		{ }
		#endregion Constructors
	}
}
