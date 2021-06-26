using PayIn.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class ApiEntranceGetAllResult
	{
		public int					Id					{ get; set; }
        public long                 Code				{ get; set; }
        public string				UserName			{ get; set; }
		public string				LastName			{ get; set; }
		public string				Login   			{ get; set; }
		public EntranceState	    State				{ get; set; }
		public int?					EntraceTypeId		{ get; set; }
		public int					EventId     		{ get; set; }
		public string               EventName           { get; set; }
        public int?                 TicketLineId		{ get; set; }
		public string				EntranceTypeName	{ get; set; }
		public decimal              TotalAmount         { get; set; }
		public decimal				ExtraPrice			{ get; set; }
		public int?				    TicketId			{ get; set; }
	}
}
