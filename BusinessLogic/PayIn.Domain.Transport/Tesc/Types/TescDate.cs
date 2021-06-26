using System;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Domain.Transport.Tesc.Types
{
	public class TescDate : GenericType<DateTime?>
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

				var anyo = 2000 + ((byte)fecha & 0x1F);
				var mes = (byte)(fecha >> 5) & 0x0F;
				var dia = (byte)(fecha >> 9) & 0x1F;

				DateTime dateTime;
				if (DateTime.TryParse(anyo + "-" + mes + "-" + dia, out dateTime))
					return dateTime;

				return base.Value;
			}
		}
		#endregion Value

		#region Constructors
		public TescDate(byte[] value, int length)
			: base(value, length)
		{ }
		public TescDate(DateTime value)
			:base(new byte[]{
				(byte)(
					((value.Year - 2000) & 0x1F) | 
					((value.Month & 0x0F) << 5)
				), // MMMYYYYY
				(byte)(
					((value.Month & 0x0F) >> 3) |
					((value.Day & 0x1F) << 1)
				)  //   DDDDDM
			}, 14)
        { }
		#endregion Constructors
	}
}
