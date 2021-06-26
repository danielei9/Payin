using System;
using System.Xml.Linq;

namespace PayIn.Infrastructure.JustMoney.Arguments
{
	public class PfsServiceCardInquiryArguments
	{
		public string CardHolderid { get; set; }

		#region ToXmlString
		public string ToXmlString()
		{
			var data = new XElement("CardInquiry");

			data.Add(new XElement("Cardholderid", CardHolderid));

			return data.ToString();
		}
		#endregion ToXmlString
	}
}
