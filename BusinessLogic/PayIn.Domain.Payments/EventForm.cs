using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;


namespace PayIn.Domain.Payments
{
	public class EventForm : Entity
	{
		public int FormId	{ get; set; }

		#region Event
		public int EventId { get; set; }
		[ForeignKey("EventId")]
		public Event Event { get; set; }
		#endregion Event
	}
}
