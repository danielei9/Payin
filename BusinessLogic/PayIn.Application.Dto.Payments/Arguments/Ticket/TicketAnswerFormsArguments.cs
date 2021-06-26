using PayIn.Domain.Payments;
using System.Collections.Generic;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class TicketAnswerFormsArguments : ICreateArgumentsBase<Ticket>
	{
		public int Id { get; set; }
		public IEnumerable<TicketAnswerFormsArguments_Form> Forms { get; set; }
        
        #region Constructor
        public TicketAnswerFormsArguments(IEnumerable<TicketAnswerFormsArguments_Form> forms)
		{
			Forms = forms;
		}
		#endregion Constructor
	}
}
