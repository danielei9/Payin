using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ApiEntranceGetAllHandler : 
		IQueryBaseHandler<ApiEntranceGetAllArguments, ApiEntranceGetAllResult>
	{
		private readonly IEntityRepository<Entrance> Repository;
		private readonly IEntityRepository<TicketLine> TLRepository;
		private readonly ISessionData SessionData;

		#region Constructors
		public ApiEntranceGetAllHandler(
			IEntityRepository<Entrance> repository,
			IEntityRepository<TicketLine> tlRepository,
		ISessionData sessionData
			)
		{
			if (repository == null) throw new ArgumentNullException("entranceRepository");
			if (tlRepository == null) throw new ArgumentNullException("tlRepository");
			if (sessionData == null) throw new ArgumentNullException("sessionData");

			Repository = repository;
			TLRepository = tlRepository;
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<ResultBase<ApiEntranceGetAllResult>> ExecuteAsync (ApiEntranceGetAllArguments arguments)
		{
			var ticketlines = (await TLRepository.GetAsync())
				.Where(x =>
					x.Type == TicketLineType.ExtraPrice &&
					x.EntranceTypeId == arguments.Id)
				.Select(y => new {
					y.TicketId,
					y.Amount
				})
				.ToList();

			var entrances = (await Repository.GetAsync())
				.Where(x =>
					x.State != EntranceState.Deleted &&
					x.EntranceTypeId == arguments.Id
				)
				.OrderBy(x => x.UserName)
				.ThenBy(x => x.LastName)
				.Select(x => new ApiEntranceGetAllResult
				{
					Id					=   x.Id,
					Code				=   x.Code,
					UserName			=   x.UserName,
					LastName			=	x.LastName,
					Login				=   x.Login,
					EntraceTypeId		=   x.EntranceTypeId,
					EventId             =   x.EventId,
					EventName           =   x.Event.Name,
					TicketLineId		=   x.TicketLineId,
					State				=   x.State,
					EntranceTypeName	=	x.EntranceType.Name,
					TotalAmount         =   ((decimal?) x.TicketLine.Amount) ?? 0,
					TicketId			=   x.TicketLine.TicketId,
					ExtraPrice          =   0
				});

			var result = new List<ApiEntranceGetAllResult>();
			
			foreach (var entrance in entrances)
			{
				result.Add(entrance);
				foreach (var ticketline in ticketlines)
				{
					if (entrance.TicketId == ticketline.TicketId)
					{
						var aux = entrance;
						result.Remove(entrance);
						aux.ExtraPrice = ticketline.Amount;
						result.Add(aux);
					}
				}
			}

			var items = result.AsEnumerable();

			return new ResultBase<ApiEntranceGetAllResult> { Data = items };
		}
		#endregion ExecuteAsync
	}
}
