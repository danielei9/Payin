using System.Web.Mvc;

namespace PayIn.Web.Areas.JustMoney
{
    public class JustMoneyAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "JustMoney";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "JustMoney_default",
                "JustMoney/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
				new string[] { "PayIn.Web.Areas.JustMoney.Controllers" }
			);
        }
    }
}