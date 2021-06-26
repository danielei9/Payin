using PayIn.Common.DI;
using System.Web.Http.ModelBinding;

namespace Xp.DistributedServices.ModelBinder
{
	public class InjectionAttribute : ModelBinderAttribute
	{
		#region Constructor
		public InjectionAttribute()
			: base(typeof(InjectionModelBinder))
		{
		}
		#endregion Constructor
	}
}
