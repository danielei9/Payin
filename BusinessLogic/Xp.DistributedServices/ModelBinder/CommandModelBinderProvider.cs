using System;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Xp.Common.Dto.Arguments;

namespace Xp.DistributedServices.ModelBinder
{
	public class CommandModelBinderProvider : ModelBinderProvider
	{
		#region GetBinder
		public override IModelBinder GetBinder(HttpConfiguration configuration, Type modelType)
		{
			//var sessionData = DIConfig.Resolve<ISessionData>();

			return
				typeof(IArgumentsBase).IsAssignableFrom(modelType) ?
					new CommandModelBinder(modelType/*, sessionData*/) :
				//typeof(IBaseArguments).IsAssignableFrom(modelType) ?
				//	new CommandModelBinder(modelType/*, sessionData*/) :
					null;
		}
		#endregion GetBinder
	}
}
