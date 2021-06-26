using System;
using System.Xml.Linq;

namespace PayIn.Infrastructure.JustMoney.Arguments
{
	public class PfsServiceGetCardBalanceArguments
	{
		public string CardHolderId { get; set; }

		#region ToXmlString
		public string ToXmlString()
		{
			var data = new XElement("GetCardBalance");

			data.Add(new XElement("Cardholderid", CardHolderId.LeftError(9)));

			return data.ToString();
		}
		#endregion ToXmlString
	}
}
