using System;
using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Results
{
	public partial class ServiceNotificationGetAllResult
	{
		public int					Id				{ get; set; }
		public NotificationType		Type			{ get; set; }
		public NotificationState	State			{ get; set; }
		public int?					ReferenceId		{ get; set; }
		public string				ReferenceClass	{ get; set; }
		public string				SenderLogin		{ get; set; }
		public string				ReceiverLogin	{ get; set; }
		public XpDateTime			CreatedAt		{ get; set; }
		public string				Message			{ get; set; }
	}
}
