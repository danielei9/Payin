using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Common.Resources;

namespace System.Reflection
{
	public static class PropertyInfoExtension
	{
		public static string GetName(this PropertyInfo field)
		{
			var result = "";
			if (field == null)
				return result;

			return field.Name.ToCamel();
		}
		public static string GetAlias(this PropertyInfo field)
		{
			if (field == null)
				return "";

			var attribute =
				field.GetCustomAttributes<DisplayAttribute>()
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
		public static XpType GetXpType(this PropertyInfo field)
		{
			if (field != null)
			{
				if (field.GetEnum() != null)
					return XpType.Enum;

				var dataType = field
					.GetCustomAttributes<DataTypeAttribute>()
					.Select(x => x.DataType)
					.FirstOrDefault();
				var dataSubTypes = field
					.GetCustomAttributes<DataSubTypeAttribute>()
					.SelectMany(x => x.DataSubType);

				if (dataType == DataType.Time)
					return XpType.Time;
				if (dataType == DataType.Date)
					return XpType.Date;
				if (dataType == DataType.Currency)
					return XpType.Currency;

				if (field.PropertyType == typeof(XpTime))
					return XpType.Time;
				if (field.PropertyType == typeof(XpDuration))
					return XpType.Duration;
				if (field.PropertyType == typeof(XpDate))
					return XpType.Date;
				if (field.PropertyType == typeof(XpDateTime))
					return XpType.DateTime;
				if (field.PropertyType == typeof(TimeSpan) || field.PropertyType == typeof(TimeSpan?))
					return XpType.Duration;
				if (field.PropertyType == typeof(DateTime) || field.PropertyType == typeof(DateTime?))
					return XpType.DateTime;
				if (field.PropertyType == typeof(decimal) || field.PropertyType == typeof(decimal?))
					return XpType.Decimal;
				if (field.PropertyType == typeof(bool) || field.PropertyType == typeof(bool?))
					return XpType.Bool;
				
				if (dataType == DataType.ImageUrl)
					return XpType.Image;
				if (field.PropertyType == typeof(byte[]))
				{
					if (dataSubTypes.Any(x => x == DataSubType.Zip))
						return XpType.Zip;
					else
						return XpType.File;
				}
				if (field.PropertyType == typeof(FileDto))
					return XpType.Zip;
			}

			return XpType.String;
		}
		public static Type GetEnum(this PropertyInfo field)
		{
			if (field != null)
			{
				if (field.PropertyType.IsEnum)
					return field.PropertyType;

				if (field.PropertyType.IsGenericType &&
					field.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) &&
					field.PropertyType.GetGenericArguments()[0].IsEnum)
					return field.PropertyType.GetGenericArguments()[0];
			}

			return null;
        }
        // Formatable
        public static bool IsFormatable(this PropertyInfo field)
        {
            var attribute = field.GetCustomAttributes<FormatableAttribute>().FirstOrDefault();
            return attribute != null;
        }
        // Multilanguage
        public static bool IsMultilanguage(this PropertyInfo field)
        {
            var attribute = field.GetCustomAttributes<MultilanguageAttribute>().FirstOrDefault();
            return attribute != null;
        }
        public static string GetMultilanguageModel(this PropertyInfo field)
        {
            var attribute = field.GetCustomAttributes<MultilanguageAttribute>().FirstOrDefault();
            return attribute?.Model ?? "";
        }
        public static string GetMultilanguageArguments(this PropertyInfo field)
        {
            var attribute = field.GetCustomAttributes<MultilanguageAttribute>().FirstOrDefault();
            return attribute?.Arguments ?? "";
        }
        // Required
        public static bool IsRequired(this PropertyInfo field)
		{
			var attribute = field.GetCustomAttributes<RequiredAttribute>().FirstOrDefault();
			return attribute != null;
		}
		public static string GetRequiredMessage(this PropertyInfo field)
		{
			var attribute = field.GetCustomAttributes<RequiredAttribute>().FirstOrDefault();
			var message = 
				attribute.ErrorMessage ??
				(
					attribute.ErrorMessageResourceType != null && !attribute.ErrorMessageResourceName.IsNullOrEmpty() ?
						attribute.ErrorMessageResourceType
							.GetPropertyValue<ResourceManager>("ResourceManager")
							.GetString(attribute.ErrorMessageResourceName) :
						field.GetPatternMessage() ??
						GlobalResources.ExceptionRequired.FormatString(field.GetAlias())
				);
			return message;
		}
		// Pattern
		public static string GetPattern(this PropertyInfo field)
		{
			var attribute = field.GetCustomAttributes<RegularExpressionAttribute>().FirstOrDefault();
			return attribute != null ? attribute.Pattern : "";
		}
		public static string GetPatternMessage(this PropertyInfo field)
		{
			var attribute = field.GetCustomAttributes<RegularExpressionAttribute>().FirstOrDefault();
			if (attribute == null)
				return null;
			var message =
			attribute.ErrorMessage ??
			(
				attribute.ErrorMessageResourceType != null && !attribute.ErrorMessageResourceName.IsNullOrEmpty() ?
					attribute.ErrorMessageResourceType
						.GetPropertyValue<ResourceManager>("ResourceManager")
						.GetString(attribute.ErrorMessageResourceName) :
					GlobalResources.ExceptionPattern.FormatString(field.GetAlias(), attribute.Pattern)
			);

			return message;
		}
		// MinLength
		public static int? GetMinLength(this PropertyInfo field)
		{
			var attribute = field.GetCustomAttributes<StringLengthAttribute>().FirstOrDefault();
			return attribute != null ? attribute.MinimumLength : (int?) null;
		}
		public static string GetMinLengthMessage(this PropertyInfo field)
		{
			var attribute = field.GetCustomAttributes<StringLengthAttribute>().FirstOrDefault();
			var message = 
				attribute.ErrorMessage ??
			(
				attribute.ErrorMessageResourceType != null && !attribute.ErrorMessageResourceName.IsNullOrEmpty() ?
					attribute.ErrorMessageResourceType
						.GetPropertyValue<ResourceManager>("ResourceManager")
						.GetString(attribute.ErrorMessageResourceName) :
					GlobalResources.ExceptionBetweenLength.FormatString(field.GetAlias(), attribute.MaximumLength, attribute.MinimumLength)
			);
			
			return message;
		}
		// MaxLength
		public static int? GetMaxLength(this PropertyInfo field)
		{
			var attribute = field.GetCustomAttributes<StringLengthAttribute>().FirstOrDefault();
			return attribute != null ? attribute.MaximumLength : (int?)null;
		}
		public static string GetMaxLengthMessage(this PropertyInfo field)
		{
			var attribute = field.GetCustomAttributes<StringLengthAttribute>().FirstOrDefault();
			var message =
				attribute.ErrorMessage ??
			(
				attribute.ErrorMessageResourceType != null && !attribute.ErrorMessageResourceName.IsNullOrEmpty() ?
					attribute.ErrorMessageResourceType
						.GetPropertyValue<ResourceManager>("ResourceManager")
						.GetString(attribute.ErrorMessageResourceName) :
					GlobalResources.ExceptionMaximumLength.FormatString(field.GetAlias(), attribute.MaximumLength)
			);

			return message;
		}
		// Compare
		public static string GetCompareProperty(this PropertyInfo field)
		{
			var attribute = field.GetCustomAttributes<CompareAttribute>().FirstOrDefault();
			return attribute != null ? attribute.OtherProperty : null;
		}
		public static string GetCompareMessage(this PropertyInfo field)
		{
			var attribute = field.GetCustomAttributes<CompareAttribute>().FirstOrDefault();
			var message =
				attribute.ErrorMessage ??
			(
				attribute.ErrorMessageResourceType != null && !attribute.ErrorMessageResourceName.IsNullOrEmpty() ?
					attribute.ErrorMessageResourceType
						.GetPropertyValue<ResourceManager>("ResourceManager")
						.GetString(attribute.ErrorMessageResourceName) :
					GlobalResources.ExceptionCompare.FormatString(field.GetAlias(), attribute.OtherProperty)
			);

			return message;
		}

