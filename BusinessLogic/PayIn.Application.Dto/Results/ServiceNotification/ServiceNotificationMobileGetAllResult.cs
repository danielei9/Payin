using PayIn.Common;
using Xp.Common;

namespace PayIn.Application.Dto.Results.ServiceNotification
{
	public class ServiceNotificationMobileGetAllResult
	{	
		public int               Id             { get; set; }
		public XpDateTime        Date           { get; set; }
		public NotificationType  Type           { get; set; }
		public NotificationState State          { get; set; }
		public string            Message        { get; set; }
		public int?              ReferenceId    { get; set; }
		public string            ReferenceClass { get; set; }
		//public string			 ReferenceName	{ get; set; } //En caso de ser una noticia, por ejemplo, el título de la misma
	}
}
