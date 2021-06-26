using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class Contact : Entity
	{
		[Required(AllowEmptyStrings = false)] public string VisitorLogin { get; set; }
		[Required(AllowEmptyStrings = false)] public string VisitorName { get; set; }
		public ContactState State { get; set; }

		#region Exhibitor
		public int ExhibitorId { get; set; }
		[ForeignKey("ExhibitorId")]
		public Exhibitor Exhibitor { get; set; }
		#endregion Exhibitor

		#region Event
		public int EventId { get; set; }
		[ForeignKey("EventId")]
		public Event Event { get; set; }
		#endregion Event

		#region Entrance
		public int? VisitorEntranceId { get; set; }
		[ForeignKey("VisitorEntranceId")]
		public Entrance VisitorEntrance { get; set; }
		#endregion Entrance
	}
}