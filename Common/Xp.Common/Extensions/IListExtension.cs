
namespace System.Collections
{
	public static class IListExtension
	{
	}
}

namespace System.Collections.Generic
{
	public static class IListExtension
	{
		#region AddFormat
		public static void AddFormat(this IList<string> THIS, string format, params object[] args)
		{
			THIS.Add(string.Format(format, args));
		}
		#endregion AddFormat
	}
}
