using System;
using PayIn.Common;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Dto.Results
{
	public partial class ServiceIncidenceGetResult_Notifications
	{
							public int					Id					{ get; set; }
						//	public NotificationType		Type				{ get; set; }
							public NotificationState	State				{ get; set; }
							public string				SenderLogin			{ get; set; }
							public string				SenderName			{ get; set; }
							public string				SenderPhotoUrl		{ get; set; }
							public bool					IsMine				{ get; set; }
							public string				ReceiverLogin		{ get; set; }
							public XpDateTime			CreatedAt			{ get; set; }
							public string				Message				{ get; set; }
							public string				NotificationPhoto	{ get; set; }
		[Precision(9, 6)]	public decimal?				Longitude			{ get; set; }
		[Precision(9, 6)]	public decimal?				Latitude			{ get; set; }
	}
}
