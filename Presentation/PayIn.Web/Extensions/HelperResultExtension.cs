using System.Text;

namespace System.Web.WebPages
{
	public static class HelperResultExtension
	{
		#region AddTabs
		public static HtmlString AddTabs(this HelperResult that, int tabs)
		{
			var result = that.ToHtmlString();

			var newValue = new StringBuilder();
			for (var i = 0; i < tabs; i++)
				newValue.Append("\t");

			result = newValue.ToString() + result.Replace("\r\n\t", "\r\n\t" + newValue.ToString());
			return new HtmlString(result);
		}
		#endregion AddTabs
	}
}