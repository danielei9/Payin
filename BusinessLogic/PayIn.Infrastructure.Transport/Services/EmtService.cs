using System;
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
using PayIn.Domain.Transport.MifareClassic;
using PayIn.Domain.Transport;
using Xp.Infrastructure;
using Xp.Domain;
using Xp.Infrastructure.Repositories;

namespace PayIn.Infrastructure.Transport.Services
{
	public class EmtService
	{
		private readonly AzureBlobRepository AzureBlobRepository;
		private readonly IEntityRepository<TransportTitle> TitleRepository;

		#region Contructors
		public EmtService(
			AzureBlobRepository azureblobRepository,
			IEntityRepository<TransportTitle> titleRepository
		)
		{
			if (azureblobRepository == null) throw new ArgumentNullException("blobRepository");
			if (titleRepository == null) throw new ArgumentNullException("titleRepository");

			AzureBlobRepository = azureblobRepository;
			TitleRepository = titleRepository;
		}

		#endregion Constructors

		#region GetData
		public async Task<EmtScript> GetData(long uid)
		{
			return await Task.Run(() =>
			{
				//http://www.codeproject.com/Articles/1041115/Webscraping-with-Csharp
				var browser = new ScrapingBrowser();
				var pageResult = browser.NavigateToPage(
					new Uri("http://www.emtvalencia.es/ciudadano/modules/mod_saldo/busca_saldo.php"),
					HttpVerb.Post,
					"numero={0}&idioma=es".FormatString(
						uid + TransportCardSupport.GetUidCrc(BitConverter.GetBytes((int)uid).ToHexadecimal())
					),
					"application/x-www-form-urlencoded"
				);
				var node = pageResult.Html.CssSelect("input");
				if ((node == null) || (node.Count() == 0))
					throw new CardNotFoundException("EMT");

				var text = node
					.First()
					.GetAttributeValue("value");

				text = text
					//.Encode('utf-8')
					.Replace("\n", " ")
					.Replace("\r", " ")
					.Replace("  ", " ")
					.Replace("  ", " ");

				{
					var resultBonobus = GetData_Bonobus_Caducado(uid, text);
					if (resultBonobus != null)
						return resultBonobus;
				}

				{
					var resultBonobus = GetData_Bonobus_Caducados(uid, text);
					if (resultBonobus != null)
						return resultBonobus;
				}

				{
					var resultBonobus = GetData_Bonobus(uid, text);
					if (resultBonobus != null)
						return resultBonobus;
				}

				throw new ScrapFormatException("EMT", uid, text);
			});
		}
		public EmtScript GetData_Bonobus_Caducado(long uid, string text)
		{
			var regex = Regex.Match(text, @"A la tarjeta ([\d ]*) le quedaban? (\d*) viajes? (.*) ya CADUCADO,? despu\?\?s de ser utilizada el ([\d\/]*) a las ([\d:]*) En el caso de haberla");
			if (regex.Success)
			{
				var result = new EmtScript();
				result.Add(
					new Dictionary<string, string>
					{
					{ "EMT", text },
					{ "EMT_UID", regex.Groups[1].Value.Replace(" ", "") },
					{ "EMT_SALDO", regex.Groups[2].Value },
					{ "EMT_SALDOUNIDADES", "v" },
					{ "EMT_TITULOCODIGO", GetTitleCode(uid, text, regex.Groups[3].Value )},
					{ "EMT_TITULONOMBRE", regex.Groups[3].Value },
					{ "EMT_TITULOCADUCADO", "1" },
					{ "EMT_ULTIMAVALIDACION", regex.Groups[4].Value + " " + regex.Groups[5].Value }
					}
				);
				return result;
			}

			return null;
		}
		public EmtScript GetData_Bonobus_Caducados(long uid, string text)
		{
			var regex = Regex.Match(text, @"A la tarjeta ([\d ]*) le quedaban? (\d*) viajes? (.*) ya CADUCADOS,? despu\?\?s de ser utilizada el ([\d\/]*) a las ([\d:]*) En el caso de haberla");
			if (regex.Success)
			{
				var result = new EmtScript();
				result.Add(
					new Dictionary<string, string>
					{
					{ "EMT", text },
					{ "EMT_UID", regex.Groups[1].Value.Replace(" ", "") },
					{ "EMT_SALDO", regex.Groups[2].Value },
					{ "EMT_SALDOUNIDADES", "v" },
					{ "EMT_TITULOCODIGO", GetTitleCode(uid, text, regex.Groups[3].Value )},
					{ "EMT_TITULONOMBRE", regex.Groups[3].Value },
					{ "EMT_TITULOCADUCADO", "1" },
					{ "EMT_ULTIMAVALIDACION", regex.Groups[4].Value + " " + regex.Groups[5].Value }
					}
				);
				return result;
			}

			return null;
		}
		public EmtScript GetData_Bonobus(long uid, string text)
		{
			var regex = Regex.Match(text, @"A la tarjeta ([\d ]*) le quedaban? (\d*) viajes? (.*)( ya CADUCADOS,)?( ya CADUCADO,)? despu\?\?s de ser utilizada el ([\d\/]*) a las ([\d:]*) En el caso de haberla");
			if (regex.Success)
			{
				var tituloNombre = regex.Groups[3].Value;
				var tituloCaducado = tituloNombre.EndsWith("ya caducados");
				if (tituloCaducado)
					tituloNombre = tituloNombre.Substring(0, 13);

				var result = new EmtScript();
				result.Add(
					new Dictionary<string, string>
					{
					{ "EMT", text },
					{ "EMT_UID", regex.Groups[1].Value.Replace(" ", "") },
					{ "EMT_SALDO", regex.Groups[2].Value },
					{ "EMT_SALDOUNIDADES", "v" },
					{ "EMT_TITULOCODIGO", GetTitleCode(uid, text, tituloNombre )},
					{ "EMT_TITULONOMBRE", tituloNombre },
					{ "EMT_TITULOCADUCADO",
							!regex.Groups[4].Value.IsNullOrEmpty() ? "1" :
							!regex.Groups[5].Value.IsNullOrEmpty() ? "1" :
							"0" },
					{ "EMT_ULTIMAVALIDACION", regex.Groups[6].Value + " " + regex.Groups[7].Value }
					}
				);
				return result;
			}

			return null;
		}
		#endregion GetData

