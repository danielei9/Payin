using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class NotificationSetGetAllArguments : IArgumentsBase
	{
		public int EventId { get; set; }

		#region Constructors
		public NotificationSetGetAllArguments(int eventId)
		{
            EventId = eventId;
		}
		#endregion Constructors
	}
}
