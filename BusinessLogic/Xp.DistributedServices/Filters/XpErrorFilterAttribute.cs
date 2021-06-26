using PayIn.Common.DI.Public;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using Xp.Infrastructure;

namespace Xp.DistributedServices.Filters
{
	public class XpErrorFilterAttribute : ExceptionFilterAttribute
	{
		#region OnException
		public override void OnException(HttpActionExecutedContext context)
		{
#if !DEBUG && !RELEASE
			if (context != null && context.Exception != null)
			{
				var log = DIConfig.Resolve<ILogService>();
				log.TrackException(context.Exception);
			}
#endif
			if (context.Response == null) {
				if (context.Exception is NotImplementedException)
				{
					context.Response = context.Request.CreateErrorResponse(
						HttpStatusCode.NotImplemented,
						context.Exception.GetXpMessage()
					);
				}
				else if (context.Exception is UnauthorizedAccessException)
				{
					context.Response = context.Request.CreateErrorResponse(
						HttpStatusCode.Unauthorized,
						context.Exception.GetXpMessage() // TODO: No se va a enviar el mensaje por temas de seguridad
					);
				}
				else if (context.Exception is ArgumentException)
				{
					context.Response = context.Request.CreateErrorResponse(
						HttpStatusCode.BadRequest,
						context.Exception.GetXpMessage()
					);
				}
				else if (context.Exception is ArgumentNullException)
				{
					context.Response = context.Request.CreateErrorResponse(
						HttpStatusCode.BadRequest,
						context.Exception.GetXpMessage()
					);
				}
				else if (context.Exception is ApplicationException)
				{
					context.Response = context.Request.CreateErrorResponse(
						HttpStatusCode.NotFound,
						context.Exception.GetXpMessage()
					);
				}
				else
				{
					context.Response = context.Request.CreateErrorResponse(
						HttpStatusCode.InternalServerError,
						context.Exception.GetXpMessage()
					);
				}
			}
			else if (context.Response.StatusCode == HttpStatusCode.Unauthorized) { }
			else
			{
				context.Response = context.Request.CreateErrorResponse(
					HttpStatusCode.InternalServerError,
					context.Exception.GetXpMessage()
				);
			}
		}
		#endregion OnException
	}
}
