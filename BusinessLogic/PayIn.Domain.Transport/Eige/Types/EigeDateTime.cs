using System;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Domain.Transport.Eige.Types
{
	public class EigeDateTime : GenericType<DateTime?>
	{
		public readonly static string ZoneId = "Romance Standard Time";

		#region Zero
		public static EigeDateTime Zero
		{
			get
			{
				var dateTime = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Local);
				//var zone = TimeZoneInfo.FindSystemTimeZoneById(ZoneId);
				//var result = TimeZoneInfo.ConvertTimeToUtc(dateTime, zone).ToLocalTime();
				return new EigeDateTime(dateTime);
			}
		}
		#endregion Zero

		#region Value
		public override DateTime? Value
		{
			get
			{
				if ((Raw == null) || Raw.Length != 4)
					return base.Value;

				var fecha =
					 (Raw[3] << 24) +
					 (Raw[2] << 16) +
					 (Raw[1] << 8) +
					 (Raw[0]);
				if (fecha == 0)
					return null;

				var anyo = 2000 + ((byte)fecha & 0x1F);
				var mes = (byte)(fecha >> 5) & 0x0F;
				var dia = (byte)(fecha >> 9) & 0x1F;
				var horas = (byte)(fecha >> 14) & 0x1F;
				var minutos = (byte)(fecha >> 19) & 0x3F;

				try
				{
					var dateTime = new DateTime(anyo, mes, dia, horas, minutos, 0, DateTimeKind.Unspecified);
					var zone = TimeZoneInfo.FindSystemTimeZoneById(ZoneId);
					var result = TimeZoneInfo.ConvertTimeToUtc(dateTime, zone).ToLocalTime();
					return result;
				}
				catch
				{
					return null;
				}
				
			}
		}
		#endregion Value

		#region Constructors
		public EigeDateTime(byte[] value, int length)
			: base(value, length)
		{ }

		public EigeDateTime(DateTime? value)
			: base(value == null ?
				new byte[] { 0x0, 0x0, 0x0, 0x0 } :
				new byte[] {
					(byte)(
						((value.Value.Year - 2000) & 0x1F) |
						((value.Value.Month & 0x0F) << 5)
					), // MMMYYYYY
					(byte)(
						((value.Value.Month & 0x0F) >> 3) |
						((value.Value.Day & 0x1F) << 1) |
						((value.Value.Hour & 0x1F) << 6)
					),  // HHDDDDDM
					(byte)(
						((value.Value.Hour & 0x1F) >> 2) |
						((value.Value.Minute & 0x3F) << 3)
					),  // MMMMMHHH
					(byte)(
						((value.Value.Minute & 0x3F) >> 5)
					) // M
				},
				25
			)
		{ }

		#endregion Constructors
	}
}
