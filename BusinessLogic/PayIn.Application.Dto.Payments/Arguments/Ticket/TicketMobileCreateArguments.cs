using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class TicketMobileCreateArguments : ICreateArgumentsBase<Ticket>
	{
		           public string                                              Reference             { get; set; }
		[Required] public XpDateTime                                          Date                  { get; set; }
		[Required] public int                                                 ConcessionId          { get; set; }
				   public IEnumerable<TicketMobileCreateArguments_TicketLine> Lines                 { get; set; }
		           public int?                                                LiquidationConcession { get; set; }
		           public TicketType                                          Type                  { get; set; }

		#region Constructor
		public TicketMobileCreateArguments(string reference, DateTime date, int concessionId, IEnumerable<TicketMobileCreateArguments_TicketLine> lines, int? liquidationConcession, TicketType? type)
		{
			Reference    = reference ?? "";
			Date         = date;
			ConcessionId = concessionId;
			Lines  = lines ?? new List<TicketMobileCreateArguments_TicketLine>();
			LiquidationConcession = liquidationConcession;
			Type = type ?? TicketType.Ticket;
		}
		#endregion Constructor
	}
}
