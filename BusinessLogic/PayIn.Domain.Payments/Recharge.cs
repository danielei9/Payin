using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Domain;

namespace PayIn.Domain.Payments
{
	public class Recharge : Entity
	{
		public decimal Amount { get; set; }
		public DateTime Date { get; set; }

		[Required(AllowEmptyStrings = true)]
		public string UserName { get; set; }

		[Required(AllowEmptyStrings = false)]
		public string UserLogin { get; set; }

		[Required(AllowEmptyStrings = false)]
		public string TaxName { get; set; }

		[Required(AllowEmptyStrings = true)]
		public string TaxAddress { get; set; }

		[Required(AllowEmptyStrings = true)]
		public string TaxNumber { get; set; }


		#region Liquidation
		public int? LiquidationId { get; set; }
		public Liquidation Liquidation { get; set; }
		#endregion Liquidation

		#region Ticket
		public int TicketId { get; set; }
		public Ticket Ticket { get; set; }
		#endregion Ticket

		#region PaymentMedia
		public int PaymentMediaId { get; set; }
		public PaymentMedia PaymentMedia { get; set; }
		#endregion PaymentMedia

		#region CampaignLine
		public int CampaignLineId { get; set; }
		public CampaignLine CampaignLine { get; set; }
		#endregion CampaignLine

		#region Payment		
		public int? PaymentId { get; set; }
		public Payment Payment { get; set; }
		#endregion Payment
	}
}
