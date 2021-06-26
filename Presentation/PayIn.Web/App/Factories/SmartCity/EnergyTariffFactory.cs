namespace PayIn.Web.App.Factories.SmartCity
{
	public partial class EnergyTariffFactory
	{
		public static string UrlApi { get { return "/SmartCity/Api/EnergyTariff"; } }
		public static string Url { get { return "/SmartCity/EnergyTariff"; } }

		#region GetAll
		public static string GetAllName { get { return "energytariffgetall"; } }
		public static string GetAllApi { get { return UrlApi; } }
		#endregion GetAll
	}
}