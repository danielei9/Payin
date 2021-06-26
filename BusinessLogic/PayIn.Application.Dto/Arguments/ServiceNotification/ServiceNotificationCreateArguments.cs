using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.Notification
{
	public class ServiceNotificationCreateArguments : IArgumentsBase
	{
		                                   public NotificationType  Type           { get; set; }
		[Required(AllowEmptyStrings=true)] public string            Message        { get; set; }
		                                   public int?				ReferenceId    { get; set; }
		[Required(AllowEmptyStrings=true)] public string            ReferenceClass { get; set; }
		[Required(AllowEmptyStrings=true)] public string            SenderLogin    { get; set; }
		[Required(AllowEmptyStrings=true)] public string            ReceiverLogin  { get; set; }

		#region Constructors
		public ServiceNotificationCreateArguments(NotificationType type, string message, int? referenceId, string referenceClass, string senderLogin, string receiverLogin)
		{
			Type            = type;
			Message         = message;
			ReferenceId     = referenceId;
			ReferenceClass  = referenceClass ?? "";
			SenderLogin     = senderLogin ?? "";
			ReceiverLogin   = receiverLogin;
		}
		#endregion Constructors
	}
}

  