using Autoescuelas.Core;
using Autoescuelas.Infrastructure.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Autoescuelas.Infrastructure.Repositories
{
	// http://developer.android.com/google/gcm/index.html
	public class PushIosRepository : IPushSpecificRepository
	{
		//public const string HostName = "gateway.sandbox.push.apple.com";
		public const string HostName = "gateway.push.apple.com";

		//public const string pushCertificate = "...";
		//public const string pushId = "estudioKUKUT2014";

		#region DispositivoTipo
		public DispositivoTipo DispositivoTipo
		{
			get { return DispositivoTipo.iOS; }
		}
		#endregion DispositivoTipo

		#region SendNotification
		public string SendNotification(string pushId, string pushCertificate, IEnumerable<string> targetIds, string message, string relatedName, string relatedId, int notificationId, int sourceId, string sourceNombre)
		{
			foreach (var targetId in targetIds)
				SendNotificationInternal(pushId, pushCertificate, targetId, message, relatedName, relatedId, notificationId, sourceId, sourceNombre);

			return "";
		}
		#endregion SendNotification

		#region SendNotificationInternal
		private string SendNotificationInternal(string pushId, string pushCertificate, string targetId, string message, string relatedName, string relatedId, int notificationId, int sourceId, string sourceNombre)
		{
			try
			{
				var certificate = Convert.FromBase64String(pushCertificate);

				var clientCertificate = new X509Certificate2(
					certificate,
					pushId
				);
				var certificatesCollection = new X509Certificate2Collection(clientCertificate);

				using (var tcpClient = new TcpClient(HostName, 2195))
				{
					using (var sslStream = new SslStream(tcpClient.GetStream()))
					{
						sslStream.AuthenticateAsClient(HostName, certificatesCollection, SslProtocols.Default, false);

						using (var memoryStream = new MemoryStream())
						{
							using (var writer = new BinaryWriter(memoryStream))
							{
								writer.Write((byte)0); //The command
								writer.Write((byte)0); //The first byte of the deviceId length (big-endian first byte)
								writer.Write((byte)32); //The deviceId length (big-endian second byte)
								writer.Write(StringToByte(targetId));

								var payloadBegin = string.Format(
									"{{" +
										"\"aps\":{{" +
											"\"alert\":\"",
									message,
									DateTime.UtcNow.ToString(),
									targetId,
									relatedName,
									relatedId
								);
								var payloadEnd = string.Format(
											"\"," +
											"\"badge\":0," +
											"\"sound\":\"default\"" +
										"}}," +
										"\"class\":\"{3}\"," +
										"\"id\":{4}" +
									"}}",
									message,
									DateTime.UtcNow.ToString(),
									targetId,
									relatedName,
									relatedId
								);

								var messageLength = Encoding.UTF8.GetBytes(message).Length;
								var charsToEliminate = payloadBegin.Length + payloadEnd.Length + messageLength - 255;
								if (charsToEliminate > 0)
									message = message.Substring(0, message.Length - charsToEliminate);
								
								var payload = Encoding.UTF8.GetBytes(payloadBegin + message + payloadEnd);

								writer.Write((byte)0);
								writer.Write((byte)payload.Length);
								writer.Write(payload);
							}

							var array = memoryStream.ToArray();
							sslStream.Write(array);
						}
					}

					return "Ok";
				}
			}
			catch (Exception ex)
			{
				return "Error: " + ex.Message;
			}
		}
		#endregion SendNotificationInternal

		#region StringToByte
		private byte[] StringToByte(string stringData)
		{
			if (stringData == null)
				return null;

			if (stringData.Length % 2 == 1)
				stringData = '0' + stringData; // Up to you whether to pad the first or last byte

			var result = new byte[stringData.Length / 2];
			for (int i = 0; i < result.Length; i++)
				result[i] = Convert.ToByte(stringData.Substring(i * 2, 2), 16);

			return result;
		}
		#endregion StringToByte

		#region ByteToString
		private string ByteToString(byte[] byteData)
		{
			if (byteData == null)
				return null;

			var result = new StringBuilder(byteData.Length * 2);
			foreach (var b in byteData)
				result.AppendFormat("{0:x2}", b);
			return result.ToString();
		}
		#endregion ByteToString
	}
}
