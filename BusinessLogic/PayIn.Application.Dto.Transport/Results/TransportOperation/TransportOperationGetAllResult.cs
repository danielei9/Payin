using PayIn.Domain.Transport.Eige.Enums;
using Xp.Common;

namespace PayIn.Application.Dto.Transport.Results.TransportOperation
{
	public class TransportOperationGetAllResult
	{
		public int        Id              { get; set; }
		public XpDateTime DateTime	      { get; set; }			
		public string     Login           { get; set; }
		public long?      Uid             { get; set; }		
		public bool       State           { get; set; }
		public string     Action          { get; set; }
		public bool       GreyListPending { get; set; }
		public int        LastOperation   { get; set; }
		public bool       TicketPay       { get; set; }
		public string     TicketPayError  { get; set; }
		public bool       RechargedApplied { get; set; }
		public bool       RechargeConfirm { get; set; }
		public string ErrorRechargeConfirm { get; set; }
		public string TitleRecharged      { get; set; }
		public EigeZonaEnum? ZoneRecharged { get; set; }
		public string Title1 { get; set; }
		public decimal? Title1Quantity { get; set; }
		public string Title2 { get; set; }
		public decimal? Title2Quantity { get; set; }
		public EigeZonaEnum? Title1Zone { get; set; }
		public EigeZonaEnum? Title2Zone { get; set; }
	}
}
