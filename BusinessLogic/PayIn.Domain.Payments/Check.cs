using PayIn.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class Check : Entity
	{
													public CheckInType Type			{ get; set; }
													public DateTime TimeStamp		{ get; set; }
		[Required(AllowEmptyStrings = false)]		public string Login				{ get; set; }
		[Required(AllowEmptyStrings = true)]		public string Observations		{ get; set; }
		[Required(AllowEmptyStrings = true)]		public string Errors			{ get; set; }

		#region Entrance
		public int EntranceId { get; set; }
		[ForeignKey("EntranceId")]
		public Entrance Entrance { get; set; }
		#endregion Entrance
	}
}
