using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace System.Reflection
{
	public static class FieldInfoExtension
	{
		#region GetAlias
		public static string GetAlias(this FieldInfo field)
		{
			if (field == null)
				return "";

			var attribute = field
				.GetCustomAttributes<DisplayAttribute>()
				.FirstOrDefault();
			if (attribute == null)
				return field.Name;

			if (attribute.ResourceType == null)
				return attribute.Name;

			var result = (string)attribute.ResourceType
						.GetProperty(attribute.Name, BindingFlags.Static | BindingFlags.Public)
						.GetValue(null, null);
			return result;
		}
		#endregion GetAlias

		#region ToEnumAlias
		public static string ToEnumAlias(this FieldInfo field)
		{
			var resources = new PayIn.Common.Resources.Resources();
			var value = resources.Enum.GetPropertyValue<string>(
				field.FieldType.Name +
				"_" +
				field.Name
			);

			if (!value.IsNullOrEmpty())
				return value;

			return field.ToString();
		}
		#endregion ToEnumAlias
	}
}