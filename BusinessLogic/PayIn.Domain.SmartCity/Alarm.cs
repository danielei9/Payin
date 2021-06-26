using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.SmartCity
{
	public class Alarm : Entity
	{
		[Required(AllowEmptyStrings = false)] public string Message { get; set; } = "";
		[Required] public DateTime Timestamp { get; set; }
		[Required] public string Sender { get; set; } = "";

		#region Alert
		public int AlertId { get; set; }
		[ForeignKey(nameof(Alarm.AlertId))]
		public Alert Alert { get; set; }
		#endregion Alert

		#region Create
		public static Alarm Create(string message, DateTime timestamp, string sender)
		{
			var item = new Alarm
			{
				Message = message ?? "",
				Timestamp = timestamp,
				Sender = sender ?? ""
			};

			return item;
		}
		#endregion Create
	}
}
