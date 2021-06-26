using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ScrapySharp.Network;
using ScrapySharp.Extensions;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using PayIn.Domain.Transport.Eige.Enums;
using Xp.Infrastructure.Http;
using PayIn.Domain.Transport;
using Xp.Domain.Transport;
using static Xp.Infrastructure.Http.HttpServer;

namespace PayIn.Infrastructure.Transport.Services
{
	public class FgvService
	{
		#region GetData
		public async Task<FgvScript> GetData(long uid)
		{
			return await Task.Run(() =>
			{
				//http://www.codeproject.com/Articles/1041115/Webscraping-with-Csharp
				var browser = new ScrapingBrowser();

				var pageResult = browser.NavigateToPage(
					new Uri("http://www.metrovalencia.es/tools_consulta_tsc.php"),
					HttpVerb.Post,
					"tsc={0}".FormatString(uid),
					"application/x-www-form-urlencoded"
				);
				var text = pageResult.Html.CssSelect(".tools p")
					.Last()
					.InnerHtml
					.Replace("\n", " ")
					.Replace("\r", " ")
					.Replace("  ", " ")
					.Replace("  ", " ");

				if (text.Trim().StartsWith("No se han encontrado datos de la tarjeta."))
					return null;

				var resultBonometro = GetData_Saldo(uid, text);
				if (resultBonometro != null)
					return resultBonometro;

				resultBonometro = GetData_Caducidad(uid, text);
				if (resultBonometro != null)
					return resultBonometro;

				resultBonometro = GetData_TarifaCaducidaConFecha(text);
				if (resultBonometro != null)
					return resultBonometro;

				resultBonometro = GetData_TarifaCaducida(text);
				if (resultBonometro != null)
					return resultBonometro;

				var resultTuin = GetData_Tuin(uid, text);
				if (resultTuin != null)
					return resultTuin;

				throw new ScrapFormatException("FGV", uid, text);
			});
		}
		public FgvScript GetData_Saldo(long uid, string text)
		{
			var regex = Regex.Match(text, @"T??tulo: (.*) zona ([ABCD]*)<br>Saldo de viajes: (\d*) <br>");
			if (regex.Success)
			{
				var result = new FgvScript();
				result.Add(
					new Dictionary<string, string>
					{
						{ "FGV", text },
						{ "FGV_SALDO", regex.Groups[3].Value },
						{ "FGV_SALDOACUMULADO", "" },
						{ "FGV_SALDOUNIDADES", "v" },
						{ "FGV_TITULOCODIGO", GetTitleCode(uid, text, regex.Groups[1].Value )},
						{ "FGV_TITULONOMBRE", regex.Groups[1].Value },
						{ "FGV_TITULOZONA", regex.Groups[2].Value },
						{ "FGV_TITULOCADUCIDAD", "" }
					}
				);
				return result;
			}

			return null;
		}
		public FgvScript GetData_Caducidad(long uid, string text)
		{
			// T??tulo: Pase FGV zona ABCD<br>Fecha de caducidad: Sin fecha de caducidad<br><a href="https://www.fgv.es/secure/movimientos_tsc.php?tsc=686075813" target="_blank">Consulte los movimientos de la tarjeta</a>
			var regex = Regex.Match(text, @"T\?\?tulo: (.*) zona ([ABCD]*)<br>Fecha de caducidad: (Sin fecha de caducidad|[\/\d]*)?<br>");
			if (regex.Success)
			{
				var result = new FgvScript();
				result.Add(
					new Dictionary<string, string>
					{
						{ "FGV", text },
						{ "FGV_SALDO", "" },
						{ "FGV_SALDOACUMULADO", "" },
						{ "FGV_SALDOUNIDADES", "" },
						{ "FGV_TITULOCODIGO", GetTitleCode(uid, text, regex.Groups[1].Value )},
						{ "FGV_TITULONOMBRE", regex.Groups[1].Value },
						{ "FGV_TITULOZONA", regex.Groups[2].Value },
						{ "FGV_TITULOCADUCIDAD", regex.Groups[3].Value.Contains("/") ? regex.Groups[3].Value : "" }
					}
				);
				return result;
			}

			return null;
		}
		public FgvScript GetData_TarifaCaducidaConFecha(string text)
		{
			// T??tulo: Bonometro zona A<br>T??tulo con tarifa caducada. Puede realizar el canje de los viajes en los <a title='http://www.metrovalencia.es/wordpress/?page_id=5' href='http://www.metrovalencia.es/wordpress/?page_id=5' target='_blank'>CAC y Punts del Client</a> hasta el 31/03/2014<a href="https://www.fgv.es/secure/movimientos_tsc.php?tsc=1250779165" target="_blank">Consulte los movimientos de la tarjeta</a>
			// T??tulo: Pase FGV zona ABCD<br>Fecha de caducidad: Sin fecha de caducidad<br><a href="https://www.fgv.es/secure/movimientos_tsc.php?tsc=686075813" target="_blank">Consulte los movimientos de la tarjeta</a>
			var regex = Regex.Match(text, @"T\?\?tulo: (.*) zona ([ABCD]*)<br>T\?\?tulo con tarifa caducada(.*)CAC y Punts del Client</a> hasta el ([\/\d]*)<a href=");
			if (regex.Success)
			{
				throw new ScrapExpiredPriceException(
					regex.Groups[1].Value,
					regex.Groups[2].Value,
					regex.Groups[4].Value.Contains("/") ? regex.Groups[4].Value : ""
				);
			}

			return null;
		}
		public FgvScript GetData_TarifaCaducida(string text)
		{
			// T??tulo: Bonometro zona A<br>T??tulo con tarifa caducada. Puede realizar el canje de los viajes en los <a title='http://www.metrovalencia.es/wordpress/?page_id=5' href='http://www.metrovalencia.es/wordpress/?page_id=5' target='_blank'>CAC y Punts del Client</a> hasta el 31/03/2014<a href="https://www.fgv.es/secure/movimientos_tsc.php?tsc=1250779165" target="_blank">Consulte los movimientos de la tarjeta</a>
			// T??tulo: Pase FGV zona ABCD<br>Fecha de caducidad: Sin fecha de caducidad<br><a href="https://www.fgv.es/secure/movimientos_tsc.php?tsc=686075813" target="_blank">Consulte los movimientos de la tarjeta</a>
			var regex = Regex.Match(text, @"T\?\?tulo: (.*) zona ([ABCD]*)<br>T\?\?tulo con tarifa caducada");
			if (regex.Success)
			{
				throw new ScrapExpiredPriceException(
					regex.Groups[1].Value,
					regex.Groups[2].Value
				);
			}

			return null;
		}
		public FgvScript GetData_Tuin(long uid, string text)
		{
			var regex = Regex.Match(text, @"T\?\?tulo: (.*)<br>Saldo: ([-,\d]*) \?\?\? (?:\(Acumulado mensual: ([-,\d]*) \?\?\?\))?<br>");
			if (regex.Success)
			{
				var result = new FgvScript();
				result.Add(
					new Dictionary<string, string>
					{
						{ "FGV", text },
						{ "FGV_SALDO", regex.Groups[2].Value },
						{ "FGV_SALDOACUMULADO", regex.Groups[3].Value },
						{ "FGV_SALDOUNIDADES", "€" },
						{ "FGV_TITULOCODIGO", GetTitleCode(uid, text, regex.Groups[1].Value )},
						{ "FGV_TITULONOMBRE", regex.Groups[1].Value },
						{ "FGV_TITULOZONA", "" },
						{ "FGV_TITULOCADUCIDAD", "" }
					}
				);
				return result;
			}

			return null;
		}
		#endregion GetData

