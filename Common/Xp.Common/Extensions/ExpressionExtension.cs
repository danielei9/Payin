using System.Reflection;

namespace System.Linq.Expressions
{
	public static class ExpressionExtension
	{
		#region GetProperty
    public static PropertyInfo GetProperty(this Expression expression)
		{
			if (expression == null)
				return null;

			if (expression.NodeType == ExpressionType.Convert)
				return GetProperty((expression as UnaryExpression).Operand);
			else if (expression.NodeType == ExpressionType.Lambda)
				return GetProperty((expression as LambdaExpression).Body);
			else if (expression.NodeType == ExpressionType.MemberAccess)
				return (expression as MemberExpression).Member as PropertyInfo;

			return null;
		}
		#endregion GetProperty
	}
}
