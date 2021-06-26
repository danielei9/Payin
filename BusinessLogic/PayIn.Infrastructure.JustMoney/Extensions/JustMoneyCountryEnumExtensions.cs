using System;

namespace PayIn.Infrastructure.JustMoney.Enums
{
	public static class JustMoneyCountryEnumExtensions
	{
		#region GetCountryName
		public static string GetCountryName(this JustMoneyCountryEnum country)
		{
			// https://www.b2bpay.co/sepa-countries

			// Euro countries
			if (country == JustMoneyCountryEnum.AlandIslands) return "Aland Islands";
			if (country == JustMoneyCountryEnum.Austria) return "Austria";
			if (country == JustMoneyCountryEnum.Azores) return "Azores";
			if (country == JustMoneyCountryEnum.Belgium) return "Belgium";
			//if (country == JustMoneyCountryEnum.CanaryIsland) return "Canary Island";
			if (country == JustMoneyCountryEnum.Cyprus) return "Cyprus";
			if (country == JustMoneyCountryEnum.Estonia) return "Estonia";
			if (country == JustMoneyCountryEnum.Finland) return "Finland";
			if (country == JustMoneyCountryEnum.France) return "France";
			if (country == JustMoneyCountryEnum.Germany) return "Germany";
			if (country == JustMoneyCountryEnum.Greece) return "Greece";
			if (country == JustMoneyCountryEnum.Ireland) return "Ireland";
			if (country == JustMoneyCountryEnum.Italy) return "Italy";
			if (country == JustMoneyCountryEnum.Latvia) return "Latvia";
			if (country == JustMoneyCountryEnum.Lithuania) return "Lithuania";
			if (country == JustMoneyCountryEnum.Luxembourg) return "Luxembourg";
			if (country == JustMoneyCountryEnum.Madeira) return "Madeira";
			if (country == JustMoneyCountryEnum.Malta) return "Malta";
			if (country == JustMoneyCountryEnum.Monaco) return "Monaco";
			if (country == JustMoneyCountryEnum.Netherlands) return "Netherlands";
			if (country == JustMoneyCountryEnum.Portugal) return "Portugal";
			if (country == JustMoneyCountryEnum.SanMarino) return "San Marino";
			if (country == JustMoneyCountryEnum.Slovakia) return "Slovakia";
			if (country == JustMoneyCountryEnum.Slovenia) return "Slovenia";
			if (country == JustMoneyCountryEnum.Spain) return "Spain";

			// European countries (same BIC / IBAN)
			if (country == JustMoneyCountryEnum.Bulgaria) return "BG";
			if (country == JustMoneyCountryEnum.CzechRepublic) return "CZ";
			if (country == JustMoneyCountryEnum.Denmark) return "DK";
			if (country == JustMoneyCountryEnum.Gibraltar) return "GI";
			if (country == JustMoneyCountryEnum.Hungary) return "HU";
			if (country == JustMoneyCountryEnum.Iceland) return "IS";
			if (country == JustMoneyCountryEnum.Liechtenstein) return "LI";
			if (country == JustMoneyCountryEnum.Norway) return "NO";
			if (country == JustMoneyCountryEnum.Poland) return "PL";
			if (country == JustMoneyCountryEnum.Romania) return "RO";
			if (country == JustMoneyCountryEnum.Sweden) return "SE";
			if (country == JustMoneyCountryEnum.Switzerland) return "CH";
			if (country == JustMoneyCountryEnum.UK) return "GB";

			// European countries (diff BIC / IBAN)
			//Territory	BIC code	IBAN code
			//French Guiana	GF	FR
			//Guadeloupe	GP	FR
			//Martinique	MQ	FR
			//Mayotte	YT	FR
			//Réunion	RE	FR
			//Saint Barthélemy	BL	FR
			//Saint Martin (French part)	MF	FR
			//Saint Pierre and Miquelon	PM	FR

			throw new Exception("Country not found");
		}
		#endregion GetCountryName