		#region GetTitleCountAsync
		public async Task<int> GetTitleCountAsync(long uid, FgvScript script)
		{
			return await Task.Run(() =>
			{
				return script.TituloNombre.IsNullOrEmpty() ? 0 : 1;
			});
		}
		#endregion GetTitleCountAsync

		#region GetTitleNameAsync
		public async Task<string> GetTitleNameAsync(long uid, FgvScript script, int index)
		{
			return await Task.Run(() =>
			{
				return script.TituloNombre;
			});
		}
		#endregion GetTitleNameAsync

		#region GetTitleZoneNameAsync
		public async Task<EigeZonaEnum?> GetTitleZoneNameAsync(long uid, FgvScript script, int index)
		{
			return await Task.Run(() =>
			{
				var zonas = script.TituloZona.ToLower();
				EigeZonaEnum result = 0;

				if (zonas.Contains("a"))
					result |= EigeZonaEnum.A;
				if (zonas.Contains("b"))
					result |= EigeZonaEnum.B;
				if (zonas.Contains("c"))
					result |= EigeZonaEnum.C;
				if (zonas.Contains("d"))
					result |= EigeZonaEnum.D;

				return result == 0 ? (EigeZonaEnum?)null : result;
			});
		}
		#endregion GetTitleZoneNameAsync