		public static string GetValidationHtmlAttributes(this PropertyInfo field)
		{
			var validations = new List<string>();
			if (field == null)
				return "";

			foreach (var attribute in field.GetCustomAttributes<ValidationAttribute>())
			{
				var length = attribute as StringLengthAttribute;
				var required = attribute as RequiredAttribute;
				var compare = attribute as CompareAttribute;

				if (field.IsRequired())
					validations.Add("required");
				if (field.GetMinLength() != null)
					validations.Add(string.Format("ng-minlength={0}", field.GetMinLength()));
				if (field.GetMaxLength() != null)
					validations.Add(string.Format("ng-maxlength={0}", field.GetMaxLength()));
				/*
				<input ng-model="{ string }"
					name="{ string }"
					required
					ng-required="{ boolean }"
					ng-minlength="{ number }"
					ng-maxlength="{ number }"
					ng-pattern="{ string }"
					ng-change="{ string }">
				</input>*/
			}

			return string.Join(" ", validations);
		}
		public static string GetPostHtml(this PropertyInfo field)
		{
			var results = new List<string>();
			if (field == null)
				return "";
			results.Add("<div>");

			foreach (var attribute in field.GetCustomAttributes<ValidationAttribute>())
			{
				var length = attribute as StringLengthAttribute;
				var required = attribute as RequiredAttribute;
				var compare = attribute as CompareAttribute;
				//var email = attribute as EmailAddressAttribute;

				if (required != null)
				{
					var mensaje = !required.ErrorMessage.IsNullOrEmpty() ?
						required.ErrorMessage :
						GlobalResources.ExceptionRequired.FormatString(field.GetAlias());
					results.Add(string.Format("<div ng-show=\"form.{0}.$error.required\"><span class=\"error control-label\">{1}</span></div>", field.GetName(), mensaje));
				}
				else if (length != null)
				{
					if (length.MinimumLength > 0)
					{
						var mensaje = !length.ErrorMessage.IsNullOrEmpty() ?
							length.ErrorMessage :
							GlobalResources.ExceptionBetweenLength.FormatString(field.GetAlias(), length.MaximumLength, length.MinimumLength);
						results.Add(string.Format("<div ng-show=\"form.{0}.$error.minlength\"><span class=\"error control-label\">{1}</span></div>", field.GetName(), mensaje));
					}
					else
					{
						var mensaje = !length.ErrorMessage.IsNullOrEmpty() ?
							length.ErrorMessage :
							GlobalResources.ExceptionMaximumLength.FormatString(field.GetAlias(), length.MaximumLength);
						results.Add(string.Format("<div ng-show=\"form.{0}.$error.maxlength\"><span class=\"error control-label\">{1}</span></div>", field.GetName(), mensaje));
					}
				}
				else if (compare != null)
				{
					var mensaje = !compare.ErrorMessage.IsNullOrEmpty() ?
						compare.ErrorMessage :
						GlobalResources.ExceptionCompare.FormatString(field.GetAlias(), compare.OtherProperty);
					results.Add(string.Format("<div ng-show=\"form.{0}.$error.match\"><span class=\"error control-label\">{1}</span></div>", field.GetName(), mensaje));
				}
				//else if (email != null)
				//{
				//	var mensaje = !email.ErrorMessage.IsNullOrEmpty() ?
				//		email.ErrorMessage.FormatString(field.GetAlias()) :
				//		GlobalResources.ExceptionEmail.FormatString(field.GetAlias());
				//	results.Add(string.Format("<div ng-show=\"form.{0}.$error.email\"><span class=\"error control-label\">{1}</span></div>", field.GetName(), mensaje));
				//}
			}
			results.Add("</div>");

			return string.Join("\r\n", results);
		}

