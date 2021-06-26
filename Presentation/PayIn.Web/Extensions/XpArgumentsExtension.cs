using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;

namespace System.Web.Mvc
{
	public static class XpArgumentsExtension
	{
		#region XpArguments
		public static Type XpArguments { get; private set; }
		public static void SetXpArguments(Type xpArguments)
		{
			XpArguments = xpArguments;
		}
		#endregion XpArguments

		#region GetHelper
		private static HelperResult GetHelper(HtmlHelper self, string helper, params object[] parameters)
		{
			var result = XpArguments.ExecuteStaticMethod(helper, parameters) as HelperResult;
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

		#region ArgumentFor
		public static HelperResult ArgumentFor<ARGT>(this HtmlHelper self, Expression<Func<ARGT, object>> expression, string fieldUrl = null, string label = "", int? width = 12, bool isPlaceHolder = false, bool focus = false, string disabled = "", string fileAutosave = null, string inputAttribute = "", string placeHolder = "", bool? showAll = null, string optionResource = "", int? dataMin = -180, int? dataMax = 1000)
		{
			PropertyInfo property = GetProperty(expression);

			return GetHelper(self, "Argument", self, property, fieldUrl, label, width, isPlaceHolder, focus, disabled, fileAutosave, inputAttribute, placeHolder, showAll, optionResource, dataMin, dataMax);
		}
		#endregion ArgumentFor

		#region SelectFor
		public static HelperResult SelectFor<ARGT>(this HtmlHelper self, Expression<Func<ARGT, object>> expression, string modelAlias, string api = "", string fieldUrl = null, string label = "", int? width = 12, bool isPlaceHolder = false, bool focus = false, string disabled = "", string fileAutosave = null, string arguments = "", string placeHolder = "", string changed = "", bool buttonAdd = false, string functionButtonAdd="")
		{
			PropertyInfo property = GetProperty(expression);

			return GetHelper(self, "Select", self, property, modelAlias, api, fieldUrl, label, width, isPlaceHolder, focus, disabled, fileAutosave, arguments, placeHolder, changed, buttonAdd, functionButtonAdd);
		}
		#endregion SelectFor
	}
}
