using PayIn.Application.Dto.Payments.Arguments.Shipment;
using PayIn.Application.Dto.Payments.Results.Shipment;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ShipmentReceiptGetAllHandler :
		IQueryBaseHandler<ShipmentReceiptGetAllArguments, ShipmentReceiptGetAllResult>
	{
		private readonly IEntityRepository<Ticket> Repository;
		private readonly ISessionData SessionData;


		#region Constructors
		public ShipmentReceiptGetAllHandler(IEntityRepository<Ticket> repository, ISessionData sessionData)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ShipmentReceiptGetAllResult>> IQueryBaseHandler<ShipmentReceiptGetAllArguments, ShipmentReceiptGetAllResult>.ExecuteAsync(ShipmentReceiptGetAllArguments arguments)
		{
			var now = DateTime.Now;
			var nowUTC = now.ToUTC();
			if (arguments.Since.Value > arguments.Until.Value)
				return new ResultBase<ShipmentReceiptGetAllResult>();
			var since = arguments.Since;
			var until = arguments.Until.AddDays(1);
			var items = (await Repository.GetAsync())
				.Where(x => x.PaymentUser.Login == SessionData.Login && x.ShipmentId != null && x.Date < until && x.Date >= since);

			var result = items
				.Select(x => new
				{
					Id = x.Id,
					Since = x.Since,
					Until = x.Until,
					Amount = x.Amount,
					Reference = x.Reference,
					Paid = (x.Payments.Any(a => a.State == PaymentState.Active)) ? true : false,
					Finished = (x.Until < nowUTC) ? true : false,
					Lines = x.Lines
						.Select(y => new ShipmentReceiptGetAllResult_TicketLine
						{
							Id = y.Id,
							Title = y.Title,
							Amount = y.Amount,
							Quantity = y.Quantity
						})
				})
			   .OrderBy(x => x.Since)
				.ToList()
				.Select(x => new ShipmentReceiptGetAllResult
				{
					Id = x.Id,
					Since = x.Since.ToUTC(),
					Until = x.Until.ToUTC(),
					Amount = x.Amount,
					Reference = x.Reference,
					Paid = x.Paid,
					Finished = x.Finished,
					Lines = x.Lines
						 .Select(y => new ShipmentReceiptGetAllResult_TicketLine
						 {
							 Id = y.Id,
							 Title = y.Title,
							 Amount = y.Amount,
							 Quantity = y.Quantity
						 })
				});

			return new ResultBase<ShipmentReceiptGetAllResult> { Data = result };

		}
		#endregion ExecuteAsync
	}
}