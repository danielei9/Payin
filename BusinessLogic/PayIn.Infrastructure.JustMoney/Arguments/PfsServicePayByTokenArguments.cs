using System;
using System.Xml.Linq;

namespace PayIn.Infrastructure.JustMoney.Arguments
{
	public class PfsServicePayByTokenArguments
	{
		public string MerchantTokenCode { get; set; }
		public string VendorId { get; set; }
		public decimal? Amount { get; set; }
		public string SuccessfullUrl { get; set; }
		public string NonSuccessfullUrl { get; set; }
		public string MerchantTransactionCode { get; set; }
		public string Currency { get; set; }
		public string Payload { get; set; }

		#region ToXmlString
		public string ToXmlString()
		{
			var data = new XElement("R");
			
			data.Add(new XElement("R1", MerchantTokenCode.LeftError(50)));
			data.Add(new XElement("R2", VendorId.LeftError(50)));
			if (Amount != null)
				data.Add(new XElement("R3", (Amount * 100).ToString()));
			data.Add(new XElement("R4", SuccessfullUrl.LeftError(250)));
			if (!NonSuccessfullUrl.IsNullOrEmpty())
				data.Add(new XElement("R5", NonSuccessfullUrl.LeftError(250)));
			data.Add(new XElement("R6", MerchantTransactionCode.LeftError(40)));
			if (!Currency.IsNullOrEmpty())
				data.Add(new XElement("R7", Currency.LeftError(3)));
			if (!Payload.IsNullOrEmpty())
				data.Add(new XElement("UDF", Payload));

			return data.ToString();
		}
		#endregion ToXmlString
	}
}
