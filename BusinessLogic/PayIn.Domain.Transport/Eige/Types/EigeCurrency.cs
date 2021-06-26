using System;
using System.Collections.Generic;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Domain.Transport.Eige.Types
{
	public class EigeCurrency : GenericType<decimal>
	{
		#region Value
		public override decimal Value
		{
			get
			{
				var val = 0L;
				for (var i = Raw.Length - 1; i >= 0; i--)
					val = (val << 8) + Raw[i];

				return val / 100m;
			}
		}
		#endregion region Value

		#region Constructors
		public EigeCurrency(byte[] value, int length)
			: base(value, length)
		{ }
		public EigeCurrency(decimal value, int length)
			: base(null, length)
		{
			var numBytes = Convert.ToInt32(Math.Ceiling(length / 8m));
			Raw = new byte[numBytes];
			var val = Convert.ToInt64(Math.Floor(value * 100));
			
			for (var i = 0; i < numBytes; i++) {
				Raw[i] = (byte) (val >> (i*8));
			}
		}
		#endregion Constructors
	}
}
