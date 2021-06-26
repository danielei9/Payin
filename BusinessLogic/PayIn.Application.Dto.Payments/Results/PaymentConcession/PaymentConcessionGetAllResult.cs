using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.Application.Dto.Payments.Results
{
    public class PaymentConcessionGetAllResult
    {
        public int Id { get; set; }
        // Información comercial
        public string Name { get; set; }
        public string Phone { get; set; }
		public string Email { get; set; }

		//public string Address { get; set; }
		//public string Observations { get; set; }
		//


		//public decimal PayinCommission { get; set; }
		//public decimal LiquidationAmountMin { get; set; }
		//public int? TicketTemplateId { get; set; }
		//public string TicketTemplateName { get; set; }
		//public bool OnlineCartActivated { get; set; }
		//public bool CanHasPublicEvent { get; set; }
		//public bool AllowUnsafePayments { get; set; }
	}
}
