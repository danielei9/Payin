using PayIn.Infrastructure.JustMoney.Enums;
using System;
using System.Xml.Linq;

namespace PayIn.Infrastructure.JustMoney.Arguments
{
	public class PfsServiceRegisterTokenArguments
	{
		public string MerchantTokenCode { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string Address { get; set; }
		public string Address2 { get; set; }
		public string ZipCode { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public JustMoneyCountryEnum Country { get; set; }
		public string SuccessUrl { get; set; }
		public string NonSuccessUrl { get; set; }
		public string Currency { get; set; }
		public string Payload { get; set; }

		#region ToXmlString
		public string ToXmlString()
		{
			var data = new XElement("R");

			data.Add(new XElement("R1", MerchantTokenCode.Left(50)));
			data.Add(new XElement("R2", FirstName.Left(50)));
			if (!MiddleName.IsNullOrEmpty())
				data.Add(new XElement("R3", MiddleName.Left(50)));
			data.Add(new XElement("R4", LastName.Left(50)));
			data.Add(new XElement("R5", Address.Left(50)));
			data.Add(new XElement("R6", Address2.Left(50)));
			data.Add(new XElement("R7", City.Left(50)));
			if (!State.IsNullOrEmpty())
				data.Add(new XElement("R8", State.Left(50)));
			data.Add(new XElement("R9", Country.GetCountryCode()));
			data.Add(new XElement("R10", ZipCode.Left(50)));
			if (!SuccessUrl.IsNullOrEmpty())
				data.Add(new XElement("R11", SuccessUrl.Left(255)));
			if (!NonSuccessUrl.IsNullOrEmpty())
				data.Add(new XElement("R12", NonSuccessUrl.Left(255)));
			if (!Currency.IsNullOrEmpty())
				data.Add(new XElement("R13", Currency.LeftError(3)));
			if (!Payload.IsNullOrEmpty())
				data.Add(new XElement("UDF", Payload));

			return data.ToString();
		}
		#endregion ToXmlString
	}
}
