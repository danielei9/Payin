using System;
using System.ComponentModel;
using System.Globalization;

namespace Xp.Common.Net.Converters
{
	public class XpTimeConverter : TypeConverter
	{
		#region CanConvertFrom
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
				return true;
			if (sourceType == typeof(DateTime))
				return true;

			return base.CanConvertFrom(context, sourceType);
		}
		#endregion CanConvertFrom

		#region ConvertFrom
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value == null)
				return null;
			if (value is string)
				return XpTime.FromString(value as string);
			if (value is DateTime)
				return XpTime.FromDateTime((DateTime) value);
			return base.ConvertFrom(context, culture, value);
		}
		#endregion ConvertFrom
	}
}
