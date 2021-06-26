using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.Notification
{
	public class MobileServiceNotificationCreateChatToVisitorArguments : IArgumentsBase
	{
		[Required(AllowEmptyStrings=false)] public string            Message        { get; set; }
		[Required(AllowEmptyStrings=false)] public string            ReceiverLogin  { get; set; }

		#region Constructors
		public MobileServiceNotificationCreateChatToVisitorArguments(string message, string receiverLogin)
		{
			Message         = message;
			ReceiverLogin   = receiverLogin;
		}
		#endregion Constructors
	}
}

  