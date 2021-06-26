﻿using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class TicketMobileGetDuesResult
	{
		public int              Id                   { get; set; }
		public string           Reference            { get; set; }
		public string           SupplierName         { get; set; }
		public decimal          Amount               { get; set; }
		public decimal          PayedAmount          { get; set; }
		public string           Currency             { get; set; }
		public XpDateTime		Date                 { get; set; }
		public XpDateTime       Since                { get; set; }
		public XpDateTime       Until                { get; set; }
	}
}