		#region GetTitleCountAsync
		public async Task<int> GetTitleCountAsync(long uid, EmtScript script)
		{
			return await Task.Run(() =>
			{
				return script.TituloNombreEMT.IsNullOrEmpty() ? 0 : 1;
			});
		}
		#endregion GetTitleCountAsync

		#region GetTitleNameAsync
		public async Task<string> GetTitleNameAsync(long uid, EmtScript script, int index)
		{
			return await Task.Run(() =>
			{
				return script.TituloNombreEMT;
			});
		}
		#endregion GetTitleNameAsync

		#region GetTitleZoneNameAsync
		public async Task<EigeZonaEnum?> GetTitleZoneNameAsync(long uid, EmtScript script, int index)
		{
			return await Task.Run(() =>
			{
				return (EigeZonaEnum?)null;
			});
		}
		#endregion GetTitleZoneNameAsync

		#region GetTitleHasBalanceAsync
		public async Task<bool> GetTitleHasBalanceAsync(long uid, EmtScript script, int index)
		{
			return await Task.Run(() =>
			{
				return script.SaldoUnidadesViajeEMT.ToLower() == "v";
			});
		}
		#endregion GetTitleHasBalanceAsync

		#region GetTitleBalanceAsync
		public async Task<decimal?> GetTitleBalanceAsync(long uid, EmtScript script, int index)
		{
			return await Task.Run(() =>
			{
				return script.SaldoViajeEMT;
			});
		}
		#endregion GetTitleBalanceAsync

		#region GetTitleIsExhaustedAsync
		public async Task<bool> GetTitleIsExhaustedAsync(long uid, EmtScript script, int index, DateTime now)
		{
			return (await GetTitleHasBalanceAsync(uid, script, index)) ?
				(await GetTitleBalanceAsync(uid, script, index)) == 0 :
				false;
		}
		#endregion GetTitleIsExhaustedAsync

		#region GetTitleIsExpiredAsync
		public async Task<bool> GetTitleIsExpiredAsync(long uid, EmtScript script, int index, DateTime now)
		{
			return await Task.Run(() =>
			{
				return script.TituloCaducado == null ? false : script.TituloCaducado.Value;
			});
		}
		#endregion GetTitleIsExhaustedAsync

		#region GetTitleBalanceUnitsAsync
		public async Task<string> GetTitleBalanceUnitsAsync(long uid, EmtScript script, int index)
		{
			return await Task.Run(() =>
			{
				return script.SaldoUnidadesViajeEMT;
			});
		}
		#endregion GetTitleBalanceUnitsAsync

		#region GetTitleIsTemporalAsync
		public async Task<bool> GetTitleIsTemporalAsync(long uid, EmtScript script, int index)
		{
			return await Task.Run(() =>
			{
				return script.SaldoUnidadesViajeEMT.ToLower() != "v";
			});
		}
		#endregion GetTitleIsTemporalAsync

		/// <summary>
		/// Genera la operación en el fichero de EMT - TODO
		/// </summary>
		/// <param name="uid"></param>
		/// <param name="script"></param>
		/// <returns></returns>
		#region PostEMTFileOperationsAsync
		public async Task<bool> PostEMTFileOperationsAsync(string entry)
		{
            DateTime now = DateTime.Now.ToUTC();
			string containerName = "emtfiles";
			string year = now.Year.ToString();
			string month = (now.Month < 10) ? "0" + now.Month.ToString() : now.Month.ToString();
			string day = (now.Day < 10) ? "0" + now.Day.ToString() : now.Day.ToString();

			var fileExists = await AzureBlobRepository.FileCreateAsync(year + month + day, containerName,entry);

			return fileExists;
		}		
		#endregion PostEMTFileOperationsAsync

