using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.Notification
{
	public class MobileServiceNotificationCreateArguments : IArgumentsBase
	{
		public int IncidenceId { get; set; }

		//[Display(Name = "resources.notification.sender")]
		//public string SenderLogin { get; set; }

		//[Display(Name = "resources.notification.receiver")]
		//public string ReceiverLogin { get; set; }

		[Display(Name = "resources.notification.longitude")]
		public decimal? Longitude { get; set; }

		[Display(Name = "resources.notification.latitude")]
		public decimal? Latitude { get; set; }

		[DataType(DataType.ImageUrl)]
		[Display(Name = "resources.serviceNotification.photo")]
		public string PhotoUrl { get; set; }

		[Display(Name = "resources.incidence.message")]
		public string Message { get; set; }

		#region Constructors
		//public MobileServiceNotificationCreateArguments(int incidenceId, string senderLogin, string receiverLogin, decimal? longitude, decimal? latitude, string message)
		public MobileServiceNotificationCreateArguments(int incidenceId, decimal? longitude, decimal? latitude, string photoUrl, string message)
		{
			IncidenceId		= incidenceId;
			//SenderLogin     = senderLogin ?? "";
			//ReceiverLogin   = receiverLogin ?? "";
			Longitude		= longitude;
			Latitude		= latitude;
			Message			= message;
			PhotoUrl		= photoUrl ?? "";
		}
		#endregion Constructors
	}
}

  