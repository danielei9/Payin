using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Public;
using PayIn.Domain.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xp.Domain;

namespace Xp.Infrastructure.Services
{
	// http://developer.android.com/google/gcm/index.html
	public class PushAndroidService : IPushAndroidService
	{
		public DeviceType Type { get { return DeviceType.Android; } }

		private readonly IEntityRepository<Device> DeviceRepository;
		private readonly IEntityRepository<Platform> PlatformRepository;
		private readonly ISessionData SessionData;

		#region Constructors
		public PushAndroidService(
			IEntityRepository<Device> deviceRepository,
			IEntityRepository<Platform> platformRepository,
			ISessionData sessionData
		)
		{
			if (deviceRepository == null)
				throw new ArgumentNullException("deviceRepository");
			DeviceRepository = deviceRepository;

			if (platformRepository == null)
				throw new ArgumentNullException("platformRepository");
			PlatformRepository = platformRepository;

			if (sessionData == null)
				throw new ArgumentNullException("sessionData");
			SessionData = sessionData;
		}
		#endregion Constructors

		#region SendNotification
		public async Task<string> SendNotification(string pushId, string pushCertificate, IEnumerable<string> targetIds, NotificationType type, NotificationState state, string message, string relatedName, string relatedId, int notificationId, int sourceId, string sourceNombre)
		{
			string y = await SendNotificationInternal(
				pushId,
				pushCertificate,
				targetIds
					.Select(x => "\"" + x + "\"")
					.JoinString(","),
				type,
				state,
				message,
				relatedName,
				relatedId,
				notificationId,
				sourceId,
				sourceNombre
				);
			return y;
		}
		#endregion SendNotification

		#region SendNotificationInternal
		private async Task<string> SendNotificationInternal(string pushId, string pushCertificate, string targetId, NotificationType type, NotificationState state, string message, string relatedName, string relatedId, int notificationId, int sourceId, string sourceNombre)
		{
			return await Task.Run(() =>
			{
				try
				{
					var postData = string.Format(
						"{{" +
							"\"collapse_key\":\"score_update\"," +
							"\"registration_ids\":[{2}]," +
							"\"data\":{{" +
								"\"message\":\"{0}\"," +
								"\"vibrate\":\"1\"," +
								"\"class\":\"{3}\"," +
								"\"id\":\"{4}\"," +
								"\"sound\":\"1\"," +
								"\"time\":\"{1}\"," +
								"\"type\":\"{5}\"," +
								"\"state\":\"{6}\"" +
							"}}" +
						"}}",
						message,
						DateTime.UtcNow.ToString(),
						targetId,
						relatedName,
						relatedId,
						(int)type,
						(int)state
					);
					var byteArray = Encoding.UTF8.GetBytes(postData);

					var request = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
					request.Method = "POST";
					request.KeepAlive = false;
					request.ContentType = "application/json";
					request.Headers.Add(string.Format("Authorization:key={0}", pushCertificate));
					request.ContentLength = byteArray.Length;

					using (var dataStream = request.GetRequestStream())
					{
						dataStream.Write(byteArray, 0, byteArray.Length);
					}

					using (var response = request.GetResponse())
					{
						var responseCode = ((HttpWebResponse)response).StatusCode;
						if (responseCode.Equals(HttpStatusCode.Unauthorized) || responseCode.Equals(HttpStatusCode.Forbidden))
							throw new ApplicationException("Unauthorized - need new token");
						else if (!responseCode.Equals(HttpStatusCode.OK))
							throw new ApplicationException("Response from web service isn't OK");

						using (var dataResponse = response.GetResponseStream())
						{
							using (var reader = new StreamReader(dataResponse))
							{
								var result = reader.ReadToEnd();
								return result;
							}
						}
					}
				}
				catch (Exception e)
				{
					throw new Exception(e.Message, e.InnerException);
				}
			});
		}
		#endregion SendNotificationInternal
	}
}
