using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class LogResult : Entity
	{
		public string Name { get; set; }
		public string Value { get; set; }

		#region Log
		public int LogId { get; set; }
		public Log Log { get; set; }
		#endregion Log
	}
}
