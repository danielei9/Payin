using Microsoft.Practices.Unity;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace PayIn.Common.DI
{
	public class InjectionModelBinder : IModelBinder
	{
		public IUnityContainer Container { get; private set; }

		#region Constructors
		public InjectionModelBinder(IUnityContainer container)
		{
			Container = container;
		}
		#endregion Constructors

		#region BindModel
		public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
		{
			if (bindingContext.Model != null)
				return false;

			bindingContext.Model = Container.Resolve(bindingContext.ModelType);

			return bindingContext.Model != null;
		}
		#endregion BindModel
	}
}
