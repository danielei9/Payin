using System;
using System.Xml.Linq;

namespace PayIn.Infrastructure.JustMoney.Arguments
{
	public class PfsServiceGetCardIdArguments
	{
		public string CardNumber { get; set; }

		#region ToXmlString
		public string ToXmlString()
		{
			var data = new XElement("GetCardID");

			data.Add(new XElement("Cardnumber", CardNumber.LeftError(9)));

			return data.ToString();
		}
		#endregion ToXmlString
	}
}
