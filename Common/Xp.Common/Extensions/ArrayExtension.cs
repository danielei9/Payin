using System.Text;
using System.Linq;

namespace System
{
	public static class ArrayExtension
	{
		#region Copy
		public static T[] Copy<T>(this T[] target, T[] source, int sourceOffset = 0, int? length = null, int targetOffset = 0)
		{
			length = length ?? Math.Min(target.Length - targetOffset, source.Length - sourceOffset);

			for (var i = 0; i < length; i++)
				target[i + targetOffset] = source[i + sourceOffset];

			return target;
		}
		#endregion Copy

		#region CloneArray
		public static T[] CloneArray<T>(this T[] source, int? length = null, int offset = 0)
		{
			var length_ = length ?? source.Length;

			var result = new T[length_];
			for (var i = 0; i < length_; i++)
				result[i] = source[i + offset];

			return result;
		}
		#endregion CloneArray
	}
}