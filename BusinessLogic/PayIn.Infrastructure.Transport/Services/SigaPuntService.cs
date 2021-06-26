using PayIn.Domain.Transport;
using PayIn.Domain.Transport.Eige.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.Infrastructure.Transport.Services
{
	public class SigapuntService
	{
		public int xor(int iValue1, int iValue2)
		{
			var iResult = iValue1 ^ iValue2;
			return iResult;
		}

		public async Task<SigapuntScript> GetData(long uid)
		{
			var result = new SigapuntScript();
			result.Add(await TAREXIAsync(uid));
			result.Add(await RECAsync(uid));
			if (result.CodigoViajero != null)
				result.Add(await CONUSUAsync(result.CodigoViajero.Value));

			return result;
		}

		char STX = (char)0x02;
		char ETX = (char)0x03;
		byte[] bytes = new byte[1024];
#if PRODUCTION
		IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("193.144.127.124"), 1550);
#else
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("193.144.127.124"), 1058);
#endif

        private string GetRequest(string request, string nromen = "")
		{
			//#if TEST
			//			request += "|CODPUN=2";
			//#endif
			if (!nromen.IsNullOrEmpty())
				request += "|NROMEN=" + nromen;

			// CRC
			var userArray = request.ToCharArray();
			var r = xor(userArray[0], 0);
			for (int i = 1; i < userArray.Length; i++)
				r = xor(userArray[i], r);
			var decValue = int.Parse(r.ToString(), System.Globalization.NumberStyles.HexNumber);
			var charValue = (char)decValue;

			var message = userArray.Length + "|" + request + "|" + charValue;
			message = STX + message + ETX;

			return message;
		}
		private Dictionary<string, string> GetResponse(string result)
		{
			if (
				(result.ToCharArray(0, 1)[0] != STX) ||
				(result.ToCharArray(result.Length - 1, 1)[0] != ETX)
			)
				throw new ApplicationException("Bad format SIG>APunT message");

			var temp = result.Substring(1, result.Length - 2).SplitString("|");
			var length = temp.FirstOrDefault();
			var verificador = temp.LastOrDefault();

			var arguments = temp
				.Skip(1)
				.Take(temp.Length - 2)
				.ToDictionary(
					x => x.Contains("=") ? x.SplitString("=")[0] : x,
					x => x.Contains("=") ? x.SplitString("=")[1] : ""
				);

			return CheckError(arguments);
		}
		private async Task<string> Send(string solicitud)
		{
			return await Task.Run(() =>
			{
				try
				{
					var respuesta = "";

					using (Socket sender = new Socket(SocketType.Stream, ProtocolType.Tcp))
					{
						sender.Connect(remoteEP);

						// Conectar
#if TEST
						var message = GetRequest("START|TIPOEMP=C|CODEMP=23|CODPUN=2|USU=payin|CLA=p4y1n4231");
#else
						var message = GetRequest("START|TIPOEMP=C|CODEMP=23|CODPUN=1|USU=payin|CLA=p4y1n4231");
#endif
						var bytesSent = sender.Send(Encoding.ASCII.GetBytes(message));
						var bytesReceived = sender.Receive(bytes);
						respuesta = Encoding.ASCII.GetString(bytes, 0, bytesReceived);


						// Obtener NROMEN
						var arguments = GetResponse(respuesta);

						var nromen = "";
						if (!respuesta.Contains("|ULTMEN=-"))
						{
							var dif = (respuesta.Length - 3) - (respuesta.LastIndexOf("|ULTMEN=") + 8);
							for (int i = 0; i < dif; i++)
								nromen += respuesta[respuesta.LastIndexOf("|ULTMEN=") + 8 + i];
							var intpos = int.Parse(nromen.ToString());
							intpos++;
							nromen = intpos.ToString();
						}

						//Enviar
						message = GetRequest(solicitud, nromen);
						bytesSent = sender.Send(Encoding.ASCII.GetBytes(message));
						bytesReceived = sender.Receive(bytes);
						respuesta = Encoding.ASCII.GetString(bytes, 0, bytesReceived);

						// Desconectar
						message = GetRequest("SOLDES");
						bytesSent = sender.Send(Encoding.ASCII.GetBytes(message));
						bytesReceived = sender.Receive(bytes);

						sender.Shutdown(SocketShutdown.Both);
						sender.Close();
					}
					return respuesta;
				}
				catch (ArgumentNullException ane)
				{
					throw new Exception("ArgumentNullException : " + ane.ToString());
				}
				catch (SocketException se)
				{
					throw new Exception("SocketException : " + se.ToString());
				}
				catch (Exception e)
				{
					throw new Exception("Unexpected exception : " + e.ToString());
				}
			});
		}
		private Dictionary<string, string> CheckError(Dictionary<string, string> arguments)
		{
			var error = arguments.GetOrDefault("ERROR") ?? "";
			var detalle = arguments.GetOrDefault("DETALLE") ?? "";

			if (!error.IsNullOrEmpty())
			{
				if (error == "600")
					throw new CardNotFoundException("GV");

				throw new ApplicationException("ERROR " + error + ": " + detalle);
			}

			return arguments;
		}

		// Lista negra
		public async Task<string> MARCALNAsync(long uid, int concession)
		{
			string solicitud = "SOL|TIPSOL=MARCALN|NROSER=" + uid + "|EMPPRO=" + concession + "|PTO=..";
			return await Send(solicitud);
		}
		public async Task<string> INCLNAsync(long uid, int concession)
		{
			string solicitud = "SOL|TIPSOL=INCLN|NROSER=" + uid + "|EMPPRO=" + concession + "|CODMOT=2047|DETALLE=";
			return await Send(solicitud);
		}
		public async Task<string> RESLNAsync(long uid, int concession)
		{
			string solicitud = "SOL|TIPSOL=RESLN|NROSER=" + uid + "|EMPPRO=" + concession;
			return await Send(solicitud);
		}
		public async Task<string> BLALNAsync(long uid, int concession)
		{
			string solicitud = "SOL|TIPSOL=BLALN|NROSER=" + uid + "|EMPPRO=" + concession;
			return await Send(solicitud);
		}

		// Lista gris
		public async Task<string> BLALGAsync(long uid)
		{
			string solicitud = "SOL|TIPSOL=BLALG|NROSER=" + uid;
			return await Send(solicitud);
		}
		public async Task<string> MARCALGAsync(long uid, string newValue, GreyList.MachineType machine, string field, byte[] changedField)
		{
			var codeOperation = (int)machine;
			string solicitud = "SOL|TIPSOL=MARCALG|NROSER=" + uid + "|NROOPE=" + codeOperation + "|PTO=" + field; // + "=" + changedField;
			return await Send(solicitud);
		}

		// Carga/Recarga
		public async Task<string> CARGAAsync(long uid, int owner, int slot, RechargeType? rechargeType, int? previouscode1, int? previouscode2, int? priority, int titleId, EigeZonaEnum? zona, int? temporalUnities, DateTime? validationDate, decimal? previousBalance, decimal? finalBalance, decimal price, string location, int rate, DateTime chargeDate)
		{
			var locationString = location == null ? "0" : location.ToString();
			var slotNumber = slot + 1; // El móvil retorna 0 o 1
			var chargeDateString = chargeDate.ToString("yyyyMMddHHmmss");
			var validationDateString = validationDate == null ? "" : ((DateTime)validationDate).ToString("yyyyMMddHHmmss");

			var rechargeTypeCode =
				(rechargeType == RechargeType.Charge | rechargeType == RechargeType.Replace) ? 0 :
				rechargeType != RechargeType.Revoke ? 1 :
				0; // nunca entra como revoke

			string solicitud = "SOL|TIPSOL=CARGA" +
				"|NROSER=" + uid +
				"|EMPPRO=" + owner +
				"|TIT=" + slotNumber +
				"|TITCAR=" + rechargeTypeCode +
				"|TITTRAS=" + //La operación de traspaso no está implementada
				"|CODTITTRAS=" + //solo si TITRAS != 0
				"|CODTIT1BOR=" + (previouscode1 == null ? "" : previouscode1.ToString()) +
				"|CODTIT2BOR=" + (previouscode2 == null ? "" : previouscode2.ToString()) +
				"|TITPR=" + (priority == null ? "" : priority.ToString()) +
				"|CODTIT=" + titleId +
				"|VALZON=" + (zona == null ? "00000" : Convert.ToString(Convert.ToByte(zona), 2).PadLeft(5, '0')) +
				"|UNI=" + (temporalUnities == null ? "" : temporalUnities.ToString()) +
				"|FECVAL=" + validationDateString +
				"|SALVIA=" + (previousBalance ?? 0) +
				"|SALVIAD=" + (finalBalance ?? 0) +
				"|IMP=" + price +
				"|FORPAG=0101" +
				"|TIPEQU=0011" +
				"|LOCIDE=" + locationString +
				"|CTRTAR=" + rate +
				"|FECCAR=" + chargeDateString +
				"|V=2";

			return await Send(solicitud);
		}
		public async Task<string> ANUCARGAAsync(long uid, int owner, DateTime chargeDate, int titleId, EigeZonaEnum? zone, int slot)
		{
			var zoneBytes = zone == null ? "" : Convert.ToString(Convert.ToByte(zone), 2).PadLeft(5, '0');
			var slotNumber = slot + 1;

			string solicitud = "SOL|TIPSOL=ANUCARGA" +
				"|NROSER=" + uid +
				"|EMPPRO=" + owner +
				"|CODTIT=" + titleId +
				"|VALZON=" + zoneBytes +
				"|FECCAR=" + chargeDate.ToString("yyyyMMddHHmmss") +
				"|TIT=" + slotNumber +
				"|V=2";
			return await Send(solicitud);
		}

		public async Task<Dictionary<string, string>> TAREXIAsync(long uid)
		{
			string solicitud = "SOL|TIPSOL=TAREXI|NROSER=" + uid + "|V=3";
			var result = await Send(solicitud);

			if (
				(result.ToCharArray(0, 1)[0] == STX) &&
				(result.ToCharArray(result.Length - 1, 1)[0] == ETX)
			)
			{
				var temp = result.Substring(1, result.Length - 2).SplitString("|");
				var length = temp.FirstOrDefault();
				var verificador = temp.LastOrDefault();

				var arguments = temp
					.Skip(1)
					.Take(temp.Length - 2)
					.ToDictionary(
						x => x.Contains("=") ? x.SplitString("=")[0] : x,
						x => x.Contains("=") ? x.SplitString("=")[1] : ""
					);

				return CheckError(arguments);
			}

			// 78|RES|TIPSOL=TAREXI|NROSER=-1|DETALLE= Tarjeta No encontrada|ERROR=600|NROMEN=-1|a
			// 91|RES|TIPSOL=TAREXI|NROSER=-1|DETALLE=la tarjeta principal en Lista Negra|ERROR=609|NROMEN=-1|

			//RES|TIPSOL=TAREXI|CODVIA=2|TIPO=0010|SUBTI=|VER=2|FECAD=20090101|TIT1PRI=12|TIT2PRI=13|MONPRI=14|BONPRI=15|CODTIT1=|CODTIT2=|SALMON=|SALBON=|TIT1ACT=0|TIT2ACT=0|MONACT=0|BONACT=1|NROMEN=111

			return null;
		}
		public async Task<Dictionary<string, string>> CONUSUAsync(long viajero)
		{
			string solicitud = "SOL|TIPSOL=CONUSU|CODVIA=" + viajero + "|V=3";
			var result = await Send(solicitud);

			if (
				(result.ToCharArray(0, 1)[0] == STX) &&
				(result.ToCharArray(result.Length - 1, 1)[0] == ETX)
			)
			{
				var temp = result.Substring(1, result.Length - 2).SplitString("|");
				var length = temp.FirstOrDefault();
				var verificador = temp.LastOrDefault();

				var arguments = temp
					.Skip(1)
					.Take(temp.Length - 2)
					.ToDictionary(
						x => x.Contains("=") ? x.SplitString("=")[0] : x,
						x => x.Contains("=") ? x.SplitString("=")[1] : ""
					);

				return CheckError(arguments);
			}

			return null;
		}
		public async Task<Dictionary<string, string>> RECAsync(long uid)  //Revisar - UID nuevo y antiguo iguales
		{
			string solicitud = "SOL|TIPSOL=REC|NROSER=" + uid + "|NROSER2=" + uid + "|INCLN=0";
			var result = await Send(solicitud);

			// 350|RES|TIPSOL=REC|NROSER=1499933674|CODVIA=6542|TIPO=0010|SUBTI=0101|VER=|OPPER=011101010000|EMPPRO=1|GRUFAM=|FECAD=20210608|TITACT=0001|TIT1PRI=|TIT2PRI=|MONPRI=0|BONPRI=0|TITUSO=|LISNEG=|SEG=0|CODTIT1=|TIPTA1=|CONTA1=|VALZO1=|UNITE1=|ESTAM1=0|FECVA1=|SALVI1=|CODTIT2=|TIPTA2=|CONTA2=|VALZO2=|UNITE2=|ESTAM2=0|FECVA2=|SALVI2=|SALMON=|SALBON=|NROMEN=105| 

			if (
				(result.ToCharArray(0, 1)[0] == STX) &&
				(result.ToCharArray(result.Length - 1, 1)[0] == ETX)
			)
			{
				var temp = result.Substring(1, result.Length - 2).SplitString("|");
				var length = temp.FirstOrDefault();
				var verificador = temp.LastOrDefault();

				var arguments = temp
					.Skip(1)
					.Take(temp.Length - 2)
					.ToDictionary(
						x => x.Contains("=") ? x.SplitString("=")[0] : x,
						x => x.Contains("=") ? x.SplitString("=")[1] : ""
					);

				return CheckError(arguments);
			}

			return null;
		}
	}
}
