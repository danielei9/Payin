using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class NotificationSetCreateArguments : IArgumentsBase
	{

		[Display(Name = "resources.notificationset.event")]
		public int EventId { get; set; }

		[Display(Name = "resources.notificationset.message")]
		[Required(AllowEmptyStrings = false)]
		public string Message { get; set; }



		#region Constructors
		public NotificationSetCreateArguments(int eventId, string message)
		{
			EventId = eventId;
			Message = message;
		}
		#endregion Constructors
	}
}
