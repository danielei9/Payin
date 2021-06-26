using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Transport
{
	public class TransportSimultaneousTitleCompatibility : Entity
	{
		#region TransportTitle
		public int TransportTitleId { get; set; }
		[ForeignKey("TransportTitleId")]
		public TransportTitle TransportTitle { get; set; }
		#endregion TransportTitle

		#region TransportTitle2
		public int? TransportTitle2Id { get; set; }
		[ForeignKey("TransportTitle2Id")]
		public TransportTitle TransportTitle2 { get; set; }
		#endregion TransportTitle2
	}
}
