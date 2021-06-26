using PayIn.Common;
using System;
using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class MobileEntranceGetResult
	{
		public int Id					               { get; set; }
		public string Name				               { get; set; }
        public string EntranceTypeConditions           { get; set; }
        public string EventConditions		           { get; set; }
		public DateTime Timestamp                      { get; set; }
		public decimal Price                           { get; set; }
		public int? TicketId                           { get; set; }
		public string Place				               { get; set; }
		public EntranceState State                     { get; set; }
		public string Description		               { get; set; }
		public string PhotoUrl			               { get; set; }
        public string EventPhotoUrl                    { get; set; }
		public XpDateTime EventStart	               { get; set; }
		public XpDateTime EventEnd		               { get; set; }
		public XpDateTime CheckInStart	               { get; set; }
		public XpDateTime CheckInEnd	               { get; set; }
		public string UserName                         { get; set; }
		public string LastName						   { get; set; }
		public string UserTaxNumber                    { get; set; }
		public string Code                             { get; set; }
		public string CodeText                         { get; set; }
		public EntranceSystemType CodeType             { get; set; }
		public string MapUrl                           { get; set; }

		public IEnumerable<MobileEntranceGetResult_Activities> Activities { get; set; }
	}
}
