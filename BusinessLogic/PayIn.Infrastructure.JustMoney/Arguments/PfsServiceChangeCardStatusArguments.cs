using PayIn.Infrastructure.JustMoney.Enums;
using System;
using System.Xml.Linq;

namespace PayIn.Infrastructure.JustMoney.Arguments
{
	public class PfsServiceChangeCardStatusArguments
	{
		public string CardHolderId { get; set; }
		public CardStatus OldStatus { get; set; }
		public CardStatus NewStatus { get; set; }

		#region ToXmlString
		public string ToXmlString()
		{
			var data = new XElement("ChangeCardStatus");

			data.Add(new XElement("Cardholderid", CardHolderId));
			data.Add(new XElement("OldStatus", ((int)OldStatus).ToString()));
			data.Add(new XElement("NewStatus", ((int)NewStatus).ToString()));

			return data.ToString();
		}
		#endregion ToXmlString
	}
}
