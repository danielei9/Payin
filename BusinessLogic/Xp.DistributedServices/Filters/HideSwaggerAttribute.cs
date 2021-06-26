using System;

namespace Xp.DistributedServices.Filters
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
	public class HideSwaggerAttribute : Attribute
	{
	}
}
