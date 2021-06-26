using System.Xml.Linq;
using System.Linq;

namespace PayIn.Infrastructure.JustMoney.Results
{
	public class PfsServiceGetCardBalanceResult
	{
		public string AvailableBalance { get; set; }
		public string LedgerBalance { get; set; }
		public string CurrencyCode { get; set; }
		public string Currency { get; set; }

		#region Constructors
		public PfsServiceGetCardBalanceResult(XElement element)
		{
			AvailableBalance = element
				.Elements()
				.Where(x => x.Name == "AvailableBalance")
				.Select(x => x.Value)
				.FirstOrDefault() ?? "";
			LedgerBalance = element
				.Elements()
				.Where(x => x.Name == "LedgerBalance")
				.Select(x => x.Value)
				.FirstOrDefault() ?? "";
			CurrencyCode = element
				.Elements()
				.Where(x => x.Name == "CurrencyCode")
				.Select(x => x.Value)
				.FirstOrDefault() ?? "";
			Currency = element
				.Elements()
				.Where(x => x.Name == "Currency")
				.Select(x => x.Value)
				.FirstOrDefault() ?? "";
		}
		#endregion Constructors
	}
}
