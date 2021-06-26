using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayIn.Web.Helpers
{
    public class UrlHelper
    {
        private static UrlHelper instance = null;

        protected UrlHelper() { }

        public static UrlHelper Instance
        {
            get
            {
                if (instance == null)
                    instance = new UrlHelper();

                return instance;
            }
        }
        public bool IsLocalUrl(string url,out string sBaseUrl) { 

            sBaseUrl=  String.Format("{0}://{1}:{2}",
                                    HttpContext.Current.Request.Url.Scheme,
                                    HttpContext.Current.Request.Url.Host,
                                    HttpContext.Current.Request.Url.Port);
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }
            else
            {
                return ((url[0] == '/' && (url.Length == 1 ||
                        (url[1] != '/' && url[1] != '\\'))) ||   // "/" or "/foo" but not "//" or "/\"
                        (url.Length > 1 &&
                         url[0] == '~' && url[1] == '/'));   // "~/" or "~/foo"
            }
        }
    }
}