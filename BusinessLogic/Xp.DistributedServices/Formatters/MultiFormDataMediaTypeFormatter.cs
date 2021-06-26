using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace Xp.DistributedServices.Formatters
{
	public class MultiFormDataMediaTypeFormatter : FormUrlEncodedMediaTypeFormatter
	{
		public MultiFormDataMediaTypeFormatter()
			: base()
		{
			SupportedMediaTypes.Add(new MediaTypeHeaderValue("multipart/form-data"));
			SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/octet-stream"));
		}
		public override bool CanReadType(Type type)
		{
			return true;
		}
		public override bool CanWriteType(Type type)
		{
			return false;
		}
		public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
		{
			try
			{
				if (!content.IsMimeMultipartContent())
					throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

				var result = await content.ReadAsMultipartAsync(new MultipartMemoryStreamProvider());

				// Data
				object command = null;
				var contentData = result.Contents.Where(c => c.Headers.ContentDisposition.Name == "\"data\"").FirstOrDefault();
				if (contentData != null && contentData.Headers.ContentLength > 2)
					command = await contentData.ReadAsAsync(type);
				else
					command = Activator.CreateInstance(type);

				// Files
				var contents = result.Contents.Where(c => c.Headers.ContentDisposition.Name != "\"data\"");
				foreach (var fileContent in contents)
				{
					using (var stream = await fileContent.ReadAsStreamAsync())
					{
						var contentFile = new byte[(int)stream.Length];
						stream.Position = 0;
						stream.Read(contentFile, 0, contentFile.Length);

						var propertyName = fileContent.Headers.ContentDisposition.Name.Replace("\"", "");
						var property = command.GetType().GetPropertyInfo(propertyName);
						if (property != null)
							property.SetValue(command, contentFile);
					}
				}

				return command;
			}
			catch (Exception ex)
			{
				throw;
			}
		}
	}
}
