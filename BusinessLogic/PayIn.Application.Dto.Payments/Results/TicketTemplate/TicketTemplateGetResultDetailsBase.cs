using PayIn.Common;
using Xp.Application.Dto;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results.TicketTemplate
{
	public class TicketTemplateGetDetailsResultBase : ResultBase<TicketTemplateGetDetailsResult>
	{
		public string Name { get; set; }
		public string RegEx { get; set; }
		public string PreviousTextPosition { get; set; }
		public string BackTextPosition { get; set; }
		public int AmountPosition { get; set; }
		public string DateFormat { get; set; }
		public int? DatePosition { get; set; }
		public DecimalCharDelimiter DecimalCharDelimiter { get; set; }
		public int? ReferencePosition { get; set; }
		public int? TitlePosition { get; set; }
		public int? WorkerPosition { get; set; }
	}
}
