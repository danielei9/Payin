using PayIn.Application.Dto.Transport.Arguments.TransportOperation;
using PayIn.Application.Dto.Transport.Results.TransportOperation;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Domain.Transport;

namespace PayIn.Application.Public.Handlers
{
	public class TransportOperationGetAllHandler :
		IQueryBaseHandler<TransportOperationGetAllArguments, TransportOperationGetAllResult>
	{		
		private readonly IEntityRepository<Log> Repository;
		private readonly IEntityRepository<Ticket> TicketRepository;
		private readonly IEntityRepository<GreyList> GreyListRepository;
		private readonly IEntityRepository<TransportOperation> TransportOperationRepository;
		private readonly IEntityRepository<TransportOperationTitle> TransportOperationTitleRepository;

		#region Constructors
		public TransportOperationGetAllHandler(
			IEntityRepository<GreyList> greyListRepository,
			IEntityRepository<Log> repository,
			IEntityRepository<Ticket> ticketRepository,
			IEntityRepository<TransportOperation> transportOperationRepository,
			IEntityRepository<TransportOperationTitle> transportOperationTitleRepository
		)
		{
			if (greyListRepository == null) throw new ArgumentNullException("greyListRepository");
			if (repository == null) throw new ArgumentNullException("repository");
			if (ticketRepository == null) throw new ArgumentNullException("ticketRepository");
			if (transportOperationRepository == null) throw new ArgumentNullException("transportOperationRepository");
			if (transportOperationTitleRepository == null) throw new ArgumentNullException("transportOperationTitleRepository");


			GreyListRepository = greyListRepository;
			Repository = repository;
			TicketRepository = ticketRepository;
			TransportOperationRepository = transportOperationRepository;
			TransportOperationTitleRepository = transportOperationTitleRepository;

		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<TransportOperationGetAllResult>> ExecuteAsync(TransportOperationGetAllArguments arguments)
		{
			if (arguments.Since.Value > arguments.Until.Value)
				return new ResultBase<TransportOperationGetAllResult>();

			var since = arguments.Since;
			var until = arguments.Until.AddDays(1);

			var greyListPending = (await GreyListRepository.GetAsync());
			var items = (await TransportOperationRepository.GetAsync())
				.Where(x =>
					x.OperationDate >= since &&
					x.OperationDate < until
					);

			if (!arguments.Filter.IsNullOrEmpty())
			{
				items = items.Where(x => (
					x.Uid.ToString().Contains(arguments.Filter) || // mejorar usando el int y no el string cuando mostremos el id en pantalla
					x.Login.Contains(arguments.Filter)
				));
			}

			var cardTitles = await TransportOperationTitleRepository.GetAsync("Title");

			var result = items
				.OrderByDescending(x => x.OperationDate)
				.Select(x => new
				{
					Id = x.Id,
					DateTime = x.OperationDate,
					Action = x.OperationType,
					Login = x.Login,
					Uid = x.Uid,
					State = ((x.Script == "" && (x.OperationType != OperationType.CreateCard && x.OperationType != OperationType.Search)) || x.Error != "") ? true : false,
					GreyListPending = greyListPending
						.Where(y => y.Uid == x.Uid && !y.Resolved && y.State == GreyList.GreyListStateType.Active)
						.Any(),
					LastOperationId = items
						.Where(z => z.Uid == x.Uid && z.Error == "" && z.Script != "" && x.OperationType == OperationType.Read)
						.OrderByDescending(z => z.OperationDate)
						.Select(z => (int?)z.Id)
						.FirstOrDefault() ?? x.Id,
					TitleRecharged = x.Price.Title.Name,
					ZoneRecharged = x.Price.Zone,
					TicketPay = (x.Ticket.Payments.FirstOrDefault().State == Common.PaymentState.Active) ? true : false,
					TicketPayError = x.Ticket.Payments.FirstOrDefault().ErrorText,
					RechargedApplied = x.ConfirmationDate != null ? true : false,
					RechargeConfirm = (x.ConfirmationDate != null && x.DateTimeEventError == null) ? true : false,
					ErrorRechargeconfirm = x.Error,
					Title1 = cardTitles.Where(y=> y.OperationId == x.Id).FirstOrDefault().Title.Name,					
					Title1Quantity = (decimal?)cardTitles.Where(y => y.OperationId == x.Id).FirstOrDefault().Quantity,
					Title1Zone = cardTitles.Where(y => y.OperationId == x.Id).FirstOrDefault().Zone,
					Title2 = (cardTitles.Where(y => y.OperationId == x.Id).Count() > 1) ? cardTitles.Where(y => y.OperationId == x.Id).OrderByDescending(y=>y.Id).FirstOrDefault().Title.Name : "",				
					Title2Quantity = (cardTitles.Where(y => y.OperationId == x.Id).Count() > 1) ? (decimal?)cardTitles.Where(y => y.OperationId == x.Id).OrderByDescending(y => y.Id).FirstOrDefault().Quantity : null,
					Title2Zone = (cardTitles.Where(y => y.OperationId == x.Id).Count() > 1) ? cardTitles.Where(y => y.OperationId == x.Id).OrderByDescending(y => y.Id).FirstOrDefault().Zone : null,

				})
				.ToList()
				.Select(x => new TransportOperationGetAllResult
				{
					Id = x.Id,
					DateTime = x.DateTime.ToUTC(),
					Action = x.Action.ToEnumAlias(),
					Login = x.Login,
					Uid = x.Uid,
					State = x.State,
					GreyListPending = x.GreyListPending,
					LastOperation = x.LastOperationId,
					TitleRecharged = x.TitleRecharged,
					ZoneRecharged = x.ZoneRecharged,
					TicketPay = x.TicketPay,
					TicketPayError = x.TicketPayError,
					RechargedApplied = x.RechargedApplied,
					RechargeConfirm = x.RechargeConfirm,
					ErrorRechargeConfirm = x.ErrorRechargeconfirm,
					Title1 = x.Title1,
					Title1Zone = x.Title1Zone,
					Title1Quantity = x.Title1Quantity,
					Title2 = x.Title2,
					Title2Quantity = x.Title2Quantity,
					Title2Zone = x.Title2Zone

				});

			return new ResultBase<TransportOperationGetAllResult> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
