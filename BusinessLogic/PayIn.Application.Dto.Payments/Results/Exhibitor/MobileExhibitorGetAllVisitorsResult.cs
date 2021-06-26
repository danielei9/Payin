using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.Application.Dto.Payments.Results
{
	public class MobileExhibitorGetAllVisitorsResult
	{
		public int Id { get; set; }
		public int? VisitorEntranceId { get; set; }
		public string VisitorName { get; set; }
		public string VisitorLogin { get; set; }
		public int ExhibitorId { get; set; }
		public int EventId { get; set; }
		public string EventName { get; set; }
	}
}
