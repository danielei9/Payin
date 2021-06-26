using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Internal.Arguments;
using PayIn.Application.Dto.Internal.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Internal;
using PayIn.Domain.Internal.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;


namespace PayIn.Application.Internal.Handlers
{
	[XpLog("Ticket", "PayWeb")]
	public class TicketPayWebHandler :
		IServiceBaseHandler<TicketPayWebArguments>
	{
        [Dependency] public IPaymentGatewayAdapter PaymentGatewayAdapter { get; set; }

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(TicketPayWebArguments arguments)
		{
			var request = await PaymentGatewayAdapter.WebCardRequestAsync(
				arguments.CommerceCode,
                0, 0,
                arguments.PublicTicketId, arguments.PublicPaymentId,
                arguments.OrderId, arguments.PaymentMediaCreateType, arguments.Amount);
			return new PaymentMediaCreateWebCardResult
			{
				Id = 0,
				TicketId = arguments.PublicTicketId,
				Verb = "POST",
#if PRODUCTION
				Url = "https://sis.redsys.es/sis/realizarPago",
#else
				Url = "https://sis-t.redsys.es:25443/sis/realizarPago",
#endif
				Request = request,
				Arguments = request
					.SplitString("&")
					.ToDictionary(
						x => x.RightError(x.IndexOf(":")),
						x => x.LeftError(x.Length - x.IndexOf(":") - 1)
					)
			};
		}
#endregion ExecuteAsync
	}
}
