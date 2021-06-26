using System.Web.Mvc;

namespace PayIn.Web.Areas.SmartCity
{
    public class SmartCityAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SmartCity";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SmartCity_default",
                "SmartCity/{controller}/{action}/{id}",
                new { controller= "SmartCity", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}