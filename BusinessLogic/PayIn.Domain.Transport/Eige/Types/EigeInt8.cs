using System;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Domain.Transport.Eige.Types
{
	public class EigeInt8 : GenericType<int>
	{
		#region Value
		public override int Value
		{
			get
			{
				return Raw.ToInt8() ?? base.Value;
			}
		}
		#endregion Value

		#region Constructors
		public EigeInt8(byte[] value, int length)
			: base(value, length)
		{ }
		public EigeInt8(int value)
			: base(new byte[] { (byte)value }, 8)
		{ }
		#endregion Constructors
	}
}
