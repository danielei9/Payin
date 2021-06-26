using PayIn.Web.Helpers;
using System.Collections.Generic;
using System.Web.WebPages;
using PayIn.Web;
using System.Linq;

namespace System.Web.Mvc
{
	public static class XpHtmlHelpersExtension
	{
		public const string Separator = "<!-- UsingBody -->";

		#region XpHelpers
		public static Type XpHelpers { get; private set; }
		public static void SetXpHelpers(Type xpHelpers)
		{
			XpHelpers = xpHelpers;
		}
		#endregion Ami2Helpers

		#region GetHelper
		private static IDisposable GetHelper(HtmlHelper self, string helper, params object[] parameters)
		{
			var result = XpHelpers.ExecuteStaticMethod(helper, parameters) as HelperResult;
			if (result == null)
				throw new Exception(string.Format("No se ha encontrado el helper {0}", helper));

			var left = result.ToString();
			var right = string.Empty;

			var index = left.IndexOf(Separator, StringComparison.CurrentCulture);
			if (index >= 0)
			{
				right = left.Substring(index + Separator.Length);
				left = left.Substring(0, index);
			}

			return new GenericDisposableWrapper(self, () => left, () => right);
		}
		#endregion GetHelper

		#region xpList
		public static IDisposable xpList(this HtmlHelper self, string name, string apiUrl, string classIcon, string title, string subtitle = "", string cache = "", string controller = null, string cancelText = null, bool isPaginable = false, bool isSearchable = true, bool isTitleActive = false, bool showSpinner = true, bool showFormButtons = false, string addPanelUrl = "", string addPopupUrl = "", string addArguments = "", string init = "", bool isHeaderActive = true, bool isBreadcrumbActive = true, string csvUrl = "", IEnumerable<ActionLink> actions = null, bool initialSearch = true, string cardBeforePage = null, string previous = null, string success = null)
		{
			return GetHelper(self, "_xpList", name, apiUrl, classIcon, title, subtitle, cache, controller, cancelText, isPaginable, isSearchable, isTitleActive, showSpinner, showFormButtons, addPanelUrl, addPopupUrl, addArguments, init, isHeaderActive, isBreadcrumbActive, csvUrl, actions ?? Enumerable.Empty<ActionLink>(), initialSearch,cardBeforePage, previous, success);
		}
		#endregion xpList

		#region xpPopupGet
		public static IDisposable xpPopupGet(this HtmlHelper self, string name, string apiUrl, string classIcon, string title, string subtitle = "", string cache = "", string id = "", string arguments = null, string init = "", string controller = "", string previous = null, string accept = "", int? goBack = 1, string success = null, string successUrl = null, string successPopup = null, bool noInitialSearch = false)
		{
			return GetHelper(self, "_xpPopupGet", name, apiUrl, classIcon, title, subtitle, cache, id, arguments, init, controller, previous, accept, goBack, success, successUrl, successPopup, noInitialSearch);
		}
		#endregion xpPopupGet

		#region xpPopupPut
		public static IDisposable xpPopupPut(this HtmlHelper self, string name, string apiUrl, string classIcon, string title, string subtitle = "", string cache = "", string id = null, string arguments = null, string init = "", string controller = null, string acceptText = null, string previous = null, string success = null, string successUrl = null)
		{
			return GetHelper(self, "_xpPopupPut", name, apiUrl, classIcon, title, subtitle, cache, id, arguments, init, controller, acceptText, previous, success, successUrl);
		}
		#endregion xpPopupPut

		#region xpPut
		public static IDisposable xpPut(this HtmlHelper self, string name, string apiUrl, string classIcon, string title, string subtitle = "", string cache = "", string arguments = null, string init = "", string controller = null, string acceptText = null, string cancelText = null, bool isTitleActive = true, bool showSpinner = true, bool showFormButtons = true, bool isHeaderActive = true, bool isBreadcrumbActive = true, IEnumerable<ActionLink> actions = null, string previous = null, int? goBack = null, string success = null, string successPopup = null)
		{
			return GetHelper(self, "_xpPut", name, apiUrl, classIcon, title, subtitle, cache, arguments, init, controller, acceptText, cancelText, isTitleActive, showSpinner, showFormButtons, isHeaderActive, isBreadcrumbActive, actions, previous, goBack, success, successPopup);
		}
		#endregion xpPut

		#region xpPopupPost
		public static IDisposable xpPopupPost(this HtmlHelper self, string name, string apiUrl, string classIcon, string title, string subtitle = "", string cache = "", string arguments = null, string init = "", string controller = null, string acceptText = null, string previous = null, string successPopup = null)
		{
			return GetHelper(self, "_xpPopupPost", name, apiUrl, classIcon, title, subtitle, cache, arguments, init, controller, acceptText, previous, successPopup);
		}
		#endregion xpPopupPost

		#region xpPost
		public static IDisposable xpPost(this HtmlHelper self, string name, string apiUrl, string classIcon, string title, string subtitle = "", string cache = "", string arguments = null, string init = "", string controller = null, string acceptText = null, string cancelText = null, bool isTitleActive = true, bool showSpinner = true, bool showFormButtons = true, bool isHeaderActive = true, bool isBreadcrumbActive = true, string previous = null, int? goBack = null, string success = null, string successUrl = null, string successPopup = null)
		{
			return GetHelper(self, "_xpPost", name, apiUrl, classIcon, title, subtitle, cache, arguments, init, controller, acceptText, cancelText, isTitleActive, showSpinner, showFormButtons, isHeaderActive, isBreadcrumbActive, previous, goBack, success, successUrl, successPopup);
		}
		#endregion xpPost

		#region xpPopupDelete
		public static IDisposable xpPopupDelete(this HtmlHelper self, string name, string apiUrl, string classIcon, string title, string subtitle = "", string cache = "", string id = null, string arguments = null, string init = "", string controller = null, string previous = null, string success = null, string successUrl = null, string successPopup = null)
		{
			return GetHelper(self, "_xpPopupDelete", name, apiUrl, classIcon, title, subtitle, cache, id, arguments, init, controller, previous, success, successUrl, successPopup);
		}
		#endregion xpPopupDelete

		#region xpPopupShow
		public static IDisposable xpPopupShow(this HtmlHelper self, string name, string classIcon, string title, string subtitle = "", string cache = "", string id = null, string arguments = null, string init = "", string controller = null, string acceptText = null, bool showAcceptButton = false, bool showCancelButton = false, string cancelText = null, string accept = "")
		{
			return GetHelper(self, "_xpPopupShow", name, classIcon, title, subtitle, cache, id, arguments, init, controller, acceptText, showAcceptButton, showCancelButton, cancelText, accept);
		}
		#endregion xpPopupShow
	}
}