		public static string GetInputAttributes(this PropertyInfo field)
		{
			var validations = new List<string>();
			if (field == null)
				return "";

			bool hasCompression = false;
			if (field.PropertyType == typeof(byte[]))
			{
				foreach (var attribute in field.GetCustomAttributes<DataSubTypeAttribute>())
				{
					var tipos = new List<string>();
					foreach (var subType in attribute.DataSubType)
					{
						if (subType == DataSubType.Zip)
							hasCompression = true;

						if (subType == DataSubType.Image)
							tipos.Add(".png");
						if (subType == DataSubType.Pdf)
							tipos.Add(".pdf");						
					}
					validations.Add(string.Format("accept={0}", tipos.JoinString(",")));
				}
			}

			if (hasCompression && field.PropertyType == typeof(byte[]))
				validations.Add("multiple");
			
			if (field.PropertyType == typeof(string))
			{
				foreach (var attribute in field.GetCustomAttributes<ValidationAttribute>())
				{
					var length = attribute as StringLengthAttribute;
					var required = attribute as RequiredAttribute;
					var compare = attribute as CompareAttribute;
					var regularExpression = attribute as RegularExpressionAttribute;

					if (length != null)
					{
						validations.Add(string.Format("ng-minlength={0}", length.MinimumLength));
						validations.Add(string.Format("ng-maxlength={0}", length.MaximumLength));
					}
					if (required != null)
						validations.Add("required");
					if (compare != null)
						validations.Add(string.Format("xp-match={0}", "arguments." + compare.OtherProperty.ToCamel()));
					if (regularExpression != null)
						validations.Add(string.Format("ng-pattern=/{0}/", regularExpression.Pattern));
				}
			}

			return string.Join(" ", validations);
		}
		public static string GetInputType(this PropertyInfo field)
		{
			var result = "text";
			if (field == null)
				return result;

			if (field.PropertyType == typeof(DateTime) || field.PropertyType == typeof(DateTime?))
				return "datetime";
			else if (field.PropertyType == typeof(byte[]))
				return "file";
			else if (field.PropertyType == typeof(string))
			{
				var dataType = field
					.GetCustomAttributes<DataTypeAttribute>()
					.Select(x => x.DataType)
					.FirstOrDefault();

				var type = dataType.ToString().ToLower();
				if (type == "emailaddress")
					return "email";
				else if (type == DataType.Url.ToString())
					return "url";
				else
					return type;
			}
			if (field.PropertyType == typeof(bool) || field.PropertyType == typeof(bool?))
				return "checkbox";

			return result;
		}
		public static string GetInputClass(this PropertyInfo field)
		{
			var result = "";
			if (field == null)
				return result;

			if (field.PropertyType == typeof(bool) || field.PropertyType == typeof(bool?))
				return "checkbox";

			return result;
		}
	}
}