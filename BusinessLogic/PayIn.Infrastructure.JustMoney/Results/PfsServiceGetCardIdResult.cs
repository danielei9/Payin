using System.Xml.Linq;
using System.Linq;

namespace PayIn.Infrastructure.JustMoney.Results
{
	public class PfsServiceGetCardIdResult
	{
		public int CardHolderId { get; set; }

		#region Constructors
		public PfsServiceGetCardIdResult(XElement element)
		{
			CardHolderId = element
				.Elements()
				.Where(x => x.Name == "Cardnumber")
				.Select(x => x.Value)
				.Cast<int?>()
				.FirstOrDefault() ?? 0;
		}
		#endregion Constructors
	}
}
