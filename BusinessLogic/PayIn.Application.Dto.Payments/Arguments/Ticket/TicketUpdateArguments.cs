using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class TicketUpdateArguments : IUpdateArgumentsBase<Ticket>
	{
		#region Id
		public int Id { get; set; }
		#endregion Id

		#region Reference
		public string Reference { get; set; }
		#endregion Reference

		#region Title
		public string Title { get; set; }
		#endregion Title

		#region Date
		public DateTime? Date { get; set; }
		#endregion Date

		#region Amount
		public Decimal Amount { get; set; }
		#endregion Amount

		#region State
		public TicketStateType State { get; set; }
		#endregion State

		#region Constructor
		public TicketUpdateArguments(string reference, string title, DateTime? date, Decimal amount, TicketStateType state) //string currency ,DateTime? until
		{
			//Currency = currency;
			Reference = reference;
			Title = title;
			Date = date;
			//Until = until;
			Amount = amount;
			State = state;
		}
		#endregion Constructor
    }
}
