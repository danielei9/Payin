using System;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Domain.Transport.Eige.Types
{
	public class EigeMesDia : GenericType<DateTime?>
	{
		#region Value
		public override DateTime? Value
		{
			get
			{
				if ((Raw == null) || Raw.Length != 2)
					return base.Value;

				var fecha =
				 (Raw[1] << 8) +
				 (Raw[0]);
				if (fecha == 0)
					return null;
				
				var mes = (byte)fecha & 0x0F;
				var dia = (byte)(fecha >> 4) & 0x1F;

				DateTime dateTime;
				if (DateTime.TryParse("1900-" + mes + "-" + dia, out dateTime))
					return dateTime;

				return base.Value;
			}
		}
		#endregion Value

		#region Constructors
		public EigeMesDia(byte[] value, int length)
			: base(value, length)
		{ }
		#endregion Constructors
	}
}