		#region GetTitleHasBalanceAsync
		public async Task<bool> GetTitleHasBalanceAsync(long uid, FgvScript script, int index)
		{
			return await Task.Run(() => {
				return (
					(script.SaldoUnidadesViaje.ToLower() == "v") ||
					(script.SaldoUnidadesViaje.ToLower() == "€")
				);
			});
		}
		#endregion GetTitleHasBalanceAsync

		#region GetTitleBalanceAsync
		public async Task<decimal?> GetTitleBalanceAsync(long uid, FgvScript script, int index)
		{
			return await Task.Run(() =>
			{
				return script.SaldoViaje;
			});
		}
		#endregion GetTitleBalanceAsync

		#region GetTitleBalanceAcumulatedAsync
		public async Task<decimal?> GetTitleBalanceAcumulatedAsync(long uid, FgvScript script, int index)
		{
			return await Task.Run(() =>
			{
				return script.SaldoAcumuladoViaje;
			});
		}
		#endregion GetTitleBalanceAcumulatedAsync

		#region GetTitleIsExhaustedAsync
		public async Task<bool> GetTitleIsExhaustedAsync(long uid, FgvScript script, int index, DateTime now)
		{
			return
				(
					(await GetTitleHasBalanceAsync(uid, script, index)) &&
					((await GetTitleBalanceAsync(uid, script, index)) == 0)
				) || (
					(await GetTitleIsTemporalAsync(uid, script, index)) &&
					((await GetTitleExhaustedDateAsync(uid, script, index)) != null) &&
					((await GetTitleExhaustedDateAsync(uid, script, index)) < now)
				)
			;
		}
		#endregion GetTitleIsExhaustedAsync

		#region GetTitleIsExpiredAsync
		public async Task<bool> GetTitleIsExpiredAsync(long uid, FgvScript script, int index, DateTime now)
		{
			return await Task.Run(() => false);
		}
		#endregion GetTitleIsExhaustedAsync

		#region GetTitleBalanceUnitsAsync
		public async Task<string> GetTitleBalanceUnitsAsync(long uid, FgvScript script, int index)
		{
			return await Task.Run(() =>
			{
				return script.SaldoUnidadesViaje;
			});
		}
		#endregion GetTitleBalanceUnitsAsync

		#region GetTitleIsTemporalAsync
		public async Task<bool> GetTitleIsTemporalAsync(long uid, FgvScript script, int index)
		{
			return await Task.Run(() =>
			{
				return script.SaldoUnidadesViaje.ToLower() != "v";
			});
		}
		#endregion GetTitleIsTemporalAsync

		#region GetTitleExhaustedDateAsync
		public async Task<DateTime?> GetTitleExhaustedDateAsync(long uid, FgvScript script, int index)
		{
			return await Task.Run(() =>
			{
				return script.FechaCaducidad;
			});
		}
		#endregion GetTitleExhaustedDateAsync

