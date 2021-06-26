using System;
using Xp.Domain.Transport.MifareClassic;
	
namespace PayIn.Domain.Transport.Eige.Types
{
	public class EigeBytes : GenericType<byte[]>
	{
		#region Value
		public override byte[] Value
		{
			get
			{
				return Raw;
			}
		}
		#endregion region Value

		#region Constructors
		public EigeBytes(byte[] value, int length)
			: base(value, length)
		{ }
		#endregion Constructors

		#region Cast a String
		public static implicit operator string(EigeBytes value)
		{
			return value == null || value.Value == null ? "" : value.Value.ToHexadecimal();
		}
		#endregion Cast a String
	}
}
