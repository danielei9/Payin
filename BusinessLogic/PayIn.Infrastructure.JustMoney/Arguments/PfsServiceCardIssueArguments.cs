using PayIn.Infrastructure.JustMoney.Enums;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace PayIn.Infrastructure.JustMoney.Arguments
{
	public class PfsServiceCardIssueArguments
	{
		public string Subbin { get; set; }
		public string UserId { get; set; }
		public CardType CardType { get; set; }
		public CardStyle CardStyle { get; set; }
		public string Pin { get; set; }
		public bool IsVirtualCard { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime BirthDay { get; set; }
		public string Address { get; set; }
		public string Address2 { get; set; }
		public string ZipCode { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public JustMoneyCountryEnum Country { get; set; }
		public string Phone { get; set; }
		public string Mobile { get; set; }
		public string Email { get; set; }

		#region ToXmlString
		public string ToXmlString()
		{
			var data = new XElement("CardIssue");

			data.Add(new XElement("FirstName", FirstName.LeftError(20)));
			data.Add(new XElement("LastName", LastName.LeftError(20)));
			data.Add(new XElement("DOB", BirthDay.ToString("dd/MM/yyyy")));
			data.Add(new XElement("Phone", Phone.LeftError(16)));
			if (!Mobile.IsNullOrEmpty())
				data.Add(new XElement("Phone2", Mobile.LeftError(16)));
			data.Add(new XElement("Email", Email.LeftError(255)));
			data.Add(new XElement("Address1", Address.LeftError(30)));
			if (Address.Length > 30)
				data.Add(new XElement("Address2", Address.Substring(30, 30)));
			if (Address.Length > 60)
				data.Add(new XElement("Address3", Address.Substring(60, 35)));
			if (Address.Length > 95)
				data.Add(new XElement("Address4", Address.Substring(95, 35)));
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
				data.Add(new XElement("State", State.LeftError(15)));
			data.Add(new XElement("CountryCode", Country.GetCountryCode()));
			data.Add(new XElement("CountryName", Country.GetCountryName()));
			data.Add(new XElement("Userdefinedfield1", UserId));
			data.Add(new XElement("Userdefinedfield2", (int)CardType));
			data.Add(new XElement("Userdefinedfield3", "SOLO"));
			if (new List<CardStyle> { CardStyle.TheaterProgramme, CardStyle.OrderOnline }.Contains(CardStyle))
				data.Add(new XElement("DistributorCode",
					CardStyle == CardStyle.TheaterProgramme ? "4452" :
					CardStyle == CardStyle.OrderOnline ? "4453" :
					""));
			data.Add(new XElement("CardStyle", ((int)CardStyle).ToString("00")));
			if (IsVirtualCard)
				data.Add(new XElement("DeliveryType", "VC"));
			if (!Pin.IsNullOrEmpty())
				data.Add(new XElement("Pin", Pin.LeftError(4)));
			data.Add(new XElement("bin", Subbin));
			data.Add(new XElement("OFACOverride", "N"));
			data.Add(new XElement("VerifySSNOverride", "N"));
			data.Add(new XElement("VerifyDOBOverride", "N"));
			data.Add(new XElement("GeoIPCheckOverride", "N"));

			return data.ToString();
		}
		#endregion ToXmlString
	}
}