		#region GetTitleCodeAsync
		public async Task<int?> GetTitleCodeAsync(long uid, FgvScript script, int index)
		{
			return await Task.Run(() =>
			{
				return script.TituloCodigo;
			});
		}
		#endregion GetTitleCodeAsync

		#region GetTitleCode
		public string GetTitleCode(long uid, string text, string nombre)
		{
			if (nombre == "Bonometro") return "1003";
			if (nombre == "Sencillo") return "1001";
			if (nombre == "TuiN") return "1271";
			if (nombre == "Ida y Vuelta") return "1002";
            if (nombre == "Reduccion 50%") return "";
            if (nombre == "Bono-2") return "";
            if (nombre == "TuiN 50FN") return "1273";
			if (nombre == "TuiN 20FN") return "1272";
            if (nombre == "TuiN Mensual") return "1274";
            if (nombre == "Bono Transbordo") return "1552";
            if (nombre == "Gent Major") return "2848";

            throw new ScrapFormatException("FGV", uid, text);
		}
        #endregion GetTitleCode

        //private string BaseUrl = "http://193.145.205.103/sc/webservices/";
        private string BaseUrl = "https://www.metrovalencia.es/ap18/redext/";
        private int PayinCode = 23;
		private string GetDateTimeString(DateTime? fecha)
		{
			return fecha?.ToString("d/M/yyyy_H:m:s") ?? "";
		}

		#region Login
		public class RedextLoginResult
		{
			public int ERROR { get; set; }
			public string RESULTADO { get; set; }
		}
		public async Task<RedextLoginResult> RedextLoginAsync()
		{
			var server = new HttpServer(BaseUrl, "");
			var result = await server.PostAsync<RedextLoginResult>("redext.login", encoding: HttpServerEncoding.UrlEncoding, arguments: new
			{
				RED = PayinCode,
				PASSWORD = ""
				//SEDE = sede
			});

			return result;
		}
		#endregion Login

		#region Listado Lista gris
		public class RedextListaGrisResult_Resultado
		{
			public int ID { get; set; }
			public long NUM_SERIE { get; set; }
			public int NUM_OPERACION { get; set; }
			public int ACCION { get; set; }
			public string CAMPO { get; set; }
			public string VALOR { get; set; }
			public int RESUELTO { get; set; }
		}
		public class RedextListaGrisResult
		{
			public int ERROR { get; set; }
			public IEnumerable<RedextListaGrisResult_Resultado> RESULTADO { get; set; }
		}
		public async Task<RedextListaGrisResult> RedextListaGrisAsync(string sede)
		{
			var server = new HttpServer(BaseUrl, "");
			var result = await server.PostAsync<RedextListaGrisResult>("redext.lista_gris", encoding: HttpServerEncoding.UrlEncoding, arguments: new
			{
				SEDE = sede
			});

			return result;
		}
		#endregion Listado Lista gris

		#region Listado Lista negra
		public class RedextListaNegraResult_Resultado
		{
			public long ID { get; set; }
			public long NUM_SERIE { get; set; }
			public int RECHAZO { get; set; }
		}
		public class RedextListaNegraResult
		{
			public int ERROR { get; set; }
			public IEnumerable<RedextListaNegraResult_Resultado> RESULTADO { get; set; }
		}
		public async Task<RedextListaNegraResult> RedextListaNegraAsync(string sede)
		{
			var server = new HttpServer(BaseUrl, "");
			var result = await server.PostAsync<RedextListaNegraResult>("redext.lista_negra", encoding: HttpServerEncoding.UrlEncoding, arguments: new
			{
				SEDE = sede
			});
			
			return result;
		}
		#endregion Listado Lista negra

