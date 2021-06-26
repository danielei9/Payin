using System;
using System.Text;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Domain.Transport.Eige.Types
{
	public class EigeString : GenericType<string>
	{
		#region Value
		public override string Value
		{
			get
			{
				var result = new StringBuilder();

				foreach (var b in this.Raw)
					if (b != 0)
						result.Append(Convert.ToChar(b));

				return result.ToString().Trim();
			}
		}
		#endregion region Value

		#region Constructors
		public EigeString(byte[] value, int length)
			: base(value, length)
		{ }
		#endregion Constructors

		#region Cast a String
		public static implicit operator string(EigeString value)
		{
			return value == null ? "" : value.Value;
		}
		#endregion Cast a String
	}
}