		#region GetCountryCode
		public static string GetCountryCode(this JustMoneyCountryEnum country)
		{
			// https://www.b2bpay.co/sepa-countries

			// Euro countries
			if (country == JustMoneyCountryEnum.AlandIslands) return "FI";
			if (country == JustMoneyCountryEnum.Austria) return "AT";
			if (country == JustMoneyCountryEnum.Azores) return "PT";
			if (country == JustMoneyCountryEnum.Belgium) return "BE";
			//if (country == JustMoneyCountryEnum.CanaryIsland) return "ES";
			if (country == JustMoneyCountryEnum.Cyprus) return "CY";
			if (country == JustMoneyCountryEnum.Estonia) return "EE";
			if (country == JustMoneyCountryEnum.Finland) return "FI";
			if (country == JustMoneyCountryEnum.France) return "FR";
			if (country == JustMoneyCountryEnum.Germany) return "DE";
			if (country == JustMoneyCountryEnum.Greece) return "GR";
			if (country == JustMoneyCountryEnum.Ireland) return "IE";
			if (country == JustMoneyCountryEnum.Italy) return "IT";
			if (country == JustMoneyCountryEnum.Latvia) return "LV";
			if (country == JustMoneyCountryEnum.Lithuania) return "LT";
			if (country == JustMoneyCountryEnum.Luxembourg) return "LU";
			if (country == JustMoneyCountryEnum.Madeira) return "PT";
			if (country == JustMoneyCountryEnum.Malta) return "MT";
			if (country == JustMoneyCountryEnum.Monaco) return "MC";
			if (country == JustMoneyCountryEnum.Netherlands) return "NL";
			if (country == JustMoneyCountryEnum.Portugal) return "PT";
			if (country == JustMoneyCountryEnum.SanMarino) return "SM";
			if (country == JustMoneyCountryEnum.Slovakia) return "SK";
			if (country == JustMoneyCountryEnum.Slovenia) return "SI";
			if (country == JustMoneyCountryEnum.Spain) return "ES";

			// European countries (same BIC / IBAN)
			if (country == JustMoneyCountryEnum.Bulgaria) return "BG";
			if (country == JustMoneyCountryEnum.CzechRepublic) return "CZ";
			if (country == JustMoneyCountryEnum.Denmark) return "DK";
			if (country == JustMoneyCountryEnum.Gibraltar) return "GI";
			if (country == JustMoneyCountryEnum.Hungary) return "HU";
			if (country == JustMoneyCountryEnum.Iceland) return "IS";
			if (country == JustMoneyCountryEnum.Liechtenstein) return "LI";
			if (country == JustMoneyCountryEnum.Norway) return "NO";
			if (country == JustMoneyCountryEnum.Poland) return "PL";
			if (country == JustMoneyCountryEnum.Romania) return "RO";
			if (country == JustMoneyCountryEnum.Sweden) return "SE";
			if (country == JustMoneyCountryEnum.Switzerland) return "CH";
			if (country == JustMoneyCountryEnum.UK) return "GB";

			// European countries (diff BIC / IBAN)
			//Territory	BIC code	IBAN code
			//French Guiana	GF	FR
			//Guadeloupe	GP	FR
			//Martinique	MQ	FR
			//Mayotte	YT	FR
			//Réunion	RE	FR
			//Saint Barthélemy	BL	FR
			//Saint Martin (French part)	MF	FR
			//Saint Pierre and Miquelon	PM	FR

			throw new Exception("Country not found");
		}
		#endregion GetCountryCode

		#region GetCountryEnum
		public static JustMoneyCountryEnum GetCountryEnum(this string country)
		{
			// https://www.b2bpay.co/sepa-countries

			// Euro countries
			if (country == "FI") return JustMoneyCountryEnum.AlandIslands;
			if (country == "AT") return JustMoneyCountryEnum.Austria;
			if (country == "PT") return JustMoneyCountryEnum.Azores;
			if (country == "BE") return JustMoneyCountryEnum.Belgium;
			//if (country == "ES") return JustMoneyCountryEnum.CanaryIsland;
			if (country == "CY") return JustMoneyCountryEnum.Cyprus;
			if (country == "EE") return JustMoneyCountryEnum.Estonia;
			if (country == "FI") return JustMoneyCountryEnum.Finland;
			if (country == "FR") return JustMoneyCountryEnum.France;
			if (country == "DE") return JustMoneyCountryEnum.Germany;
			if (country == "GR") return JustMoneyCountryEnum.Greece;
			if (country == "IE") return JustMoneyCountryEnum.Ireland;
			if (country == "IT") return JustMoneyCountryEnum.Italy;
			if (country == "LV") return JustMoneyCountryEnum.Latvia;
			if (country == "LT") return JustMoneyCountryEnum.Lithuania;
			if (country == "LU") return JustMoneyCountryEnum.Luxembourg;
			if (country == "PT") return JustMoneyCountryEnum.Madeira;
			if (country == "MT") return JustMoneyCountryEnum.Malta;
			if (country == "MC") return JustMoneyCountryEnum.Monaco;
			if (country == "NL") return JustMoneyCountryEnum.Netherlands;
			if (country == "PT") return JustMoneyCountryEnum.Portugal;
			if (country == "SM") return JustMoneyCountryEnum.SanMarino;
			if (country == "SK") return JustMoneyCountryEnum.Slovakia;
			if (country == "SI") return JustMoneyCountryEnum.Slovenia;
			if (country == "ES") return JustMoneyCountryEnum.Spain;

			// European countries (same BIC / IBAN)
			if (country == "BG") return JustMoneyCountryEnum.Bulgaria;
			if (country == "CZ") return JustMoneyCountryEnum.CzechRepublic;
			if (country == "DK") return JustMoneyCountryEnum.Denmark;
			if (country == "GI") return JustMoneyCountryEnum.Gibraltar;
			if (country == "HU") return JustMoneyCountryEnum.Hungary;
			if (country == "IS") return JustMoneyCountryEnum.Iceland;
			if (country == "LI") return JustMoneyCountryEnum.Liechtenstein;
			if (country == "NO") return JustMoneyCountryEnum.Norway;
			if (country == "PL") return JustMoneyCountryEnum.Poland;
			if (country == "RO") return JustMoneyCountryEnum.Romania;
			if (country == "SE") return JustMoneyCountryEnum.Sweden;
			if (country == "CH") return JustMoneyCountryEnum.Switzerland;
			if (country == "GB") return JustMoneyCountryEnum.UK;

			// European countries (diff BIC / IBAN)
			//Territory	BIC code	IBAN code
			//French Guiana	GF	FR
			//Guadeloupe	GP	FR
			//Martinique	MQ	FR
			//Mayotte	YT	FR
			//Réunion	RE	FR
			//Saint Barthélemy	BL	FR
			//Saint Martin (French part)	MF	FR
			//Saint Pierre and Miquelon	PM	FR

			throw new Exception("Country not found");
		}
		#endregion GetCountryEnum
	}
}
