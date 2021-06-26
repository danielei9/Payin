using PayIn.Infrastructure.JustMoney.Enums;
using System;
using System.Xml.Linq;

namespace PayIn.Infrastructure.JustMoney.Arguments
{
	public class PfsServiceCardToCardArguments
	{
		public string CardHolderId { get; set; }
		public decimal Amount { get; set; }
		//public string CardNumberTo { get; set; }
		public string CardHolderIdTo { get; set; }
		public JustMoneyCurrencyCode CurrencyCode { get; set; }
		public string TerminalOwner { get; set; }
		public string TerminalLocation { get; set; }
		public string TerminalCity { get; set; }
		public string TerminalState { get; set; }
		public int? TerminalId { get; set; }
		public JustMoneyCountryEnum? Country { get; set; }
		public string Description { get; set; }
		public JustMoneyCurrencyCode SettlementCurrencyCode { get; set; }
		public string DirectFee { get; set; }
		//public string MobileNumberTo { get; set; }

		#region ToXmlString
		public string ToXmlString()
		{
			var data = new XElement("Cardtocard");
			
			data.Add(new XElement("Cardholderid", CardHolderId));
			data.Add(new XElement("Amount", (Amount * 100).ToString()));
			data.Add(new XElement("CurrencyCode", CurrencyCode.ToEnumAlias()));
			//data.Add(new XElement("CardNumberTo", CardNumberTo.ToString()));
			data.Add(new XElement("CardholderidTo", CardHolderIdTo));
			if (!TerminalOwner.IsNullOrEmpty())
				data.Add(new XElement("TerminalOwner", TerminalOwner.LeftError(30)));
			data.Add(new XElement("TerminalLocation", TerminalLocation
				.DeleteCharactersExcept("a-zA-Z0-9 ")
				.LeftError(30)
			));
			if (!TerminalCity.IsNullOrEmpty())
				data.Add(new XElement("TerminalCity", TerminalCity.LeftError(30)));
			if (!TerminalState.IsNullOrEmpty())
				data.Add(new XElement("TerminalState", TerminalState.LeftError(2)));
			if (TerminalId != null)
				data.Add(new XElement("TerminalID", TerminalId.ToString()));
			if (Country != null)
				data.Add(new XElement("Country", Country.Value.GetCountryCode()));
			data.Add(new XElement("Description", Description.LeftError(30)));
			data.Add(new XElement("SettlementCurrencyCode", SettlementCurrencyCode.ToEnumAlias()));
			if (!DirectFee.IsNullOrEmpty())
				data.Add(new XElement("DirectFee", DirectFee.LeftError(6)));
			//data.Add(new XElement("MobileNumberTo", MobileNumberTo.LeftError(15)));

			return data.ToString();
		}
		#endregion ToXmlString
	}
}