		#region FormatDateEMT
		public string FormatDateEMT(DateTime date)
		{			
			var year = date.Year.ToString();
			var month = (date.Month < 10) ? "0" + date.Month.ToString() : date.Month.ToString();
			var day = (date.Day < 10)? "0" + date.Day.ToString() : date.Day.ToString();
			var hour = (date.Hour < 10) ? "0" + date.Hour.ToString() : date.Hour.ToString();
			var minute = (date.Minute < 10) ? "0" + date.Minute.ToString() : date.Minute.ToString();
			var second = (date.Second < 10) ? "0" + date.Second.ToString() : date.Second.ToString();

			return year + month + day + hour + minute + second;
		}
		#endregion FormatDateEMT

		#region OperationCodeEMT
		public int OperationCodeEMT(TransportOperation operation)
		{
			int code = 0;
			if (operation.OperationType == Xp.Domain.Transport.OperationType.Charge)
				code = 2;
			else if (operation.OperationType == Xp.Domain.Transport.OperationType.Recharge)
			{
				if (operation.RechargeType == RechargeType.Recharge || operation.RechargeType == RechargeType.Charge )
					code = 2;
				else if (operation.RechargeType == RechargeType.RechargeAndUpdatePrice || operation.RechargeType == RechargeType.RechargeExpiredPrice)
					code = 3;
				else if (operation.RechargeType == RechargeType.Replace)
					code = 4;
				else if (operation.RechargeType == RechargeType.Revoke)
					code = 5;
			}
			else if (operation.OperationType == Xp.Domain.Transport.OperationType.Revoke)
				code = 5;
				

			return code;
		}
		#endregion OperationCodeEMT

		#region GetSaldoViajes
		public int GetSaldoViajes(dynamic script)
		{
			int viajes = 0;
			if (script.Card.Titulo.CodigoTitulo1.Value == 96) viajes = script.Card.Titulo.SaldoViaje1.Value;
			else if (script.Card.Titulo.CodigoTitulo2.Value == 96) viajes = script.Card.Titulo.SaldoViaje2.Value;

			return viajes;
		}
		#endregion GetSaldoViajes

		#region GetValidity
		/// <summary>
		/// Solo devuelve fecha de validez si es título personalizado
		/// </summary>
		/// <param name="script"></param>
		/// <returns></returns>
		public string GetValidity(dynamic script)
		{
			// por ahora nunca es título personalizado
			string date = "";

			//if (script.Card.Tarjeta.Tipo.Value == 6 || script.Card.Tarjeta.Tipo.Value == 9 || script.Card.Tarjeta.Tipo.Value == 2)
			//{
			//	DateTime dateCard = script.Card.Tarjeta.Caducidad.Value;
			//	if (dateCard != null)
			//	{
			//		var year = dateCard.Year.ToString();
			//		var month = (dateCard.Month < 10) ? "0" + dateCard.Month.ToString() : dateCard.Month.ToString();
			//		var day = (dateCard.Day < 10) ? "0" + dateCard.Day.ToString() : dateCard.Day.ToString();
			//		var hour = (dateCard.Hour < 10) ? "0" + dateCard.Hour.ToString() : dateCard.Hour.ToString();
			//		var minute = (dateCard.Minute < 10) ? "0" + dateCard.Minute.ToString() : dateCard.Minute.ToString();
			//		var second = (dateCard.Second < 10) ? "0" + dateCard.Second.ToString() : dateCard.Second.ToString();

			//		date = year + month + day + hour + minute + second;
			//	}
			//}
			return date;
		}
		#endregion GetValidity

		#region GetAmpliated
		public string GetAmpliated(dynamic script)
		{
			string ampliated = "0";
			if (script.Card.Titulo.CodigoTitulo1.Value == 96 && script.Card.Titulo.TituloEnAmpliacion1.Value == true)  ampliated = "1";
			else if (script.Card.Titulo.CodigoTitulo2.Value == 96 && script.Card.Titulo.TituloEnAmpliacion2.Value == true) ampliated = "1";
			return ampliated;
		}
		#endregion GetAmpliated

		#region GetCodViajero
		public string GetCodViajero(dynamic script)
		{
			//Por ahora vacío
			string codViajero = "";
			//if (script.Card.Tarjeta.Tipo.Value == EigeTipoTarjetaEnum.PasePersonalizado || script.Card.Tarjeta.Tipo.Value == EigeTipoTarjetaEnum.ValenciaCardPerso || script.Card.Tarjeta.Tipo.Value == EigeTipoTarjetaEnum.ViajeroPersonalizado)
			//{
			//	codViajero = script.Card.Usuario.CodigoViajero.Value.ToString();
			//}
			return codViajero;
		}
		#endregion GetCodViajero

		#region GetTitleCodeAsync
		public async Task<string> GetTitleCodeAsync(long uid, EmtScript script, int index)
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
			if (nombre == "BONOBUS PLUS") return "96";
			
			throw new ScrapFormatException("EMT", uid, text);
		}
		#endregion GetTitleCode
	}
}
