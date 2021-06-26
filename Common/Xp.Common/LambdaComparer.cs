using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq
{
	public class LambdaComparer<T> : IEqualityComparer<T>
	{
		private readonly Func<T, T, bool> _lambdaComparer;
		private readonly Func<T, int> _lambdaHash;

		#region Constructors
		public LambdaComparer(Func<T, T, bool> lambdaComparer)
			: this(lambdaComparer, o => 0)
		{
		}
		public LambdaComparer(Func<T, T, bool> lambdaComparer, Func<T, int> lambdaHash)
		{
			if (lambdaComparer == null)
				throw new ArgumentNullException("lambdaComparer");
			if (lambdaHash == null)
				throw new ArgumentNullException("lambdaHash");

			_lambdaComparer = lambdaComparer;
			_lambdaHash = lambdaHash;
		}
		#endregion Constructors

		#region Equals
		public bool Equals(T x, T y)
		{
			var result = _lambdaComparer(x, y);
			return result;
		}
		#endregion Equals

		#region GetHashCode
		public int GetHashCode(T obj)
		{
			return _lambdaHash(obj);
		}
		#endregion GetHashCode
	}
}
