using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class EntranceFormValue : Entity
	{
		public int	FormValueId { get; set; }

		#region Entrance
		public int EntranceId { get; set; }
		[ForeignKey("EntranceId")]
		public Entrance Entrance { get; set; }
		#endregion Entrance
	}
}
