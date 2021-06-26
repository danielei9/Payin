using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace PayIn.Infrastructure.JustMoney.Results
{
	public class PfsServiceViewStatementResult
	{
		public IEnumerable<PfsServiceViewStatementResult_Transaction> Transactions { get; set; }

		#region Constructors
		public PfsServiceViewStatementResult(XElement element)
		{
			var transactionElements = element
				?.Element("cardholderstatementdetails")
				?.Element("cardpan")
				?.Elements("cardaccount")
				.SelectMany(x => x
					?.Element("transactionlist")
					?.Elements("transaction")
					?.ToList()
				)??
				new List<XElement>();

			Transactions = transactionElements
				.Where(b => b.Name == "transaction")
				.Select(b => new PfsServiceViewStatementResult_Transaction(b))
				.Where (b => b.ResponseCode == "000");
		}
		#endregion Constructors
	}
}
