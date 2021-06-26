using System;
using System.Collections.Generic;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace Xp.DistributedServices.Formatters
{
	public class ChangeMediaTypeFormatterFormatter : MediaTypeFormatter
	{
		protected IEnumerable<MediaTypeFormatter> Formatters;
		protected IEnumerable<Type> Types;

		#region CanReadType
		public override bool CanReadType(Type type)
		{
			var canRead = Types
				.Any(x => x == type);
			return canRead;
		}
		#endregion CanReadType

		#region CanWriteType
		public override bool CanWriteType(Type type)
		{
			return false;
		}
		#endregion CanWriteType

		#region Constructors
		public ChangeMediaTypeFormatterFormatter(MediaTypeFormatterCollection formatters, string sourceMediaType, string targetMediaType, IEnumerable<Type> types)
		{
			Formatters = formatters
				.Where(x =>
					x.SupportedMediaTypes
						.Any(y =>
							y.MediaType == targetMediaType
						)
				);
			Types = types;

			SupportedMediaTypes.Add(new MediaTypeHeaderValue(sourceMediaType));
		}
		#endregion Constructors

		#region ReadFromStreamAsync
		public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
		{
			var formatter = Formatters
				.Where(x => x.CanReadType(type))
				.FirstOrDefault();

			return formatter != null ?
				formatter.ReadFromStreamAsync(type, readStream, content, formatterLogger) :
				base.ReadFromStreamAsync(type, readStream, content, formatterLogger);
		}
		public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger, CancellationToken cancellationToken)
		{
			var formatter = Formatters
				.Where(x => x.CanReadType(type))
				.FirstOrDefault();

			return formatter != null ?
				formatter.ReadFromStreamAsync(type, readStream, content, formatterLogger, cancellationToken) :
				base.ReadFromStreamAsync(type, readStream, content, formatterLogger, cancellationToken);
		}
		#endregion ReadFromStreamAsync
	}
}