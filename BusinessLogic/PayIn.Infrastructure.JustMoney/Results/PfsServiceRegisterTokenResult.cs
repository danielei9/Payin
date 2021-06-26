using System.Linq;
using System.Xml.Linq;

namespace PayIn.Infrastructure.JustMoney.Results
{
	public class PfsServiceRegisterTokenResult
	{
		public string ErrorCode { get; set; }
		public string Description { get; set; }

		#region Constructors
		public PfsServiceRegisterTokenResult(XElement element)
		{
			ErrorCode = element
				.Elements()
				.Where(x => x.Name == "R1")
				.Select(x => x.Value)
				.FirstOrDefault() ?? "";
			Description = element
				.Elements()
				.Where(x => x.Name == "R2")
				.Select(x => x.Value)
				.FirstOrDefault() ?? "";
		}
		#endregion Constructors
	}
}
