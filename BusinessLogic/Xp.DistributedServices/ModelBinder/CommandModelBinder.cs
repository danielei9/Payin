using System;
using System.Collections.Generic;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace Xp.DistributedServices.ModelBinder
{
	public class CommandModelBinder : IModelBinder
	{
		private readonly Type _modelType;
		//private readonly ISessionData _SessionData;

		#region Constructors
		public CommandModelBinder(Type modelType //,
			//ISessionData sessionData
			)
		{
			if (modelType == null)
				throw new ArgumentNullException("modelType");
			_modelType = modelType;

			//if (sessionData == null)
			//	throw new ArgumentNullException("sessionData");
			//_SessionData = sessionData;
		}
		#endregion Constructors

		#region BindModel
		public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
		{
			//#region Check tenant url vs identity
			//var tenantRouting = bindingContext.ValueProvider.GetValue("tenant");
			//if ((_SessionData.Tenant != null) && (tenantRouting != null) && (string.Compare(tenantRouting.RawValue.ToString(), _SessionData.Tenant, true) != 0))
			//	throw new HttpResponseException(HttpStatusCode.Unauthorized);
			//#endregion Check tenant url vs identity

			//#region Add Tenant claim if server multi-tenant
			//if (_SessionData.Tenant.IsNullOrEmpty() && tenantRouting != null)
			//{
			//	var identity = Thread.CurrentPrincipal.Identity as ClaimsIdentity;
			//	identity.AddClaim(new Claim(XpClaimTypes.Tenant, tenantRouting.RawValue.ToString()));
			//}
			//#endregion Add Tenant claim if server multi-tenant

			#region Inyectar argumentos
			var parameters = _modelType.GetConstructors()[0].GetParameters();
			var values = new List<object>();
			foreach (var parameter in parameters)
			{
				var valueProvider = bindingContext.ValueProvider.GetValue(parameter.Name);
				values.Add(null == valueProvider ? null : valueProvider.ConvertTo(parameter.ParameterType, valueProvider.Culture));
			}
			#endregion Inyectar argumentos

			bindingContext.Model = Activator.CreateInstance(bindingContext.ModelType, values.ToArray());

			return true;
		}
		#endregion BindModel
	}
}
