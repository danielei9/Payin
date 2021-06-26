using Xp.Common.Resources;

namespace PayIn.Common.Resources
{
	public class Resources : CommonResources
	{
		public string                      Test               { get; private set; }

		public EnumResources               Enum               { get; private set; }
		public MainResources               Main               { get; private set; }
		public PaymentMediaResources       PaymentMedia       { get; private set; }
		public ServiceAddressResources     ServiceAddress     { get; private set; }
		public ServiceAddressNameResources ServiceAddressName { get; private set; }
		public ServiceCityResources        ServiceCity        { get; private set; }
		public ServiceNumberPlateResources ServiceNumberPlate { get; private set; }
		public ServicePriceResources       ServicePrice       { get; private set; }
		public ServiceCheckPointResources  ServiceCheckPoint  { get; private set; }
		public ServiceTimeTableResources   ServiceTimeTable   { get; private set; }
		public ServiceZoneResources        ServiceZone        { get; private set; }
		public TicketResources             Ticket             { get; private set; }
		public PaymentResources            Payment            { get; private set; }

		#region Constructors
		public Resources()
		{
			Test = "Test";

			Enum =               new EnumResources();
			Main =               new MainResources();
			PaymentMedia =       new PaymentMediaResources();
			ServiceAddress =     new ServiceAddressResources();
			ServiceAddressName = new ServiceAddressNameResources();
			ServiceCity =        new ServiceCityResources();
			ServiceNumberPlate = new ServiceNumberPlateResources();
			ServicePrice =       new ServicePriceResources();
			ServiceTimeTable =   new ServiceTimeTableResources();
			ServiceZone =        new ServiceZoneResources();
			Ticket =             new TicketResources();
			Payment =            new PaymentResources();
		}
		#endregion Constructors
	}
}
