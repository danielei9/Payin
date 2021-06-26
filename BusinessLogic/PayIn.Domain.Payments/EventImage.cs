using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class EventImage : Entity
	{
		[Required(AllowEmptyStrings = false)] public string PhotoUrl { get; set; }

		#region Event
		public int EventId { get; set; }
		[ForeignKey("EventId")]
		public Event Event { get; set; }
		#endregion Event
	}
}
