using System.Collections.Generic;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class TicketAnswerFormsArguments_Form
    {
        public int Id { get; set; }

        public IEnumerable<TicketAnswerFormsArguments_Argument> Arguments { get; set; }
    }
}
