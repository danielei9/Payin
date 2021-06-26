using PayIn.Infrastructure.JustMoney.Enums;
using System;
using System.Xml.Linq;

namespace PayIn.Infrastructure.JustMoney.Arguments
{
	public class PfsServiceRegisterPayByTokenArguments
	{
		public string MerchantTokenCode { get; set; }
		public string FirstName { get; set; }
		public string MiddleInitial { get; set; }
		public string LastName { get; set; }
		public string Address { get; set; }
		public string Address2 { get; set; }
		public string ZipCode { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public JustMoneyCountryEnum Country { get; set; }
		public string VendorId { get; set; }
		public decimal Amount { get; set; }
		public string SuccessUrl { get; set; }
		public string NonSuccessUrl { get; set; }
		public string MerchantTransactionCode { get; set; }
		public string Currency { get; set; }
		public string Payload { get; set; }

		#region ToXmlString
		public string ToXmlString()
		{
			var data = new XElement("R");

			if (!MerchantTokenCode.IsNullOrEmpty())
				data.Add(new XElement("R1", MerchantTokenCode.LeftError(50)));
			data.Add(new XElement("R2", FirstName.LeftError(50)));
			data.Add(new XElement("R3", MiddleInitial.LeftError(50)));
			data.Add(new XElement("R4", LastName.LeftError(50)));
			data.Add(new XElement("R5", Address.LeftError(50)));
			data.Add(new XElement("R6", Address2.LeftError(50)));
			data.Add(new XElement("R7", City.LeftError(50)));
			data.Add(new XElement("R8", State.LeftError(50)));
			data.Add(new XElement("R9", Country.GetCountryCode()));
			data.Add(new XElement("R10", ZipCode.LeftError(50)));
			data.Add(new XElement("R11", VendorId.LeftError(50)));
			data.Add(new XElement("R12", (Amount * 100).ToString()));
			data.Add(new XElement("R13", SuccessUrl.LeftError(250)));
			data.Add(new XElement("R14", NonSuccessUrl.LeftError(250)));
			data.Add(new XElement("R15", MerchantTransactionCode.LeftError(40)));
			if (!Currency.IsNullOrEmpty())
				data.Add(new XElement("R16", Currency.LeftError(3)));
			if (!Payload.IsNullOrEmpty())
				data.Add(new XElement("UDF", Payload));

			return data.ToString();
		}
		#endregion ToXmlString
	}
}
