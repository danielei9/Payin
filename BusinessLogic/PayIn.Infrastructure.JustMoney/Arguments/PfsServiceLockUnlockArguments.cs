using PayIn.Infrastructure.JustMoney.Enums;
using System;
using System.Xml.Linq;

namespace PayIn.Infrastructure.JustMoney.Arguments
{
	public class PfsServiceLockUnlockArguments
	{
		public string CardHolderId { get; set; }
		public CardStatus OldStatus { get; set; }
		public CardStatus NewStatus { get; set; }

		#region ToXmlString
		public string ToXmlString()
		{
			var data = new XElement("LockUnlock");

			data.Add(new XElement("Cardholderid", CardHolderId.LeftError(9)));
			data.Add(new XElement("OldStatus", OldStatus.ToString()));
			data.Add(new XElement("NewStatus", NewStatus.ToString()));

			return data.ToString();
		}
		#endregion ToXmlString
	}
}
