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
	public class ShipmentGetAllHandler :
		IQueryBaseHandler<ShipmentGetAllArguments, ShipmentGetAllResult>
	{
		private readonly IEntityRepository<Shipment> Repository;
		private readonly ISessionData SessionData;


		#region Constructors
		public ShipmentGetAllHandler(IEntityRepository<Shipment> repository, ISessionData sessionData)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
			if (sessionData == null) throw new ArgumentNullException("sessionData");
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ShipmentGetAllResult>> IQueryBaseHandler<ShipmentGetAllArguments, ShipmentGetAllResult>.ExecuteAsync(ShipmentGetAllArguments arguments)
		{

			var now =  DateTime.Now;
			var nowUTC = now.ToUTC();
			var items = (await Repository.GetAsync())
			.Where(x => x.Concession.Concession.Supplier.Login == SessionData.Login && x.State == Common.ShipmentState.Active);				

			var result = items			
			.Select(x => new 
			{
				Id = x.Id,
				Since = x.Since,
				Until = x.Until,
				Amount =  x.Amount,
				Name = x.Name,				
				Started = (x.Since <= now) ? true : false,
				Finished = (x.Until <= now) ? true : false,
				NumberTickets = x.Tickets.Where(y => y.State != TicketStateType.Cancelled).Count(),
				NumberPayers = (x.Tickets
					.Where(y => y.State != TicketStateType.Cancelled && (y.Payments
						.Where(z => z.State == PaymentState.Active)
						.Sum(z => z.Amount) >= y.Amount
					))
					.Count()
				)
			})
				.OrderByDescending(x => x.Since)
				.ToList()
				.Select(x => new ShipmentGetAllResult
				{
					Id = x.Id,
					Since = x.Since.ToUTC(),
					Until = x.Until.ToUTC(),
					Amount = x.Amount,
					Name = x.Name,
					Started = x.Started,
					Finished = x.Finished,
					NumberPayers = x.NumberPayers,
					NumberTickets = x.NumberTickets
				})
				.ToList()
			;

			return new ResultBase<ShipmentGetAllResult> { Data = result };

		}
		#endregion ExecuteAsync
	}
}