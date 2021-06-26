using PayIn.Domain.Transport.Eige.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.Infrastructure.Transport.Services
{
	public class SigapuntScript
	{
		public Dictionary<string, string> Data = new Dictionary<string, string>();

		private static bool? GetBool(string text)
		{
			if (text.IsNullOrEmpty())
				return null;

			return text == "1";
		}
		private static int? GetInt(string text)
		{
			if (text.IsNullOrEmpty())
				return null;

			return Convert.ToInt32(text);
		}
		private static decimal? GetCurrency(string text)
		{
			if (text.IsNullOrEmpty())
				return null;

			return Convert.ToInt32(text) / 100;
		}
		private static int? GetInt32FromBinary(string text)
		{
			if (text.IsNullOrEmpty())
				return null;

			return Convert.ToInt32(text, 2);
		}
		private static DateTime? GetDate(string text)
		{
			if (text.IsNullOrEmpty())
				return null;
			
			// AAAAMMDD
			var year = Convert.ToInt32(text.Substring(0, 4));
			var month = Convert.ToInt32(text.Substring(4, 2));
			var day = Convert.ToInt32(text.Substring(6, 2));

			return new DateTime(year, month, day);
		}
		private static string GetString(string text)
		{
			if (text.IsNullOrEmpty())
				return "";

			return text.Trim();
		}

		#region Add
		public void Add(Dictionary<string, string> data)
		{
			foreach (var item in data)
				Data[item.Key] = item.Value;
		}
		#endregion Add

		// TAXEMIN
		public int? CodigoViajero { get { return GetInt(Data.GetOrDefault("CODVIA")); } }
		public EigeTipoTarjetaEnum? Tipo { get { return (EigeTipoTarjetaEnum?)GetInt32FromBinary(Data.GetOrDefault("TIPO")); } }
		public EigeSubtipoTarjetaEnum? Subtipo { get { return (EigeSubtipoTarjetaEnum?)GetInt32FromBinary(Data.GetOrDefault("SUBTI")); } }
		public int? VersionTarjeta { get { return GetInt32FromBinary(Data.GetOrDefault("VER")); } }
		public DateTime? FechaCaducidad { get { return GetDate(Data.GetOrDefault("FECAD")); } }
		public bool? TituloActivo1 { get { return GetBool(Data.GetOrDefault("TIT1ACT")); } }
		public bool? TituloActivo2 { get { return GetBool(Data.GetOrDefault("TIT2ACT")); } }
		public bool? TituloActivoM { get { return GetBool(Data.GetOrDefault("MONACT")); } }
		public bool? TituloActivoB { get { return GetBool(Data.GetOrDefault("BONACT")); } }
		public int? CodigoTitulo1 { get { return GetInt(Data.GetOrDefault("CODTIT1")); } }
		public int? CodigoTitulo2 { get { return GetInt(Data.GetOrDefault("CODTIT2")); } }
		public decimal? SaldoMonedero { get { return GetCurrency(Data.GetOrDefault("SALMON")); } }
		public decimal? SaldoBonus { get { return GetCurrency(Data.GetOrDefault("SALBON")); } }

		// CONUSU
		public string Nombre { get { return GetString(Data.GetOrDefault("NOMBRE")); } }
		public string Apellidos { get { return GetString(Data.GetOrDefault("APE1") + " " + Data.GetOrDefault("APE2")); } }
		public string Dni { get { return GetString(Data.GetOrDefault("DNI")); } }

		// REC
		public int? EmpresaPropietaria { get { return GetInt(Data.GetOrDefault("EMPPRO")); } }
		public int? SaldoViaje1 { get { return GetInt(Data.GetOrDefault("SALVI1")); } }
		public DateTime? FechaValidez1 { get { return GetDate(Data.GetOrDefault("FECVA1")); } }
		public int? ValidezZonal1 { get { return GetInt32FromBinary(Data.GetOrDefault("VALZO1")); } }
		public bool? TituloEnAmpliacion1 { get { return GetBool(Data.GetOrDefault("ESTAM1")); } }
		public int? NumeroUnidadesValidezTemporal1 { get { return GetInt(Data.GetOrDefault("UNITE1")); } }
		public EigeTituloTipoTarifaEnum? TipoTarifa1 { get { return (EigeTituloTipoTarifaEnum?)GetInt32FromBinary(Data.GetOrDefault("TIPTA1")); } }
		public int? SaldoViaje2 { get { return GetInt(Data.GetOrDefault("SALVI2")); } }
		public DateTime? FechaValidez2 { get { return GetDate(Data.GetOrDefault("FECVA2")); } }
		public int? ValidezZonal2 { get { return GetInt32FromBinary(Data.GetOrDefault("VALZO2")); } }
		public bool? TituloEnAmpliacion2 { get { return GetBool(Data.GetOrDefault("ESTAM2")); } }
		public int? NumeroUnidadesValidezTemporal2 { get { return GetInt(Data.GetOrDefault("UNITE2")); } }
		public EigeTituloTipoTarifaEnum? TipoTarifa2 { get { return (EigeTituloTipoTarifaEnum?)GetInt32FromBinary(Data.GetOrDefault("TIPTA2")); } }
	}
}
