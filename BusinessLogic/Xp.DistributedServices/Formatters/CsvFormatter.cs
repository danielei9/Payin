using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;

namespace Xp.DistributedServices.Formatters
{
	public class CsvFormatter : BufferedMediaTypeFormatter
	{
		private string FileName = "File.csv";

		public CsvFormatter(string fileName)
			: this()
		{
			FileName = fileName;
		}
		public CsvFormatter()
		{
			SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/csv"));
		}
		public override bool CanWriteType(Type type)
		{
			if (type == typeof(IEnumerable<string>))
				return true;

			Type enumerableType = typeof(IEnumerable<IEnumerable<string>>);
			return enumerableType.IsAssignableFrom(type);
		}
		public override bool CanReadType(Type type)
		{
			return false;
		}
		public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
		{
			base.SetDefaultContentHeaders(type, headers, mediaType);

			headers.Add("Content-Disposition", "attachment; filename=" + FileName);
			headers.ContentType = new MediaTypeHeaderValue("text/csv");
		}
		public override void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
		{
			using (var writer = new StreamWriter(writeStream, Encoding.UTF8))
			{
				var products = value as IEnumerable<IEnumerable<string>>;
				if (products != null)
					foreach (var product in products)
						WriteItem(product, writer);
				else
				{
					var singleProduct = value as IEnumerable<string>;
					if (singleProduct == null)
						throw new InvalidOperationException("Cannot serialize type");
					WriteItem(singleProduct, writer);
				}
			}
			writeStream.Close();
		}

		// Helper methods for serializing Products to CSV format. 
		private void WriteItem(IEnumerable<string> texts, StreamWriter writer)
		{
			writer.WriteLine((
				from t in texts
				select Escape(t)
			).JoinString(";"));
		}

		static char[] _specialChars = new char[] { ';', '\n', '\r', '"' };
		private string Escape(object o)
		{
			if (o == null)
			{
				return "";
			}
			string field = o.ToString();
			if (field.IndexOfAny(_specialChars) != -1)
			{
				return String.Format("\"{0}\"", field.Replace("\"", "\"\""));
			}
			else return field;
		}
	}
}
