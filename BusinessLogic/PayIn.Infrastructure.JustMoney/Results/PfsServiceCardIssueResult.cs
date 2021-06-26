using System.Linq;
using System.Xml.Linq;

namespace PayIn.Infrastructure.JustMoney.Results
{
	public class PfsServiceCardIssueResult
	{
		public string CardHolderId { get; set; }
		public int AvailableBalance { get; set; }
		public int LedgerBalance { get; set; }

		#region Constructors
		public PfsServiceCardIssueResult(XElement element)
		{
			CardHolderId = element
				.Elements()
				.Where(x => x.Name == "Cardholderid")
				.Select(x => x.Value)
				.FirstOrDefault() ?? "";
			AvailableBalance = element
				.Elements()
				.Where(x => x.Name == "AvailableBalance")
				.Select(x => int.Parse(x.Value))
				.FirstOrDefault();
			LedgerBalance = element
				.Elements()
				.Where(x => x.Name == "LedgerBalance")
				.Select(x => int.Parse(x.Value))
				.FirstOrDefault();
		}
		#endregion Constructors
	}
}
