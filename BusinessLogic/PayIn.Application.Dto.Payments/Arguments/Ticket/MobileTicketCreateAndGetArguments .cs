using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobileTicketCreateAndGetArguments : ICreateArgumentsBase<Ticket>
	{
                   public string Reference { get; set; }
		           public long? Uid { get; set; }
		[Required] public XpDateTime Date { get; set; }
        [Required] public int ConcessionId { get; set; }
                   public int? EventId { get; set; }
                   public int? LiquidationConcession { get; set; }
                   public int? TransportPrice { get; set; }
                   public decimal? Amount { get; set; }
                   public TicketType Type { get; set; }
                   public IEnumerable<MobileTicketCreateAndGetArguments_TicketLine> Lines { get; set; }
		           public IEnumerable<MobileTicketCreateAndGetArguments_Payment> Payments { get; set; }
		           public IEnumerable<MobileTicketCreateAndGetArguments_Group> Groups { get; set; }
		           public IEnumerable<TicketAnswerFormsArguments_Form> Forms { get; set; }

        #region Constructor
        public MobileTicketCreateAndGetArguments(string reference, long? uid, DateTime date, int concessionId, int? eventId, int? liquidationConcession, int? transportPrice, decimal? amount, TicketType? type,
			IEnumerable<MobileTicketCreateAndGetArguments_TicketLine> lines, IEnumerable<MobileTicketCreateAndGetArguments_Payment> payments, IEnumerable<MobileTicketCreateAndGetArguments_Group> groups)
		{
			Reference = reference ?? "";
			Uid = uid;
			Date = date;
			ConcessionId = concessionId;
			EventId = eventId;
			LiquidationConcession = liquidationConcession;
			TransportPrice = transportPrice;
			Amount = amount;
			Type = type ?? TicketType.Ticket;
			Lines = lines ?? new List<MobileTicketCreateAndGetArguments_TicketLine>();
			Payments = payments ?? new List<MobileTicketCreateAndGetArguments_Payment>();
			Groups = groups ?? new List<MobileTicketCreateAndGetArguments_Group>();
		}
		#endregion Constructor
	}
}
