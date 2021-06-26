using System;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Domain.Transport.Eige.Types
{
	public class EigeInt16 : GenericType<int>
	{
		#region Value
		public override int Value
		{
			get
			{
				return Raw.ToInt16() ?? base.Value;
			}
		}
		#endregion Value

		#region Constructors
		public EigeInt16(byte[] value, int length)
			: base(value, length)
		{ }
		public EigeInt16(int value)
			: base(new byte[] {
				(byte)value,
				(byte)(value >> 8)
			}, 16)
		{ }
		#endregion Constructors
	}
}
