using PayIn.Common;
using System;

namespace PayIn.Application.Dto.Arguments
{
	public class MobileMainSynchronizeArguments_TicketLine
	{
		public string Title { get; set; }
		public decimal? Amount { get { return amount; } set { amount = value == null ? (decimal?)null : Math.Round(value.Value, 2); } }
		private decimal? amount;
		public decimal Quantity { get { return quantity; } set { quantity = Math.Round(value, 2); } }
		private decimal quantity;
		public int? EntranceTypeId { get; set; }
		public int? ProductId { get; set; }
		public TicketLineType Type { get; set; } = TicketLineType.Buy;
		public int? CampaignLineId { get; set; }
		public long? Uid{ get; set; }
	}
}
