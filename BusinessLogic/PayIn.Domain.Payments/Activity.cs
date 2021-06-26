using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class Activity : Entity
	{
												public DateTime		Start		{ get; set; }
												public DateTime		End			{ get; set; }
		[Required(AllowEmptyStrings = false)]	public string		Name		{ get; set; }
		[Required(AllowEmptyStrings = true)]	public string		Description { get; set; }

		#region Event
		public int EventId { get; set; }
		[ForeignKey("EventId")]
		public Event Event { get; set; }
		#endregion Event
	}
}
