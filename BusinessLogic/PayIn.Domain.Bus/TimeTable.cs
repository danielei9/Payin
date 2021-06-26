using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Bus
{
	public class TimeTable : Entity
	{
		[Required(AllowEmptyStrings = false)] public string Name { get; set; }
		                                      public DateTime Since { get; set; }
		                                      public DateTime Until { get; set; }

		#region Line
		public int LineId { get; set; }
		[ForeignKey(nameof(LineId))]
		public Line Line { get; set; }
		#endregion Line
	}
}
