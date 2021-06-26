using PayIn.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xp.Common;

namespace PayIn.Application.Dto.Arguments
{
	public class MobileMainSynchronizeArguments_Ticket
    {
        public string Reference { get; set; } = "";
        public string ExternalLogin { get; set; } = "";
        public int? TransportPrice { get; set; }
		public long? Uid { get; set; }
		public int? ESeq { get; set; }
		[Required]
		public int Seq { get; set; }
        public TicketType Type { get; set; }
        [Required]public XpDateTime Date { get; set; }
        public string Login { get; set; } = "";
        public int? EventId { get; set; }
        public int? PaymentConcessionId { get; set; }

        public IEnumerable<MobileMainSynchronizeArguments_Wallet> Wallets { get; set; }
        public IEnumerable<MobileMainSynchronizeArguments_TicketLine> Lines { get; set; }
		public IEnumerable<MobileMainSynchronizeArguments_Payment> Payments { get; set; }
		public IEnumerable<MobileMainSynchronizeArguments_Form> Forms { get; set; }
	}
}