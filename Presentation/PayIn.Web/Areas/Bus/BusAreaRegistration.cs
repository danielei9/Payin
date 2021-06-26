using System.Web.Mvc;

namespace PayIn.Web.Areas.Bus
{
    public class BusAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Bus";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Bus_default",
                "Bus/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
				new string[] { "PayIn.Web.Areas.Bus.Controllers" }
			);
        }
    }
}