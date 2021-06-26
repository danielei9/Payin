using PayIn.Common;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Payments.Results
{
	public class ExhibitorMobileGetResult
    {
		public int           Id              { get; set; }
		public string        Name            { get; set; }
		public string        Phone           { get; set; }
		public string        Address         { get; set; }
		public string        Email           { get; set; }
		public ContactState? ContactState    { get; set; }
		public int?          ConcessionId    { get; set; }
		public string        ConcessionLogin { get; set; }

		public IEnumerable<ExhibitorMobileGetResult_Product> Products { get; set; }
		public IEnumerable<ExhibitorMobileGetResult_Event> Events { get; set; }
		public IEnumerable<ExhibitorMobileGetResult_Notification> Notifications { get; set; }
	}
}
