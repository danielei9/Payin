using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Xp.DistributedServices.Filters
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true)]
	public class CheckModelForNullAttribute : ActionFilterAttribute
	{
		private readonly Func<Dictionary<string, object>, bool> _validate;

		#region Constructors
		public CheckModelForNullAttribute()
			: this(arguments =>
				arguments.ContainsValue(null))
		{ }
		public CheckModelForNullAttribute(Func<Dictionary<string, object>, bool> checkCondition)
		{
			_validate = checkCondition;
		}
		#endregion Constructors

		#region OnActionExecuting
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			if (_validate(actionContext.ActionArguments))
				actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The argument cannot be null");
		}
		#endregion OnActionExecuting
	}
}
