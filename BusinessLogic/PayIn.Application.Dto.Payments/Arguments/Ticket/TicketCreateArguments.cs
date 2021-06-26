using PayIn.Common;
using PayIn.Domain.Payments;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class TicketCreateArguments : ICreateArgumentsBase<Ticket>
	{
		[Display(Name = "resources.ticket.amount")]     [Required] public decimal Amount { get; private set; }
		[Display(Name = "resources.ticket.reference")]             public string Reference { get; private set; }
		[Display(Name = "resources.ticket.title")]                 public string Title { get; private set; }
		[Display(Name = "resources.ticket.date")]                  public XpDateTime Date { get; private set; }
		[Display(Name = "resources.ticket.concession")] [Required] public int ConcessionId { get; private set; }
		[Display(Name = "resources.ticket.since")]                 public XpDateTime Since { get; private set; }
		[Display(Name = "resources.ticket.until")]                 public XpDateTime Until { get; private set; }
		                                                           public IEnumerable<TicketCreateArguments_TicketLine> Lines { get; set; }
		                                                           public TicketType Type { get; set; }

		#region Constructors
		public TicketCreateArguments(decimal amount, string reference, string title, XpDateTime date, int concessionId, XpDateTime since, XpDateTime until, IEnumerable<TicketCreateArguments_TicketLine> lines, TicketType? type)
		{
			Amount = amount;
			Reference = reference;
			Title = title;
			Date = date;
			ConcessionId = concessionId;
			Since = since;
			Until = until;
			Lines = lines;
			Type = type ?? TicketType.Ticket;
		}
		#endregion Constructors
	}
}