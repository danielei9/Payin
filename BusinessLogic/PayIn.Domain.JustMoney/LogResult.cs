using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.JustMoney
{
	public class LogResult : Entity
	{
		public string Name { get; set; }
		public string Value { get; set; }

		#region Log
		public int LogId { get; set; }
		[ForeignKey(nameof(LogId))]
		public Log Log { get; set; }
		#endregion Log
	}
}
