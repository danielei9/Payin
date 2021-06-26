using PayIn.Infrastructure.JustMoney.Enums;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace PayIn.Infrastructure.JustMoney.Arguments
{
	public class PfsServiceUpdateCardArguments
	{
		public string CardHolderId { get; set; }
		//public string Subbin { get; set; }
		public string UserId { get; set; }
		public CardStyle CardStyle { get; set; }
		public CardType CardType { get; set; }
		public bool IsVirtualCard { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime BirthDay { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string ZipCode { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public JustMoneyCountryEnum Country { get; set; }
		public string Phone { get; set; }
		public string Mobile { get; set; }
		public string Email { get; set; }

		#region GetCountryCode
		public string GetCountryCode(string countryCode)
		{
			// https://www.b2bpay.co/sepa-countries

			// Euro countries
			if (countryCode == "Aland Islands") return "FI";
			if (countryCode == "Austria") return "AT";
			if (countryCode == "Azores") return "PT";
			if (countryCode == "Belgium") return "BE";
			if (countryCode == "Canary Island") return "ES";
			if (countryCode == "Cyprus") return "CY";
			if (countryCode == "Estonia") return "EE";
			if (countryCode == "Finland") return "FI";
			if (countryCode == "France") return "FR";
			if (countryCode == "Germany") return "DE";
			if (countryCode == "Greece") return "GR";
			if (countryCode == "Ireland") return "IE";
			if (countryCode == "Italy") return "IT";
			if (countryCode == "Latvia") return "LV";
			if (countryCode == "Lithuania") return "LT";
			if (countryCode == "Luxembourg") return "LU";
			if (countryCode == "Madeira") return "PT";
			if (countryCode == "Malta") return "MT";
			if (countryCode == "Monaco") return "MC";
			if (countryCode == "Netherlands") return "NL";
			if (countryCode == "Portugal") return "PT";
			if (countryCode == "San Marino") return "SM";
			if (countryCode == "Slovakia") return "SK";
			if (countryCode == "Slovenia") return "SI";
			if (countryCode == "Spain") return "ES";

			// European countries (same BIC / IBAN)
			if (countryCode == "Bulgaria") return "BG";
			if (countryCode == "Czech Republic") return "CZ";
			if (countryCode == "Denmark") return "DK";
			if (countryCode == "Gibraltar") return "GI";
			if (countryCode == "Hungary") return "HU";
			if (countryCode == "Iceland") return "IS";
			if (countryCode == "Liechtenstein") return "LI";
			if (countryCode == "Norway") return "NO";
			if (countryCode == "Poland") return "PL";
			if (countryCode == "Romania") return "RO";
			if (countryCode == "Sweden") return "SE";
			if (countryCode == "Switzerland") return "CH";
			if (countryCode == "UK") return "GB";

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

		#region ToXmlString
		public string ToXmlString()
		{
			var data = new XElement("UpdateCard");

			data.Add(new XElement("Cardholderid", CardHolderId));
			data.Add(new XElement("FirstName", FirstName.LeftError(20)));
			data.Add(new XElement("LastName", LastName.LeftError(20)));
			data.Add(new XElement("DOB", BirthDay.ToString("dd/MM/yyyy")));
			data.Add(new XElement("Phone", Phone.LeftError(16)));
			if (!Mobile.IsNullOrEmpty())
				data.Add(new XElement("Phone2", Mobile.LeftError(15))); // antes 16
			data.Add(new XElement("Email", Email.LeftError(40))); // antes 255
			data.Add(new XElement("Address1", Address1.LeftError(30)));
			if (Address1.Length > 30)
				data.Add(new XElement("Address2", Address1.Substring(30, 30)));
			if (Address1.Length > 60)
				data.Add(new XElement("Address3", Address1.Substring(60, 30))); // antes 35
			if (Address1.Length > 95)
				data.Add(new XElement("Address4", Address1.Substring(95, 30))); // antes 35
			if (!Address2.IsNullOrEmpty())
			{
				data.Add(new XElement("Addresslineforsecondaryaddress", Address2.LeftError(35)));
				if (Address2.Length > 35)
					data.Add(new XElement("Addressline2forsecondaryaddress", Address2.Substring(35, 35)));
				if (Address2.Length > 70)
					data.Add(new XElement("Addressline3forsecondaryaddress", Address2.Substring(70, 35)));
				if (Address2.Length > 105)
					data.Add(new XElement("Addressline4forsecondaryaddress", Address2.Substring(105, 35)));
			}
			if (!ZipCode.IsNullOrEmpty())
			{
				data.Add(new XElement("ZipCode", ZipCode.LeftError(15)));
				if (ZipCode.Length > 15)
					data.Add(new XElement("ZipCode2", ZipCode.Substring(15, 15)));
			}
			data.Add(new XElement("City", City.LeftError(25)));
			if (City.Length > 25)
				data.Add(new XElement("City2", City.Substring(25, 25)));
			if (!State.IsNullOrEmpty())
				data.Add(new XElement("State", State.LeftError(30))); // antes 15
			data.Add(new XElement("CountryCode", Country.GetCountryCode()));
			data.Add(new XElement("CountryName", Country.GetCountryName().LeftError(30))); // antes XX
			data.Add(new XElement("Userdefinedfield1", UserId.LeftError(16))); // antes XX
			data.Add(new XElement("Userdefinedfield2", (int)CardType));
			data.Add(new XElement("Userdefinedfield3", "SOLO"));
			if (new List<CardStyle> { CardStyle.TheaterProgramme, CardStyle.OrderOnline }.Contains(CardStyle))
				data.Add(new XElement("DistributorCode",
					CardStyle == CardStyle.TheaterProgramme ? "4452" :
					CardStyle == CardStyle.OrderOnline ? "4453" :
					"")); // distributorCode.Left(8)));
			data.Add(new XElement("CardStyle", ((int)CardStyle).ToString("00")));
			if (IsVirtualCard)
				data.Add(new XElement("DeliveryType", "VC"));
			//data.Add(new XElement("bin", Subbin));
			data.Add(new XElement("Ofacoverride", "N"));
			data.Add(new XElement("Verifyssnoverride", "N"));
			data.Add(new XElement("Verifydoboverride", "N"));
			//data.Add(new XElement("GeoIPCheckOverride", "N"));

			return data.ToString();
		}
		#endregion ToXmlString
	}
}
