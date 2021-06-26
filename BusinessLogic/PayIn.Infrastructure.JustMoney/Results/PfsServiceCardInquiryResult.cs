using System.Linq;
using System.Xml.Linq;

namespace PayIn.Infrastructure.JustMoney.Results
{
	public class PfsServiceCardInquiryResult
	{
		public PfsServiceCardInquiryResult_CardInfo CardInfo { get; set; }
		public PfsServiceCardInquiryResult_CardHolder CardHolder { get; set; }

		#region Constructors
		public PfsServiceCardInquiryResult(XElement element)
		{
			CardInfo = new PfsServiceCardInquiryResult_CardInfo(element
				.Elements()
				.Where(x => x.Name == "cardinfo")
				.FirstOrDefault()
			);
			CardHolder = new PfsServiceCardInquiryResult_CardHolder(element
				.Elements()
				.Where(x => x.Name == "cardholder")
				.FirstOrDefault()
			);
		}
		#endregion Constructors
	}
}
