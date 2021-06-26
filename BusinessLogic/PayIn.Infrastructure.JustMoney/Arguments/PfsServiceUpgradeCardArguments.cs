using PayIn.Infrastructure.JustMoney.Enums;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace PayIn.Infrastructure.JustMoney.Arguments
{
	public class PfsServiceUpgradeCardArguments
	{
		public string CardHolderId { get; set; }
		public CardType CardType { get; set; }

		#region ToXmlString
		public string ToXmlString()
		{
			var data = new XElement("UpgradeCard");

			data.Add(new XElement("CardholderID", CardHolderId));
			data.Add(new XElement("ProductType", (int)CardType));

			return data.ToString();
		}
		#endregion ToXmlString
	}
}
