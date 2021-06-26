using Xp.Domain;

namespace PayIn.Domain.Transport
{
	public class TransportCardSupportTitleCompatibility : Entity
	{
		#region TransportTitle
		public int TransportTitleId { get; set; }
		public TransportTitle TransportTitle { get; set; }
		#endregion TransportTitleTitle

		#region TransportCardSupport
		public int TransportCardSupportId { get; set; }
		public TransportCardSupport TransportCardSupport { get; set; }
		#endregion TransportCardSupport
	}
}
