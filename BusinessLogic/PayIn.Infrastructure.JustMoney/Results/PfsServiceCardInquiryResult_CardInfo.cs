using PayIn.Infrastructure.JustMoney.Enums;
using System.Linq;
using System.Xml.Linq;

namespace PayIn.Infrastructure.JustMoney.Results
{
	public class PfsServiceCardInquiryResult_CardInfo
	{
		//<AccountBaseCurrency>826</AccountBaseCurrency>
		//<CardType>20</CardType>
		//<AccountNumber>717949358724</AccountNumber>
		public CardStatus CardStatus { get; set; }
		//<PinTriesExceeded>0</PinTriesExceeded>
		//<BadPinTries>0</BadPinTries>
		//<ExpirationDate>2106</ExpirationDate>
		public int EspirationCodeYear { get; set; }
		public int EspirationCodeMonth { get; set; }
		//<Client />
		//<PhonecardNumber />
		public decimal AvailBal { get; set; }
		//<LedgerBal>000000012200</LedgerBal>
		//<DistributorCode>1873</DistributorCode>
		//<LoadAmount>000000000</LoadAmount>
		//<CompanyName />
		//<CardStyle>01</CardStyle>
		//<DeliveryType>VC</DeliveryType>
		//<SortCode>13-72-24</SortCode>
		//<SortCodeAccountNumber>00621674</SortCodeAccountNumber>
		public string Iban { get; set; }

		#region Constructors
		public PfsServiceCardInquiryResult_CardInfo(XElement element)
		{
			CardStatus = element
				.Elements()
				.Where(x => x.Name == "CardStatus")
				.Select(x => (CardStatus)int.Parse(x.Value))
				.FirstOrDefault();
			EspirationCodeYear = element
				.Elements()
				.Where(x => x.Name == "ExpirationDate")
				.Select(x => int.Parse(x.Value.Substring(0,2)))
				.FirstOrDefault();
			EspirationCodeMonth = element
				.Elements()
				.Where(x => x.Name == "ExpirationDate")
				.Select(x => int.Parse(x.Value.Substring(2, 2)))
				.FirstOrDefault();
			AvailBal = element
				.Elements()
				.Where(x => x.Name == "AvailBal")
				.Select(x => decimal.Parse(x.Value) / 100)
				.FirstOrDefault();
			Iban = element
				.Elements()
				.Where(x => x.Name == "Iban")
				.Select(x => x.Value)
				.FirstOrDefault() ?? "XXXX XXXX XX XXXXXXXXXX";
		}
		#endregion Constructors
	}
}
