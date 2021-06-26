namespace PayIn.Web.App.Factories
{
    public class CheckFactory
    {
        public static string UrlApi { get { return "/Api/Check"; } }
        public static string Url { get { return "/Check"; } }

        #region Get
        public static string Get { get { return UrlApi; } }
        public static string GetApi { get { return UrlApi + "/:id"; } }
        #endregion Get

        #region GetAll
        public static string GetAllName { get { return "checkgetall"; } }
        public static string GetAll { get { return Url; } }
        public static string GetAllApi { get { return UrlApi; } }
        #endregion GetAll
    }
}