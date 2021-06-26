using PayIn.Application.Dto.Payments.Arguments.Shipment;
using PayIn.Application.Dto.Payments.Results.Shipment;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Public;
using serV = PayIn.Domain.Payments.Shipment;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using PayIn.Domain.Payments;

namespace PayIn.Application.Public.Handlers
{
	public class ShipmentTicketGetAllHandler :
		IQueryBaseHandler<ShipmentTicketGetAllArguments, ShipmentTicketGetAllResult>
	{
		private readonly IEntityRepository<serV> _Repository;
		private readonly IEntityRepository<Ticket> TicketRepository;
		private readonly ISessionData _SessionData;

		#region Constructors
		public ShipmentTicketGetAllHandler(IEntityRepository<serV> repository, ISessionData sessionData, IEntityRepository<Ticket> ticketRepository)
		{
			if (repository == null)	throw new ArgumentNullException("repository");
			_Repository = repository;

			if (sessionData == null) throw new ArgumentNullException("sessionData");
			_SessionData = sessionData;

			if (ticketRepository == null) throw new ArgumentNullException("ticketRepository");
			TicketRepository = ticketRepository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ShipmentTicketGetAllResult>> ExecuteAsync(ShipmentTicketGetAllArguments arguments)
		{
			var items = (await TicketRepository.GetAsync())
				.Where(x => x.State != TicketStateType.Cancelled);
			var now = DateTime.Now;
			var nowUTC = now.ToUTC();

			var result = items
				.Where(x => x.ShipmentId == arguments.Id)
				.Select(x => new
				{
					Id = x.Id,
					Started = (x.Since < nowUTC)? true : false,
					Date = (x.Payments.Count()!=0) ? x.Payments.FirstOrDefault().Date : DateTime.MinValue,
					Payed = (x.Payments.Sum(y => y.Amount) >= x.Amount) ? true : false,
					Amount = x.Amount,
					Login = x.PaymentUser.Login,
					Name = x.PaymentUser.Name,
					Reference = x.Reference,
					TicketLines = x.Lines
						.Select(y => new
						{
							Id = y.Id,
							Title = y.Title,
							Amount = y.Amount,
							Quantity = y.Quantity
						})
				})
				.OrderBy(x => x.Date)
				.ToList()
				.Select(x => new ShipmentTicketGetAllResult
				{
					Id = x.Id,
					Started = x.Started,
					Payed = x.Payed,
					Date = x.Date.ToUTC(),
					Amount = x.Amount,
					Login = x.Login,
					Name = x.Name,
					Reference = x.Reference,
					Lines = x.TicketLines
						 .Select(y => new ShipmentTicketGetAllResult_TicketLine
						 {
							 Id = y.Id,
							 Title = y.Title,
							 Amount = y.Amount,
							 Quantity = y.Quantity
						 })
				});

			return new ResultBase<ShipmentTicketGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