		#region Ejecuta Lista gris
		public class RedextEjecutalistaGrisResult
		{
			public int ERROR { get; set; }
			public string RESULTADO { get; set; }
		}
		public async Task<RedextEjecutalistaGrisResult> RedextEjecutarListaGrisAsync(/*int id, DateTime fecha,*/ int resuelto, string sede)
		{
			var server = new HttpServer(BaseUrl, "");
			var result = await server.PostAsync<RedextEjecutalistaGrisResult>("redext.ejecuta_lista_gris", encoding: HttpServerEncoding.UrlEncoding, arguments: new
			{
				//ID = id,
				//FECHA = GetDateTimeString(fecha),
				RESUELTO = resuelto,
				SEDE = sede
			});

			return result;
		}
		#endregion Ejecuta Lista gris

		#region Ejecuta Lista negra
		public class RedextEjecutaListaNegraResult
		{
			public int ERROR { get; set; }
			public string RESULTADO { get; set; }
		}
		public async Task<RedextEjecutaListaNegraResult> RedextEjecutarListaNegraAsync(/*int id, DateTime fecha,*/ string sede)
		{
			var server = new HttpServer(BaseUrl, "");
			var result = await server.PostAsync<RedextEjecutaListaNegraResult>("redext.ejecutar_lista_negra", encoding: HttpServerEncoding.UrlEncoding, arguments: new
			{
				//ID = id,
				//FECHA = GetDateTimeString(fecha),
				SEDE = sede
			});

			return result;
		}
		#endregion Ejecuta Lista negra

		#region Ejecuta Operación
        public enum RedextOperacionType
        {
            Recharge = 7,
            Refound = 11
        }
        public class RedextOperacionResult
		{
			public int ERROR { get; set; }
			public int RESULTADO { get; set; }
		}
		public async Task<string> RedextOperacionAsync(
            DateTime fecha, long? uid, EigeCodigoEntornoTarjetaEnum? entorno, long titleCode,
            EigeZonaEnum? zonas, DateTime? fechaValidez, int? saldo, bool ampliado, int tarifa,
            OperationType tipoOperacion, EigeTituloEnUsoEnum? slot, decimal importe, EigeFormaPagoEnum formaPago,
            int emision, bool esCorrecto, string mapaInicial, string mapaFinal, string autorizacion, int operationId,
            int? usuarioId, RedextOperacionType operationType)
		{
			var server = new HttpServer(BaseUrl, "");
			var result = await server.PostAsync<RedextOperacionResult>("redext.operacion", encoding: HttpServerEncoding.UrlEncoding, arguments: new
			{
				SEDE = entorno == EigeCodigoEntornoTarjetaEnum.Alicante ? "A" : "V",
				FECHA_OP = GetDateTimeString(fecha),
				EQUIPO_ID = PayinCode,
				NUM_SERIE = uid,
				TITULO_ID = titleCode,
				COMB_ZONAL = zonas?.JoinString("") ?? "ABCD",
				FECHA_VALIDEZ = GetDateTimeString(fechaValidez),
				SALDO = saldo,
				AMPLIADO = ampliado ? 1 : 0,
				TARIFA = tarifa,
				TIPO_OP = (int)operationType,
				POSICION_TITULO =
					slot == EigeTituloEnUsoEnum.Titulo1 ? 1 :
					slot == EigeTituloEnUsoEnum.Titulo2 ? 2 :
					0,
				IMPORTE = (int) Math.Truncate(importe * 100),
				FORMA_PAGO = formaPago,
				EMISION = emision,
				ESTADO = esCorrecto ? 0 : 1,
				MAPA_INICIAL = mapaInicial,
				MAPA_FINAL = mapaFinal,
                EQUIPO_OP = operationId.ToString() ?? "",
                AUTORIZACION = autorizacion ?? "",
                USUARIO_ID = usuarioId?.ToString() ?? ""
			});
			if (result.ERROR != 0)
				throw new Exception("ERROR: " + result.ToJson());

			return result.ToJson();
		}
		#endregion Ejecuta Operación
	}
}
