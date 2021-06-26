using System;

namespace Xp.Common
{
	[AttributeUsage(AttributeTargets.Field)]
	public class AliasAttribute : Attribute
	{
		public string Name { get; set; }
	}
}
