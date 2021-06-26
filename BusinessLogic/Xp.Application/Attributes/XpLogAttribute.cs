using System;

namespace Xp.Application.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class XpLogAttribute : Attribute
	{
		public string RelatedClass { get; set; }
		public string RelatedMethod { get; set; }
		public string RelatedId { get; set; }

		#region Constructors
		public XpLogAttribute(string relatedClass, string relatedMethod, string relatedId = "id")
		{
			RelatedClass = relatedClass;
			RelatedMethod = relatedMethod;
			RelatedId = relatedId;
		}
		#endregion Constructors
	}
}
