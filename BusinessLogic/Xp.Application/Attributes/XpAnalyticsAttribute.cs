using System;
using System.Collections.Generic;

namespace Xp.Application.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class XpAnalyticsAttribute : Attribute
	{
		public string RelatedClass { get; private set; }
		public string RelatedMethod { get; private set; }
		public string[] Arguments { get; set; }
		public string[] Response { get; set; }

		#region Constructors
		public XpAnalyticsAttribute(string relatedClass, string relatedMethod)
		{
			RelatedClass = relatedClass;
			RelatedMethod = relatedMethod;
		}
		#endregion Constructors
	}
}
