using System.Linq;
using System.Xml.Linq;

namespace PayIn.Infrastructure.JustMoney.Results
{
	public class PfsServiceCardToCardResult
	{
		public string ErrorCode { get; set; }
		public string Description { get; set; }

		#region Constructors
		public PfsServiceCardToCardResult(XElement element)
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
		}
		#endregion Constructors
	}
}
