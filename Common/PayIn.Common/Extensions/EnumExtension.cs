using System;
using System.Collections.Generic;
using System.Reflection;

namespace System
{
	public static class EnumExtension
	{
		#region ToEnumAlias
		public static string ToEnumAlias(this Enum that, string separator = ",")
		{
			if (that == null)
				return "";

			var resources = new PayIn.Common.Resources.Resources();
			if (that.GetType().GetTypeInfo().GetCustomAttribute<FlagsAttribute>() != null)
			{
				var result = new List<string>();
				foreach (Enum val in Enum.GetValues(that.GetType()))
				{
					if (that.HasFlag(val))
					{
						var value = resources.Enum.GetPropertyValue<string>(
							that.GetType().Name +
							"_" +
							val.ToString()
						);
						if (value.IsNullOrEmpty())
							value = val.ToString();
						result.Add(value);
					}
				}
				return result.JoinString(separator);
			}
			else
			{
				var value = resources.Enum.GetPropertyValue<string>(
					that.GetType().Name +
					"_" +
					that.ToString()
				);
				if (!value.IsNullOrEmpty())
					return value;
			}

			return that.ToString();
		}
        #endregion ToEnumAlias

        #region JoinString
        public static string JoinString(this Enum that, string separator = ",")
        {
            if (that == null)
                return "";

            var resources = new PayIn.Common.Resources.Resources();
            if (that.GetType().GetTypeInfo().GetCustomAttribute<FlagsAttribute>() != null)
            {
                var result = new List<string>();
                foreach (Enum val in Enum.GetValues(that.GetType()))
                {
                    if (that.HasFlag(val))
                        result.Add(val.ToString());
                }
                return result.JoinString(separator);
            }

            return that.ToString();
        }
        #endregion JoinString
    }
}
