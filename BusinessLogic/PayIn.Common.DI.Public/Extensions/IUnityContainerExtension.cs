using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Microsoft.Practices.Unity
{
	public static class IUnityContainerExtension
	{
		#region RegisterTypeList
		public static IUnityContainer RegisterTypeList(this IUnityContainer container, IEnumerable<Type> to, Func<Type, IEnumerable<Type>> from)
		{
			foreach (var itemTo in to)
				foreach (var itemFrom in from(itemTo))
					container.RegisterType(itemFrom, itemTo);

			return container;
		}
		#endregion RegisterTypeList
	}
}
