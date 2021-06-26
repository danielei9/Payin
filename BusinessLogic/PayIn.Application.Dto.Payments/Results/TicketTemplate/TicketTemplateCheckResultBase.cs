using Xp.Application.Dto;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results.TicketTemplate
{
	public class TicketTemplateCheckResultBase : ResultBase<TicketTemplateCheckResult>
	{
		public bool Success { get; set; }
		public decimal? Amount { get; set; }
		public XpDateTime Date { get; set; }
		public string Reference { get; set; }
		public string Title { get; set; }
		public string WorkerName { get; set; }
	}
}
