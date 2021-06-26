using System.Linq;
using System.Xml.Linq;

namespace PayIn.Infrastructure.JustMoney.Results
{
	public class PfsServiceDepositToCardResult
	{
		public string ErrorCode { get; set; }
		public string Description { get; set; }
		public string ReferenceId { get; set; }

		#region Constructors
		public PfsServiceDepositToCardResult(XElement element)
		{
			ErrorCode = element
				.Elements()
				.Where(x => x.Name == "ErrorCode")
				.Select(x => x.Value)
				.FirstOrDefault() ?? "";
			Description = element
				.Elements()
				.Where(x => x.Name == "Description")
				.Select(x => x.Value)
				.FirstOrDefault() ?? "";
			ReferenceId = element
				.Elements()
				.Where(x => x.Name == "ReferenceID")
				.Select(x => x.Value)
				.FirstOrDefault() ?? "";
		}
		#endregion Constructors
	}
}
