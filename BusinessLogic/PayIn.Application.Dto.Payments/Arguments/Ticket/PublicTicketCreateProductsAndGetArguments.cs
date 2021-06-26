using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PublicTicketCreateProductsAndGetArguments : ICreateArgumentsBase<Ticket>
	{
		[Required] public string                                                            Email        { get; set; }
		[Required] public string                                                            Login        { get; set; }
		           public string                                                            Reference    { get; set; }
		[Required] public int                                                               ConcessionId { get; set; }
		           public int?                                                              EventId      { get; set; }
		           public DateTime                                                          Now          { get; set; }
		           public TicketType                                                        Type         { get; set; }
		           public IEnumerable<PublicTicketCreateProductsAndGetArguments_TicketLine> Lines        { get; set; }
        
        #region Constructor
        public PublicTicketCreateProductsAndGetArguments(
			string login,
			string reference,
			int concessionId,
			IEnumerable<PublicTicketCreateProductsAndGetArguments_TicketLine> lines,
			TicketType? type
#if DEBUG
			, DateTime? now
#endif //DEBUG
		)
		{
			Login = login;
			Reference    = reference ?? "";
			ConcessionId = concessionId;
			Lines  = lines ?? new List<PublicTicketCreateProductsAndGetArguments_TicketLine>();
			Type = type ?? TicketType.Ticket;
#if DEBUG
			Now = now ?? DateTime.Now;
#else
			Now = DateTime.UtcNow;
#endif // DEBUG
		}
		#endregion Constructor
	}
}
