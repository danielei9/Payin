using HtmlAgilityPack;
using PayIn.Domain.Transport.Eige.Enums;
using ScrapySharp.Network;
using ScrapySharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.Infrastructure.Transport.Services
{
	public class EmtScript
	{
		public Dictionary<string, string> Data = new Dictionary<string, string>();

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
		private static bool? GetBool(string text)
		{
			if (text.IsNullOrEmpty())
				return null;

			return text.Trim() == "1";
		}

		#region Add
		public void Add(Dictionary<string, string> data)
		{
			foreach (var item in data)
				Data[item.Key] = item.Value;
		}
		#endregion Add

		// EMT
		public int? UidEMT { get { return GetInt(Data.GetOrDefault("EMT_UID")); } }
		public decimal? SaldoViajeEMT { get { return GetInt(Data.GetOrDefault("EMT_SALDO")); } }
		public string SaldoUnidadesViajeEMT { get { return GetString(Data.GetOrDefault("EMT_SALDOUNIDADES")); } }
		public string TituloCodigo { get { return GetString(Data.GetOrDefault("EMT_TITULOCODIGO")); } }
		public string TituloNombreEMT { get { return GetString(Data.GetOrDefault("EMT_TITULONOMBRE")); } }
		public string UltimaValidacionEMT { get { return GetString(Data.GetOrDefault("EMT_ULTIMAVALIDACION")); } }
		public bool? TituloCaducado { get { return GetBool(Data.GetOrDefault("EMT_TITULOCADUCADO")); } }
	}
}
