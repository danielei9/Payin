using PayIn.Application.Dto.Payments.Arguments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;

namespace PayIn.Application.Payments.Handlers
{
	[XpLog("Ticket", "PayUser")]
	[XpAnalytics("Ticket", "PayUser")]
	public class PublicTicketPayUserHandler :
		IServiceBaseHandler<PublicTicketPayUserArguments>
	{
		private readonly MobileTicketPayV3Handler MobileTicketPayV3Handler;

		#region Contructors
		public PublicTicketPayUserHandler(
			MobileTicketPayV3Handler mobileTicketPayV3Handler
		)
		{
			if (mobileTicketPayV3Handler == null) throw new ArgumentNullException("mobileTicketPayV3Handler");
			
			MobileTicketPayV3Handler = mobileTicketPayV3Handler;
		}
		#endregion Contructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PublicTicketPayUserArguments arguments)
		{
			var now = DateTime.Now.ToUTC();

			return await MobileTicketPayV3Handler.ExecuteInternalAsync(
				now,
				arguments.Id,
				arguments.Login,
				arguments.Name,
				arguments.TaxNumber,
				arguments.TaxName,
				arguments.TaxAddress,
				arguments.PaymentMedias
					.Select(x => new MobileTicketPayV3Arguments_PaymentMedia
					{
						Id = x.Id,
						Balance = x.Balance,
						Order = x.Order
					}),
				null,
				arguments.Pin
			);
		}
		#endregion ExecuteAsync
	}
}