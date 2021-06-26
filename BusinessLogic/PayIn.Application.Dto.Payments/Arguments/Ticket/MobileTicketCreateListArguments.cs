using PayIn.Domain.Payments;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class MobileTicketCreateListArguments : ICreateArgumentsBase<Ticket>
    {
        [Required] public int ConcessionId { get; set; }
        public int? LiquidationConcession { get; set; }

        public List<MobileTicketCreateListArguments_Ticket> List { get; set; }
	}
}
