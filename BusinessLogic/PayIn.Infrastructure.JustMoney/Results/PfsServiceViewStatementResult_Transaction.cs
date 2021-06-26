using System;
using System.Linq;
using System.Xml.Linq;

namespace PayIn.Infrastructure.JustMoney.Results
{
	public class PfsServiceViewStatementResult_Transaction
	{
		public DateTime Date { get; set; } // MM/DD/YYYY HH:MM:SS *M FORMAT
										   //<Cardholderid>500000285771</Cardholderid>
										   //<TransactionType>230000</TransactionType>
										   //<MTI>27JR</MTI>
										   //<ARN>000000000000</ARN>
										   //<STN>0000000000</STN>
										   //<TermID />
										   //<AuthNum>090664</AuthNum>
										   //<RecType />
										   //<TransactionOrigin>P</TransactionOrigin>
		public string Description { get; set; }
		public decimal Amount { get; set; }
		//<fee>00000000000000</fee>
		public decimal AvailableBalance { get; set; }
		//<ledgerbalance>0000000010</ledgerbalance>
		//<issuerfee>00000000000000</issuerfee>
		//<clientid>SCS1</clientid>
		//<termnamelocation />
		//<termowner>200000285771</termowner>
		//<termcity>PFS</termcity>
		//<termstate>GB</termstate>
		//<termcountry>UK</termcountry>
		//<surcharge>00000000000000</surcharge>
		public string ResponseCode { get; set; }
		//<currency>GBP</rspcode>
		//<origtransamt>00000000000000</rspcode>
		//<termcurrency>GBP</rspcode>
		//<origholdamt>0000000000000</rspcode>

		#region Constructors
		public PfsServiceViewStatementResult_Transaction(XElement element)
		{
			Date = element
				.Elements()
				.Where(x => x.Name == "date")
				.Select(x => new DateTime(
					int.Parse(x.Value.Substring(6, 4)), int.Parse(x.Value.Substring(0, 2)), int.Parse(x.Value.Substring(3, 2)),
					int.Parse(x.Value.Substring(11, 2)) + (x.Value.Substring(20, 2).Equals("PM") ? 12 : 0), int.Parse(x.Value.Substring(14, 2)), int.Parse(x.Value.Substring(17, 2))
				))
				.FirstOrDefault();
			Description = element
				.Elements()
				.Where(x => x.Name == "description")
				.Select(x => x.Value)
				.FirstOrDefault() ?? "";
			Amount = element
				.Elements()
				.Where(x => x.Name == "amount")
				.Select(x => (decimal?)decimal.Parse(x.Value) / 100)
				.FirstOrDefault() ?? 0;
			AvailableBalance = element
				.Elements()
				.Where(x => x.Name == "availablebalance")
				.Select(x => (decimal?)decimal.Parse(x.Value) / 100)
				.FirstOrDefault() ?? 0;
			ResponseCode = element
				.Elements()
				.Where(x => x.Name == "rspcode")
				.Select(x => x.Value)
				.FirstOrDefault() ?? "";
		}
		#endregion Constructors
	}
}
