using PayIn.Common;
using PayIn.Domain.Payments;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public class MobileMainSynchronizeArguments : ICreateArgumentsBase<Ticket>
	{
		[Required] public int ConcessionId { get; set; }
        public int? SystemCardId { get; set; }
        public int? LiquidationConcession { get; set; }
		public LanguageEnum? Language { get; set; } = LanguageEnum.Spanish;

		public IEnumerable<MobileMainSynchronizeArguments_Ticket> Tickets { get; set; } = new List<MobileMainSynchronizeArguments_Ticket>();
		public IEnumerable<MobileMainSynchronizeArguments_ServiceCardEmition> ServiceCardEmitions { get; set; } = new List<MobileMainSynchronizeArguments_ServiceCardEmition>();
        public IEnumerable<MobileMainSynchronizeArguments_ServiceCardReturned> ServiceCardReturned { get; set; } = new List<MobileMainSynchronizeArguments_ServiceCardReturned>();
        public IEnumerable<MobileMainSynchronizeArguments_GreyList> GreyList { get; set; } = new List<MobileMainSynchronizeArguments_GreyList>();
		public IEnumerable<MobileMainSynchronizeArguments_BlackListUpdates> BlackList { get; set; } = new List<MobileMainSynchronizeArguments_BlackListUpdates>();
	}
}
