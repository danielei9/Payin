using HtmlAgilityPack;
using PayIn.Domain.Transport.Eige.Enums;
using ScrapySharp.Network;
using ScrapySharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace PayIn.Infrastructure.Transport.Services
{
	public class FgvScript
	{
		public Dictionary<string, string> Data = new Dictionary<string, string>();

		private static decimal? GetDecimal(string text)
		{
			if (text.IsNullOrEmpty())
				return null;

			return Convert.ToDecimal(text, new NumberFormatInfo() { NumberDecimalSeparator = "," });
		}
		private static int? GetInt(string text)
		{
			if (text.IsNullOrEmpty())
				return null;

			return Convert.ToInt32(text);
		}
		private static string GetString(string text)
		{
			if (text.IsNullOrEmpty())
				return "";

			return text.Trim();
		}
		private static DateTime? GetDate(string text)
		{
			if (text.IsNullOrEmpty())
				return null;

			// DD/MM/AAAA
			var day = Convert.ToInt32(text.Substring(0, 2));
			var month = Convert.ToInt32(text.Substring(3, 2));
			var year = Convert.ToInt32(text.Substring(6, 4));

			return new DateTime(year, month, day).ToUTC();
		}

		#region Add
		public void Add(Dictionary<string, string> data)
		{
			foreach (var item in data)
				Data[item.Key] = item.Value;
		}
		#endregion Add

		// FGV
		public decimal? SaldoViaje { get { return GetDecimal(Data.GetOrDefault("FGV_SALDO")); } }
		public decimal? SaldoAcumuladoViaje { get { return GetDecimal(Data.GetOrDefault("FGV_SALDOACUMULADO")); } }
		public string SaldoUnidadesViaje { get { return GetString(Data.GetOrDefault("FGV_SALDOUNIDADES")); } }
		public int? TituloCodigo { get { return GetInt(Data.GetOrDefault("FGV_TITULOCODIGO")); } }
		public string TituloNombre { get { return GetString(Data.GetOrDefault("FGV_TITULONOMBRE")); } }
		public string TituloZona { get { return GetString(Data.GetOrDefault("FGV_TITULOZONA")); } }
		public DateTime? FechaCaducidad { get { return GetDate(Data.GetOrDefault("FGV_TITULOCADUCIDAD")); } }
	}
}
