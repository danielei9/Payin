using System.Xml.Linq;

namespace PayIn.Infrastructure.JustMoney.Arguments
{
	public class PfsServiceViewStatementArguments
	{
		public string CardHolderid { get; set; }
		public int StartDateYear { get; set; }
		public int StartDateMonth { get; set; }
		public int StartDateDay { get; set; }
		public int EndDateYear { get; set; }
		public int EndDateMonth { get; set; }
		public int EndDateDay { get; set; }

		#region ToXmlString
		public string ToXmlString()
		{
			var data = new XElement("ViewStatement");

			data.Add(new XElement("Cardholderid", CardHolderid));
			data.Add(new XElement("StartDate", StartDateYear.ToString("0000") + "-" + StartDateMonth.ToString("00") + "-" + StartDateDay.ToString("00")));
			data.Add(new XElement("EndDate", EndDateYear.ToString("0000") + "-" + EndDateMonth.ToString("00") + "-" + EndDateDay.ToString("00")));
			data.Add(new XElement("ViewStyle", "Y"));

			return data.ToString();
		}
		#endregion ToXmlString
	}
}
