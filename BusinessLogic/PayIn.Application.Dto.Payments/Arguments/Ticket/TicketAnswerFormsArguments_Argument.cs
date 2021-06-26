using System;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class TicketAnswerFormsArguments_Argument
    {
        public int Id { get; set; }
        public decimal? ValueNumeric { get; set; }
        public bool? ValueBool { get; set; }
        public DateTime? ValueDateTime { get; set; }
        public string ValueString { get; set; }

        public IEnumerable<TicketAnswerFormsArguments_Option> Options { get; set; }
    }
}
