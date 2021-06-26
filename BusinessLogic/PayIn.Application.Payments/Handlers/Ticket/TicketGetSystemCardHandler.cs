using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using PayIn.Domain.Public;
using PayIn.Domain.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using System.Collections.Generic;

namespace PayIn.Application.Public.Handlers
{
	public class TicketGetSystemCardHandler :
		IQueryBaseHandler<TicketGetSystemCardArguments, TicketGetAllResult>
	{
		[Dependency] public IEntityRepository<Ticket> Repository { get; set; }
        [Dependency] public IEntityRepository<SystemCardMember> SystemCardMemberRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceCard> ServiceCardRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

        private string hexArray = "0123456789ABCDEF";

        #region ExecuteAsync
        public async Task<ResultBase<TicketGetAllResult>> ExecuteAsync(TicketGetSystemCardArguments arguments)
		{
			if (arguments.Since.Value > arguments.Until.Value)
				return new ResultBase<TicketGetAllResult>();

			var since = arguments.Since;
			var until = arguments.Until.AddDays(1);

            var systemCardMembers = (await SystemCardMemberRepository.GetAsync());
			var serviceCardRepository = await ServiceCardRepository.GetAsync();

			var items = (await Repository.GetAsync())
				.Where(x =>
					x.ShipmentId == null && 
					x.Date < until && 
					x.Date >= since &&
					(
						// Tickets que yo como empresa liquido
						(
                            systemCardMembers
                                .Any(y =>
                                    y.Login == x.Concession.Concession.Supplier.Login &&
                                    y.SystemCard.ConcessionOwner.Supplier.Login == SessionData.Login
                                )
                        ) ||
						// Superadministrador
						(SessionData.Roles.Contains(AccountRoles.Superadministrator))
					) 
				);

			if (arguments.EventId > 0)
				items = items.Where(x => (
					x.EventId == arguments.EventId
				));

			var items2 = items
				.Select(x => new
				{
					Id = x.Id,
					Amount = x.Lines.Sum(y => (decimal?)y.Amount) ?? 0,
					State = x.State,
					Type = x.Type,
					//Uid = x.Payments.FirstOrDefault().Uid,
     //               UidFormat = x.Payments.FirstOrDefault().UidFormat,
					Uids =
						x.Lines.Select(y => new
						{
							y.Uid,
							UidFormat = serviceCardRepository
								.Where(
									z => z.Uid == y.Uid
								)
								.Select(z => z.ServiceCardBatch.UidFormat)
								.FirstOrDefault(),
						})
						.Union(
							x.Payments.Select(y => new
							{
								y.Uid,
								UidFormat = serviceCardRepository
								.Where(
									z => z.Uid == y.Uid
								)
								.Select(z => z.ServiceCardBatch.UidFormat)
								.FirstOrDefault(),
							})
						),
					//UidFormat = serviceCardRepository
					//	.Where(
					//		y => y.Uid == x.Lines.FirstOrDefault().Uid
					//	)
					//	.Select(y => y.ServiceCardBatch.UidFormat)
					//	.FirstOrDefault(),
					Title = x.Lines.FirstOrDefault().Title,
					Date = x.Date,
					PayedAmount = x.Payments.Where(y => y.State == PaymentState.Active).Sum(y => (decimal?)y.Amount) ?? 0,
					HasShipment = x.ShipmentId != null,
					TemplateId = x.Concession.TicketTemplate.Concessions.Count() == 1 ? x.Concession.TicketTemplate.Id : (int?)null, // Si hay más de uno significa que no es propio y no se puede modificar
					HasText = (x.TextUrl != ""),
					EventName = x.Event.Name,
                    ConcessionName = x.TaxName
                })
                .Select(x => new
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    State = x.State,
                    Type = x.Type,
					UidsText = x.Uids
						.Select(y =>
							y.UidFormat == UidFormat.BigEndian ?
								(
									hexArray.Substring(((int)y.Uid / 268435456) % 16, 1) +
									hexArray.Substring(((int)y.Uid / 16777216) % 16, 1) +
									hexArray.Substring(((int)y.Uid / 1048576) % 16, 1) +
									hexArray.Substring(((int)y.Uid / 65536) % 16, 1) +
									hexArray.Substring(((int)y.Uid / 4096) % 16, 1) +
									hexArray.Substring(((int)y.Uid / 256) % 16, 1) +
									hexArray.Substring(((int)y.Uid / 16) % 16, 1) +
									hexArray.Substring(((int)y.Uid / 1) % 16, 1)
								).ToString() :
							y.UidFormat == UidFormat.LittleEndian ?
								(
									hexArray.Substring(((int)y.Uid / 16) % 16, 1) +
									hexArray.Substring(((int)y.Uid / 1) % 16, 1) +
									hexArray.Substring(((int)y.Uid / 4096) % 16, 1) +
									hexArray.Substring(((int)y.Uid / 256) % 16, 1) +
									hexArray.Substring(((int)y.Uid / 1048576) % 16, 1) +
									hexArray.Substring(((int)y.Uid / 65536) % 16, 1) +
									hexArray.Substring(((int)y.Uid / 268435456) % 16, 1) +
									hexArray.Substring(((int)y.Uid / 16777216) % 16, 1)
								).ToString() :
							y.ToString()
						),
					Title = x.Title,
                    Date = x.Date,
                    PayedAmount = x.PayedAmount,
                    HasShipment = x.HasShipment,
                    TemplateId = x.TemplateId,
                    HasText = x.HasText,
                    EventName = x.EventName,
                    ConcessionName = x.ConcessionName
                })
				.ToList();

			var items3 = items2
				.Select(x => new
				{
					x.Id,
					x.Amount,
					x.State,
					x.Type,
					//Uids = x.Uids,
					UidsText = x.UidsText.Select(y => y).JoinString(","),
					x.Title,
					x.Date,
					x.PayedAmount,
					x.HasShipment,
					x.TemplateId,
					x.HasText,
					x.EventName,
					x.ConcessionName
				});

			if (!arguments.Filter.IsNullOrEmpty())
                items3 = items3
                   .Where(x => 
                        x.Id.ToString().Contains(arguments.Filter) ||
                        x.UidsText.Contains(arguments.Filter) ||
                        x.Title.Contains(arguments.Filter) ||
                        x.EventName.Contains(arguments.Filter) ||
                        x.ConcessionName.Contains(arguments.Filter)
                    );

            var result = items3
                .OrderByDescending(x => x.Date)
                .ToList();

            var totalCharges = result
                .Where(x => x.Type == TicketType.Ticket)
				.Sum(x => (decimal?)x.Amount) ?? 0;
            var totalPaidCharges = result
                .Where(x => x.Type == TicketType.Ticket)
                .Sum(x => (decimal?)x.PayedAmount) ?? 0;
            var totalRecharges = result
                .Where(x => x.Type == TicketType.Recharge)
                .Sum(x => (decimal?)x.Amount) ?? 0;
            var totalPaidRecharges = result
                .Where(x => x.Type == TicketType.Recharge)
                .Sum(x => (decimal?)x.PayedAmount) ?? 0;
            var totalOrders = result
                .Where(x => x.Type == TicketType.Order)
                .Sum(x => (decimal?)x.Amount) ?? 0;
            var totalPaidOrders = result
                .Where(x => x.Type == TicketType.Order)
                .Sum(x => (decimal?)x.PayedAmount) ?? 0;
            var totalShipments = result
                .Where(x => x.Type == TicketType.Shipment)
                .Sum(x => (decimal?)x.Amount) ?? 0;
            var totalPaidShipments = result
                .Where(x => x.Type == TicketType.Shipment)
                .Sum(x => (decimal?)x.PayedAmount) ?? 0;
            var totalAmount = result
                .Sum(x => (decimal?)x.Amount) ?? 0;
            var totalPaidAmount = result
                .Sum(x => (decimal?)x.PayedAmount) ?? 0;
            var totalOthers = totalAmount - totalCharges - totalRecharges - totalOrders - totalShipments;
            var totalPaidOthers = totalPaidAmount - totalPaidCharges - totalPaidRecharges - totalPaidOrders - totalPaidShipments;

			return new TicketGetAllResultBase
			{
				Data = result
					.Select(x => new TicketGetAllResult
					{
						Id = x.Id,
						Amount = x.Amount,
						Date = x.Date.ToUTC(), // Needs to be calculated in memory
						UidsText = x.UidsText ?? "",
                        PayedAmount = x.PayedAmount,
                        State = x.State,
                        Title = x.Title,
                        HasShipment = x.HasShipment,
                        TemplateId = x.TemplateId,
                        HasText = x.HasText,
                        EventName = x.EventName,
                        ConcessionName = x.ConcessionName
                    }),
				TotalCharges = totalCharges,
                TotalPaidCharges = totalPaidCharges,
                TotalRecharges = totalRecharges,
                TotalPaidRecharges = totalPaidRecharges,
                TotalOrders = totalOrders,
                TotalPaidOrders = totalPaidOrders,
                TotalShipments = totalShipments,
                TotalPaidShipments = totalPaidShipments,
                TotalOthers = totalOthers,
                TotalPaidOthers = totalPaidOthers,
                TotalAmount = totalAmount,
                TotalPaidAmount = totalPaidAmount
            };
		}
		#endregion ExecuteAsync
	}
 }
