using PayIn.Infrastructure.JustMoney.Enums;
using System;
using System.Xml.Linq;

namespace PayIn.Infrastructure.JustMoney.Arguments
{
	public class PfsServiceDepositToCardArguments
	{
		public string CardHolderId { get; set; }
		public decimal Amount { get; set; }
		public JustMoneyTransactionType TransactionType { get; set; }
		public string CurrencyCode { get; set; }
		public decimal? SettlementAmount { get; set; }
		public string SettlementCurrencyCode { get; set; }
		public string TransactionDescription { get; set; }
		public string TerminalOwner { get; set; }
		public string TerminalLocation { get; set; }
		public string TerminalCity { get; set; }
		public string TerminalState { get; set; }
		public int? TerminalId { get; set; }
		public JustMoneyCountryEnum? Country { get; set; }
		public int? TransactionFlatFee { get; set; }
		public string FeeDescription { get; set; }
		public string DirectFee { get; set; }
		public string VoucherCode { get; set; }

		#region ToXmlString
		public string ToXmlString()
		{
			var data = new XElement("DepositToCard");
			
			data.Add(new XElement("Cardholderid", CardHolderId));
			data.Add(new XElement("Amount", Convert.ToInt32(Amount * 100).ToString()));
			data.Add(new XElement("TransactionType", (int)TransactionType));
			//data.Add(new XElement("CurrencyCode", CurrencyCode.ToEnumAlias()));
			data.Add(new XElement("CurrencyCode", CurrencyCode));
			if (SettlementAmount != null)
				data.Add(new XElement("SettlementAmount", (SettlementAmount * 100).ToString()));
			//data.Add(new XElement("SettlementCurrencyCode", SettlementCurrencyCode.ToEnumAlias()));
			data.Add(new XElement("SettlementCurrencyCode", SettlementCurrencyCode));
			data.Add(new XElement("TransactionDescription", TransactionDescription.LeftError(30)));
			if (!TerminalOwner.IsNullOrEmpty())
				data.Add(new XElement("TerminalOwner", TerminalOwner.LeftError(22)));
			data.Add(new XElement("TerminalLocation", TerminalLocation.LeftError(30)));
			if (!TerminalCity.IsNullOrEmpty())
				data.Add(new XElement("TerminalCity", TerminalCity.LeftError(20)));
			if (!TerminalState.IsNullOrEmpty())
				data.Add(new XElement("TerminalState", TerminalState.LeftError(2)));
			if (TerminalId != null)
				data.Add(new XElement("TerminalID", TerminalId.ToString()));
			if (Country != null)
				data.Add(new XElement("Country", Country.Value.GetCountryCode()));
			if (TransactionFlatFee != null)
				data.Add(new XElement("TransactionFlatFee", TransactionFlatFee.ToString()));
			if (!FeeDescription.IsNullOrEmpty())
				data.Add(new XElement("FeeDescription", FeeDescription.LeftError(30)));
			if (!DirectFee.IsNullOrEmpty())
				data.Add(new XElement("DirectFee", DirectFee));
			if (!VoucherCode.IsNullOrEmpty())
				data.Add(new XElement("VoucherCode", VoucherCode));

			return data.ToString();
		}
		#endregion ToXmlString
	}
}
