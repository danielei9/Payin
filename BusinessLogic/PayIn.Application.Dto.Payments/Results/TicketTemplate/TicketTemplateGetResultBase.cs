using PayIn.Common;
using Xp.Application.Dto;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results.TicketTemplate
{
	public class TicketTemplateGetResultBase : ResultBase<TicketTemplateGetResult>
	{
		public string TicketText { get; set; }
	}
}
