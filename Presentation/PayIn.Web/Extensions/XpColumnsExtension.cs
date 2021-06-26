using System.Linq.Expressions;
using System.Reflection;
using System.Web.WebPages;

namespace System.Web.Mvc
{
	public static class XpColumnsExtension
	{
		#region XpColumns
		public static Type XpColumns { get; private set; }
		public static void SetXpColumns(Type xpColumns)
		{
			XpColumns = xpColumns;
		}
		#endregion XpColumns

		#region GetHelper
		private static HelperResult GetHelper(HtmlHelper self, string helper, params object[] parameters)
		{
			var result = XpColumns.ExecuteStaticMethod(helper, parameters) as HelperResult;
			if (result == null)
				throw new Exception(string.Format("No se ha encontrado el helper {0}", helper));

			return result;
		}
		#endregion GetHelper

		#region GetProperty
		private static PropertyInfo GetProperty(Expression expression)
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

		#region ColumnFor
		public static HelperResult ColumnFor<ARGT>(this HtmlHelper self, Expression<Func<ARGT, object>> expression, int? width = null, string popupUrl = null, string panelUrl = null, string id = null, string arguments = null, string filter = null, string filterArgument = null, int decimals = 2, string currency = "EUR", string iterator = "item")
		{
			PropertyInfo property = GetProperty(expression);

			return GetHelper(self, "Column", self, property, width, popupUrl, panelUrl, id, arguments, filter, filterArgument, decimals, currency, iterator);
		}
		#endregion ColumnFor
	}
}
