using System.Web.Mvc;

namespace PayIn.Web.Areas.SmartCity.Controllers
{
	public class SensorController : Controller
    {
		#region GET SmartCity/Sensor
		public ActionResult Index()
        {
            return PartialView();
		}
		#endregion GET SmartCity/Sensor

		#region GET SmartCity/Sensor/Energy
		public ActionResult Energy()
		{
			return PartialView();
		}
		#endregion GET SmartCity/Sensor/Energy

		#region GET SmartCity/Sensor/MaxEnergy
		public ActionResult MaxEnergy()
		{
			return PartialView();
		}
		#endregion GET SmartCity/Sensor/MaxEnergy

		#region GET SmartCity/Sensor/Power
		public ActionResult Power()
		{
			return PartialView();
		}
		#endregion GET SmartCity/Sensor/Power

		#region GET SmartCity/Sensor/Instantaneous
		public ActionResult Instantaneous()
		{
			return PartialView();
		}
		#endregion GET SmartCity/Sensor/Instantaneous

		#region GET SmartCity/Sensor/PerHour
		public ActionResult PerHour()
		{
			return PartialView();
		}
        #endregion GET SmartCity/Sensor/PerHour

        #region GET SmartCity/Sensor/SetTargetValue
        public ActionResult SetTargetValue()
        {
            return PartialView();
        }
        #endregion GET SmartCity/Sensor/SetTargetValue

        #region GET SmartCity/Sensor/RemoveTargetValue
        public ActionResult RemoveTargetValue()
        {
            return PartialView();
        }
        #endregion GET SmartCity/Sensor/RemoveTargetValue
    }
}