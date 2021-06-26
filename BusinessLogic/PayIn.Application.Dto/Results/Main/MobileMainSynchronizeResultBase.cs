using System.Collections.Generic;
using Xp.Application.Dto;

namespace PayIn.Application.Dto.Results
{
	public partial class MobileMainSynchronizeResultBase : ResultBase<MobileMainSynchronizeResult>
	{
        public IEnumerable<MobileMainSynchronizeResult_ServiceUser> ServiceUsers { get; set; }
        public IEnumerable<MobileMainSynchronizeResult_ServiceCardNotEmited> ServiceCardsNotEmited { get; set; }
        public IEnumerable<MobileMainSynchronizeResult_ServiceCardEmited> ServiceCardsEmited { get; set; }
		public IEnumerable<MobileMainSynchronizeResult_Event> Events { get; set; }
		public IEnumerable<MobileMainSynchronizeResult_Page> Pages { get; set; }
		public IEnumerable<MobileMainSynchronizeResult_Product> Products { get; set; }
		public IEnumerable<MobileMainSynchronizeResult_ServiceCategory> Categories { get; set; }
        public IEnumerable<MobileMainSynchronizeResult_BlackList> BlackList { get; set; }
        public IEnumerable<MobileMainSynchronizeResult_GreyList> GreyList { get; set; }
        public MobileMainSynchronizeResult_ConfigCard ConfigCard { get; set; }
        public IEnumerable<MobileMainSynchronizeResult_ConfigCard> ConfigCards { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}
