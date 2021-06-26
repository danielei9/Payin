namespace PayIn.Web.App.Factories.SmartCity
{
	public partial class SensorFactory
	{
		public static string UrlApi { get { return "/SmartCity/Api/Sensor"; } }
		public static string Url { get { return "/SmartCity/Sensor"; } }

		#region GetAll
		public static string GetAllName { get { return "sensorgetall"; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll

		#region GetEnergy
		public static string GetEnergyName { get { return "sensorgetenergy"; } }
		public static string GetEnergyUrl { get { return Url + "/Energy/:id"; } }
		public static string GetEnergyApi { get { return UrlApi + "/Energy"; } }
		#endregion GetEnergy

		#region GetPower
		public static string GetPowerName { get { return "sensorgetpower"; } }
		public static string GetPowerUrl { get { return Url + "/Power/:id"; } }
		public static string GetPowerApi { get { return UrlApi + "/Power"; } }
		#endregion GetPower

		#region GetInstantaneous
		public static string GetInstantaneousName { get { return "sensorgetinstantaneous"; } }
		public static string GetInstantaneousUrl { get { return Url + "/Instantaneous/:id"; } }
		public static string GetInstantaneousApi { get { return UrlApi + "/Instantaneous"; } }
		#endregion GetInstantaneous

		#region GetPerHour
		public static string GetPerHourName { get { return "sensorgetperhour"; } }
		public static string GetPerHourUrl { get { return Url + "/PerHour/:id"; } }
		public static string GetPerHourApi { get { return UrlApi + "/PerHour"; } }
		#endregion GetPerHour

		#region SetTargetValue
		public static string SetTargetValueName { get { return "sensorsettargetvalue"; } }
		//public static string SetTargetValue { get { return Url + "/SetTargetValue"; } }
		public static string SetTargetValueApi { get { return UrlApi + "/SetTargetValue"; } }
        #endregion SetTargetValue

        #region RemoveTargetValue
        public static string RemoveTargetValueName { get { return "sensorremovetargetvalue"; } }
        public static string RemoveTargetValueApi { get { return UrlApi + "/RemoveTargetValue/:id"; } }
        #endregion RemoveTargetValue
    }
}