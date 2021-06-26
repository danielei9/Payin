using PayIn.Common;
using System.Collections.Generic;
using Xp.Application.Dto;
using Xp.Common;

namespace PayIn.Application.Dto.Results.Main
{
	public partial class MainMobileGetAllResultBase : ResultBase<MainMobileGetAllResult>
	{
		public class Ticket
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
			public TicketType       Type                 { get; set; }
			public TicketStateType  State                { get; set; }
		}
		public class Favorite
		{
			public int              Id                { get; set; }
			public string           Title             { get; set; }
			public string           Subtitle          { get; set; }
			public int              VisualOrder       { get; set; }
			public string           ImagePath         { get; set; }
			public decimal          Balance           { get; set; }
			public PinType          Type              { get; set; }
		}

		public int                   NumNotifications { get; set; }
		public int					 NumReceipts	  { get; set; }
		public string                AppVersion       { get; set; }
		public XpDuration            SumChecks        { get; set; }
		public XpDuration            CheckDuration    { get; set; }
		public bool                  Working          { get; set; }
		public IEnumerable<Favorite> Favorites        { get; set; }
		public IEnumerable<Ticket>   Tickets          { get; set; }
		public bool                  WorkerHasConcession { get; set; }
		public bool                  UserHasConcession { get; set; }
	}
}
