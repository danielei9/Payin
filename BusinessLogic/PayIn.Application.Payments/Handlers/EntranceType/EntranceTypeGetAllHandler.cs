using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	class EntranceTypeGetAllHandler : 
		IQueryBaseHandler<EntranceTypeGetAllArguments, EntranceTypeGetAllResult>
	{
		[Dependency] public IEntityRepository<Event> Repository { get; set; }
		[Dependency] public IEntityRepository<ServiceGroup> ServiceGroupRepository { get; set; }
		//private readonly IEntityRepository<TicketLine> TLRepository;
		[Dependency] public ISessionData SessionData { get; set; }
		
		#region ExecuteAsync
		public async Task<ResultBase<EntranceTypeGetAllResult>> ExecuteAsync (EntranceTypeGetAllArguments arguments)
		{
			var serviceGroups = (await ServiceGroupRepository.GetAsync());

			var items = (await Repository.GetAsync())
				.Where(x =>
					(x.State != EventState.Deleted)
				);

			if (arguments.EventId > 0)
				items = items
					.Where(x =>
						x.Id == arguments.EventId
					);

			var result = items
				.Select(a => new
				{
					Name = a.Name,
					EntranceTypes = a.EntranceTypes
						.Where(x =>
							(x.State != EntranceTypeState.Deleted) &&
							(
								(arguments.Filter == null) ||
								(arguments.Filter == "") ||
								(x.Name.Contains(arguments.Filter))
							)
						)
						.Select(x => new
						{
							x.Id,
							x.Name,
							x.Price,
							x.MaxEntrance,
							x.ExtraPrice,
							SellStart = (x.SellStart == XpDateTime.MinValue) ? null : (DateTime?)x.SellStart,
							SellEnd = (x.SellEnd == XpDateTime.MaxValue) ? null : (DateTime?)x.SellEnd,
							x.State,
							x.Visibility,
							SellEntrances = x.Entrances
								.Where(y =>
									y.State == EntranceState.Active ||
									y.State == EntranceState.Validated ||
									y.State == EntranceState.Suspended
								)
								.Count(),
							IsVisible = x.IsVisible,
							TotalAmount = x.Entrances
								.Where(y =>
									//y.State != EntranceState.Pending &&
									//y.State != EntranceState.Deleted
									y.State == EntranceState.Active ||
									y.State == EntranceState.Validated ||
									y.State == EntranceState.Suspended
								)
								.Sum(y => (decimal?)y.TicketLine.Amount) ?? 0,
							TotalExtraPrice = x.TicketLines
								.Where(y=>
									y.EntranceTypeId == x.Id && 
									y.Type==TicketLineType.ExtraPrice)
								.Sum(z=>
									(decimal?) z.Amount * z.Quantity) ?? 0,
							Forms = x.EntranceTypeForms
								.Where(y =>
									y.EntranceTypeId == x.Id)
								.Count(),
							GroupsCount = serviceGroups
								.Where(y =>
									y.Category.ServiceConcessionId == x.Event.PaymentConcession.ConcessionId &&
									y.EntranceTypes
										.Any(z => z.EntranceTypeId == x.Id)
								)
								.Count()
						})
				})
				.ToList()
			.Select(a => new
				{
					Name = a.Name,
					EntranceTypes = a.EntranceTypes
						.Select(x => new EntranceTypeGetAllResult
						{
							Id = x.Id,
							Name = x.Name,
							Price = x.Price,
							MaxEntrance = x.MaxEntrance == int.MaxValue ? (int?)null : x.MaxEntrance,
							ExtraPrice = x.ExtraPrice,
							SellStart = x.SellStart.ToUTC(),
							SellEnd = x.SellEnd.ToUTC(),
							State = x.State,
							Visibility = x.Visibility,
							SellEntrances = x.SellEntrances,
							SellEntrancesPercent = (x.MaxEntrance == 0) ? 0 : (((decimal?)x.SellEntrances * 100) / (decimal?)x.MaxEntrance),
							IsVisible = x.IsVisible,
							TotalAmount = x.TotalAmount,
							TotalExtraPrice = x.TotalExtraPrice,
							Forms = x.Forms,
							GroupsCount = x.GroupsCount
						})
				}).FirstOrDefault();

			

				if (result == null)
				throw new ArgumentNullException("EventId");

			return new EntranceTypeGetAllResultBase
			{
				EventName = result.Name,
				Data = result.EntranceTypes
			};
        }
		#endregion ExecuteAsync
	}
}
