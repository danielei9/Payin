using System.Linq;
using System.Xml.Linq;

namespace PayIn.Infrastructure.JustMoney.Results
{
	public class PfsServiceRegisterPayByTokenResult
	{
		public string ErrorCode { get; set; }
		public string Description { get; set; }
		public string RedirectUrl { get; set; }
		public string MessageId { get; set; }

		#region Constructors
		public PfsServiceRegisterPayByTokenResult(XElement element, string messageId)
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
			RedirectUrl = element
				.Elements()
				.Where(x => x.Name == "R3")
				.Select(x => x.Value)
				.FirstOrDefault() ?? "";
			MessageId = messageId;
		}
		#endregion Constructors
	}
}
