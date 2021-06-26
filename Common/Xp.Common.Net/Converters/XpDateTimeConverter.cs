using System;
using System.ComponentModel;
using System.Globalization;

namespace Xp.Common.Net.Converters
{
	public class XpDateTimeConverter : TypeConverter
	{
		#region CanConvertFrom
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
				return true;

			return base.CanConvertFrom(context, sourceType);
		}
		#endregion CanConvertFrom

		#region ConvertFrom
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if ("Invalid date" == (value as string))
				return null;
			if (value == null)
				return null;
			if (value is string)
				return XpDateTime.FromString(value as string);
			if (value is DateTime)
				return XpDateTime.FromDateTime((DateTime) value);
			return base.ConvertFrom(context, culture, value);
		}
		#endregion ConvertFrom
	}
}
