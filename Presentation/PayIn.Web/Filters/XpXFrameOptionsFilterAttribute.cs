
using System;
using System.Web.Mvc;

namespace Xp.DistributedServices.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class XpXFrameOptionsFilterAttribute : ActionFilterAttribute
    {
        public enum XpXFrameOption
        {
            None = 0,
            Deny = 1,
            SameOrigin = 2,
            AllowFrom = 3
        }

        #region AllowFrom
        private string allowFrom;
        public string AllowFrom
        {
            get { return allowFrom; }
            set { allowFrom = value; option = XpXFrameOption.AllowFrom; }
        }
        #endregion AllowFrom

        #region Option
        private XpXFrameOption option;
        public XpXFrameOption Option
        {
            get { return option; }
            set { option = value; }
        }
        #endregion Option

        #region Constructors
        public XpXFrameOptionsFilterAttribute()
          : this(XpXFrameOption.Deny)
        {
        }
        public XpXFrameOptionsFilterAttribute(XpXFrameOption option)
        {
            Option = option;
        }
        public XpXFrameOptionsFilterAttribute(string allowFrom)
        {
            AllowFrom = allowFrom;
        }
        #endregion Constructors

        #region OnResultExecuting
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Headers.Remove("x-frame-options");
            if (option != XpXFrameOption.None)
                filterContext.HttpContext.Response.AddHeader("x-frame-options", FormatOption(option, allowFrom));
        }
        #endregion OnResultExecuting

        #region FormatOption
        private static string FormatOption(XpXFrameOption option, string allowFrom)
        {
            if (option == XpXFrameOption.AllowFrom)
                return "ALLOW-FROM " + allowFrom;

            return option.ToString("G").ToUpperInvariant();
        }
        #endregion FormatOption
    }
}
