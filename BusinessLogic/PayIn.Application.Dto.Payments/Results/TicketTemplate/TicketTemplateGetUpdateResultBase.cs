using PayIn.Common;
using Xp.Application.Dto;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results.TicketTemplate
{
	public class TicketTemplateGetUpdateResultBase : ResultBase<TicketTemplateGetUpdateResult>
	{
		public string TicketText { get; set; }
	}
}
