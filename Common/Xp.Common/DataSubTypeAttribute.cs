using System;

namespace Xp.Common
{
	[AttributeUsage(AttributeTargets.Property)]
	public class DataSubTypeAttribute : Attribute
	{
		public DataSubType[] DataSubType { get; private set; }

		#region Constructors
		public DataSubTypeAttribute(params DataSubType[] dataSubType)
		{
			DataSubType = dataSubType;
		}
		#endregion Constructors
	}
}
