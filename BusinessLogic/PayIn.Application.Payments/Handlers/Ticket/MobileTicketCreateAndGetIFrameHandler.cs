using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xp.Application.Dto;

namespace PayIn.Application.Payments.Handlers
{
    public class MobileTicketCreateAndGetIFrameHandler :
        IServiceBaseHandler<TicketCreateAndGetIFrameArguments>
    {
        [Dependency] public MobileTicketCreateAndGetHandler TicketMobileCreateAndGetHandler { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TicketCreateAndGetIFrameArguments arguments)
		{
			var now = DateTime.UtcNow;

            var ticket = await TicketMobileCreateAndGetHandler.CreateTicketAsync(arguments.Reference, arguments.Uid, arguments.Date, arguments.ConcessionId, arguments.EventId, arguments.Lines, null, arguments.Forms, arguments.LiquidationConcession, arguments.TransportPrice, arguments.Type, arguments.Email, arguments.Login, "", arguments.Amount, now, true, false);

            return new
            {
                Id = ticket.Id,
                Request = new Dictionary<string, string>
                {
                    { "ticketId", ticket.Id.ToString() }
                }
            };
		}
        #endregion ExecuteAsync
    }
}
