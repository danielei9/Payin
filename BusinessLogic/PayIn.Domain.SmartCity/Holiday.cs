using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.SmartCity
{
	public class Holiday : Entity
	{
		[DataType(DataType.Date)]
		public DateTime Date { get; set; }
		public string Name { get; set; }

		#region Concession
		public int ConcessionId { get; set; }
		[ForeignKey(nameof(Holiday.ConcessionId))]
		public Concession Concession { get; set; }
		#endregion Concession
	}
}
