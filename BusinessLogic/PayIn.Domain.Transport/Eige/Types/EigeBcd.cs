using System;
using System.Text;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Domain.Transport.Eige.Types
{
	public class EigeBcd : GenericType<string>
	{
		#region Value
		public override string Value
		{
			get
			{
				var result = new StringBuilder();

				for (var i = 0; i < Raw.Length; i++)
				{
					var byte1 = Raw[i] & 0x0F;
					result.Append(Convert.ToInt16(byte1));

					var byte2 = Raw[i] >> 4;
					result.Append(Convert.ToInt16(byte2));
				}

				var cadena = result.ToString().Trim();
				while ((cadena.Length > 1) && (cadena[0] == '0') && ((cadena[1] != '.') || (cadena[1] != ',')))
					cadena = cadena.Substring(1);

				return cadena;
			}
		}
		#endregion region Value

		#region Constructors
		public EigeBcd(byte[] value, int length)
			: base(value, length)
		{ }
		#endregion Constructors

		#region Cast a String
		public static implicit operator string(EigeBcd value)
		{
			return value == null ? "" : value.Value;
		}
		#endregion Cast a String
	}
}