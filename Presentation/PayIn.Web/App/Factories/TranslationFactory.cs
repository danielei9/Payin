using PayIn.Common;

namespace PayIn.Web.App.Factories
{
	public class TranslationFactory
	{
		public static string UrlApi { get { return "/Api/Translation"; } }
		public static string Url { get { return "/Translation"; } }

		#region Get
		public static string GetApi { get { return UrlApi; } }
		#endregion Get

		#region Create
		public static string Create { get { return "translationcreate"; } }
		public static string CreateFormattedText { get { return "translationcreateformattedtext"; } }
		public static string CreateApi { get { return UrlApi; } }
		#endregion Create

		#region Update
		public static string Update { get { return "translationupdate"; } }
		public static string UpdateFormattedText { get { return "translationupdateformattedtext"; } }
		public static string UpdateApi { get { return UrlApi; } }
		#endregion Update		

		#region Delete
		public static string Delete { get { return "translationdelete"; } }
		public static string DeleteApi { get { return UrlApi; } }
		#endregion NameDelete
	}
}