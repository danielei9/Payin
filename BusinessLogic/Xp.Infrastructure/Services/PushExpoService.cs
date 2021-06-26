using PayIn.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Xp.Infrastructure.Services
{
    public class PushExpoService : IPushExpoService
    {
        public DeviceType Type { get { return DeviceType.Expo; } }

        #region Constructors

        public PushExpoService(
        )
        {
        }

        #endregion

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

        #endregion

        #region SendNotificationInternal

        private async Task<string> SendNotificationInternal(string pushId, string pushCertificate, string targetId, NotificationType type, NotificationState state, string message, string relatedName, string relatedId, int notificationId, int sourceId, string sourceNombre)
        {
            return await Task.Run(() =>
            {
                try
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    var postData = string.Format(
                        "{{" +
                            "\"to\":[{2}]," +
                            "\"body\":\"{0}\"," +
                            "\"title\":\"{3}\"," +
                            "\"sound\":\"default\"," +
                            "\"channelId\":\"finestrat\"," +
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

                    var request = (HttpWebRequest)WebRequest.Create("https://exp.host/--/api/v2/push/send");
                    request.Method = "POST";
                    request.KeepAlive = false;
                    request.Accept = "application/json";
                    request.ContentType = "application/json";
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

        #endregion
    }
}
