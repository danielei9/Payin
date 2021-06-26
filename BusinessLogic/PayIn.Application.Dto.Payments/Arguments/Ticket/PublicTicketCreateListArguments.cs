using System.Collections.Generic;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class PublicTicketCreateListArguments : IArgumentsBase
	{
        /// <summary>
        /// Lista de tickets
        /// </summary>
		public IEnumerable<PublicTicketCreateListArguments_Ticket> Tickets { get; set; }
        
        #region Constructor
        public PublicTicketCreateListArguments(IEnumerable<PublicTicketCreateListArguments_Ticket> tickets)
		{
            Tickets = tickets;
		}
		#endregion Constructor
	}
}
