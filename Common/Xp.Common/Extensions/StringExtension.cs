using Newtonsoft.Json;
using PCLWebUtility;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace System
{
	public static class StringExtension
	{
		#region LeftError
		public static string LeftError(this string that, int num)
		{
			return that.Length <= num ? that : that.Substring(that.Length - num);
		}
		#endregion LeftError

		#region Left
		public static string Left(this string that, int num)
		{
			return that.Length <= num ? that : that.Substring(0, num);
		}
		#endregion Left

		#region RightError
		public static string RightError(this string that, int num)
		{
			return that.Length <= num ? that : that.Substring(that.Length - num, num);
		}
		#endregion RightError

		#region Right
		public static string Right(this string that, int num)
		{
			return that.Length <= num ? that : that.Substring(that.Length - num, num);
		}
		#endregion Right

		#region ToCamel
		public static string ToCamel(this string that)
		{
			var camel = that.ToPascal();

			return camel.Substring(0, 1).ToLower() + camel.Substring(1);
		}
		#endregion ToCamel

		#region ToPascal
		public static string ToPascal(this string that)
		{
			var parts = that.Split(',');
			return string.Join("", parts
				.Select(x => x.Substring(0, 1).ToUpper() + x.Substring(1))
			);
		}
		#endregion ToPascal

		#region FormatString
		public static string FormatString(this string that, params object[] parameters)
		{
			return string.Format(that, parameters);
		}
		#endregion FormatString

		#region IsNullOrEmpty
		public static bool IsNullOrEmpty(this string that)
		{
			return string.IsNullOrEmpty(that);
		}
		#endregion IsNullOrEmpty

		#region SplitString
		public static string[] SplitString(this string that, string separator, StringSplitOptions options = StringSplitOptions.None)
		{
			return that.Split(new[] { separator }, options);
		}
		public static string[] SplitString(this string that, string separator, int count, StringSplitOptions options = StringSplitOptions.None)
		{
			return that.Split(new[] { separator }, count, options);
		}
		#endregion SplitString

		#region FromJson
		public static T FromJson<T>(this string that)
		{
			var result = JsonConvert.DeserializeObject<T>(that, new JsonSerializerSettings
			{
				ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
				NullValueHandling = NullValueHandling.Ignore,
				Formatting = Formatting.None
			});
			return result;
		}
		public static dynamic FromJson(this string that)
		{
			var result = JsonConvert.DeserializeObject(that, new JsonSerializerSettings
			{
				ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
				NullValueHandling = NullValueHandling.Ignore,
				Formatting = Formatting.None,
			});
			return result;
		}
		#endregion FromJson

		#region FromXml
		public static T FromXml<T>(this string that)
		{
			//var xDoc = XDocument.Parse(that);

			var serializer = new XmlSerializer(typeof(T));
			var stream = new MemoryStream(Encoding.UTF8.GetBytes(that));
			var result = (T)serializer.Deserialize(stream);
			
			return result;
		}
		#endregion FromXml

		#region ToBytes
		public static byte[] ToBytes(this string that)
		{
			var bytes = new byte[that.Length * sizeof(char)];
			Buffer.BlockCopy(that.ToCharArray(), 0, bytes, 0, bytes.Length);
			return bytes;
		}
		#endregion ToBytes

		#region FromUtf8
		public static byte[] FromUtf8(this string that)
		{
			var bytes = new UTF8Encoding().GetBytes(that);
			return bytes;
		}
		#endregion FromUtf8

		#region ToUrlEncode
		public static string ToUrlEncode(this string that)
		{
			var urlEncoded = WebUtility.UrlEncode(that);
			return urlEncoded;
		}
		#endregion ToUrlEncode

		#region ToBase64
		public static string ToBase64(this string that)
		{
			return that.FromUtf8().ToBase64();
		}
		#endregion ToBase64

		#region FromBase64
		public static byte[] FromBase64(this string that)
		{
			return Convert.FromBase64String(that);
		}
		#endregion FromBase64

		#region FromHexadecimal
		public static byte[] FromHexadecimal(this string that)
        {
			try
			{
				that = that.Replace(" ", "");
				if (that.Length % 2 == 1)
					that = "0" + that;

				byte[] raw = new byte[that.Length / 2];
				for (int i = 0; i < raw.Length; i++)
					raw[i] = Convert.ToByte(that.Substring(i * 2, 2), 16);

				return raw;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		#endregion FromHexadecimal

		#region FromHexadecimalBE
		public static byte[] FromHexadecimalBE(this string that)
		{
			that = that.Replace(" ", "");
			if (that.Length % 2 == 1)
				that = "0" + that;

			byte[] raw = new byte[that.Length / 2];
			for (int i = 0; i < raw.Length; i++)
				raw[i] = Convert.ToByte(that.Substring(that.Length - i * 2 - 2, 2), 16);

			return raw;
		}
		#endregion FromHexadecimalBE

		public static string SeedBase32 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

		#region FromBase32ToInt
		public static int FromBase32ToInt(this string value)
		{
			var result = 0;

			foreach(var v in value)
			{
				result *= 32;
				result += SeedBase32.IndexOf(v);
			}

			return result;
		}
		#endregion FromBase32ToInt

		#region FromBase32ToLong
		public static long FromBase32ToLong(this string value)
		{
			var result = 0L;

			foreach (var v in value)
			{
				result *= 32;
				result += SeedBase32.IndexOf(v);
			}

			return result;
		}
		#endregion FromBase32ToLong

		#region FromIsoString
		public static T FromIsoString<T>(this string that)
		{
				return default(T);
		}
		#endregion FromIsoString

		#region DeleteAccentMarks
		public static string DeleteAccentMarks(this string that)
		{
			that = Regex.Replace(that, "[áàâä]", "a");
			that = Regex.Replace(that, "[éèêë]", "e");
			that = Regex.Replace(that, "[íìîï]", "i");
			that = Regex.Replace(that, "[óòôö]", "o");
			that = Regex.Replace(that, "[úùûü]", "u");

			that = Regex.Replace(that, "[ÁÀÂÄ]", "A");
			that = Regex.Replace(that, "[ÉÈÊË]", "E");
			that = Regex.Replace(that, "[ÍÌÎÏ]", "I");
			that = Regex.Replace(that, "[ÓÒÔÖ]", "O");
			that = Regex.Replace(that, "[ÚÙÛÜ]", "U");

			return that;
		}
		#endregion DeleteAccentMarks

		#region DeleteCharactersExcept
		public static string DeleteCharactersExcept(this string that, string exceptions)
		{
			var template = "[^" + exceptions + "]+";
			that = Regex.Replace(that, template, "");

			return that;
		}
		#endregion DeleteCharactersExcept
	}
}