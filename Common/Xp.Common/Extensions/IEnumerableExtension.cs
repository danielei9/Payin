using System.Linq;

namespace System.Collections
{
	public static class IEnumerableExtension
	{
		#region Count
		public static int Count(this IEnumerable THIS)
		{
			var i = 0;
			foreach (var item in THIS)
				i++;

			return i;
		}
		#endregion Count

		#region ElementAt
		public static object ElementAt(this IEnumerable THIS, int num)
		{
			var i = 0;
			foreach (var item in THIS)
			{
				if (i == num)
					return item;
				i++;
			}

			return null;
		}
		#endregion ElementAt

		#region IsNullOrEmpty
		public static bool IsNullOrEmpty(this IEnumerable THIS)
		{
			return ((THIS == null) || THIS.Count() == 0);
		}
		#endregion IsNullOrEmpty
	}
}

namespace System.Collections.Generic
{
	public static class IEnumerableExtension
	{
		#region JoinString
		public static string JoinString(this IEnumerable<string> THIS, string separator)
		{
			return string.Join(separator, THIS);
		}
		#endregion JoinString

		#region SetEquals
		public static bool SetEquals<T>(this IEnumerable<T> that, IEnumerable<T> other)
		{
			return (
				that.Count() == other.Count() &&
				!that
					.Where(x => !other.Contains(x))
					.Any()
			);
		}
		#endregion SetEquals
	}
}
