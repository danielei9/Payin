using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Common;
using PayIn.Domain.Public;
using PayIn.Domain.Transport;
using System.Collections.Generic;
using PayIn.Domain.Security;

namespace PayIn.Application.Public.Handlers
{
	public class TicketGetAllHandler :
		IQueryBaseHandler<TicketGetAllArguments, TicketGetAllResult>
	{
		[Dependency] public IEntityRepository<Ticket> TicketRepository { get; set; }
		[Dependency] public IEntityRepository<PaymentConcession> PaymentConcessionRepository { get; set; }
		[Dependency] public IEntityRepository<SystemCard> SystemCardRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceConcession> ServiceConcessionRepository { get; set; }
		[Dependency] public IEntityRepository<ServiceCard> ServiceCardRepository { get; set; }
		[Dependency] public IEntityRepository<TransportOperation> TransportOperationRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<TicketGetAllResult>> ExecuteAsync(TicketGetAllArguments arguments)
		{
			if (arguments.Since.Value > arguments.Until.Value)
				return new ResultBase<TicketGetAllResult>();

			var since = arguments.Since ?? new XpDate(XpDate.MinValue);
			var until = arguments.Until.AddDays(1);

			var serviceConcessions = (await ServiceConcessionRepository.GetAsync());

			var myOwnedSystemCards = (await SystemCardRepository.GetAsync())
				.Where(x =>
					x.ConcessionOwner.Supplier.Login == SessionData.Login
				);
			var myConcessions = (await PaymentConcessionRepository.GetAsync())
				.Where(x =>
					x.Concession.Type == ServiceType.Charge &&
					x.Concession.State == ConcessionState.Active &&
					(
						x.Concession.Supplier.Login == SessionData.Login ||
						x.PaymentWorkers.Any(y => y.Login == SessionData.Login) ||
						myOwnedSystemCards
							.Any(y =>
								y.SystemCardMembers.Any(z => z.Login == x.Concession.Supplier.Login)
							)
					)
				);

			var items = (await TicketRepository.GetAsync());

            if (arguments.EventId > 0)
                items = items.Where(x => (
                    x.EventId == arguments.EventId
                ));

			items = items
				.Where(x =>
					x.ShipmentId == null && 
					x.Date < until && 
					x.Date >= since &&
					(
						myConcessions.Any(y => x.LiquidationConcessionId == y.Id) ||
						myConcessions.Any(y => x.ConcessionId == y.Id) ||
						// Usuario que ha pagado
						(x.Payments.Any(y => y.UserLogin == SessionData.Login)) ||
						// Superadministrador
						(SessionData.Roles.Contains(AccountRoles.Superadministrator))
					)
				);

			var serviceCardRepository = (await ServiceCardRepository.GetAsync()); // Se han de devolver tb las eliminadas
			var transportOperation = await TransportOperationRepository.GetAsync();

			var items2 = items
				.Select(x => new
				{
					x.Id,
					Amount = x.Lines.Sum(y => (decimal?)y.Amount) ?? 0,
					x.State,
					x.Type,
					x.Lines,
					x.Payments,
					Uids =
						x.Lines
						.Where( y => y.Uid != null)
						.Select(y => new
						{
							y.Uid,
							UidText = serviceCardRepository
								.Where(z => z.Uid == y.Uid)
								.Select(z => z.UidText)
								.FirstOrDefault() ?? y.Uid.ToString() ?? ""
						})
						.Union(
							x.Payments
							.Where(y => y.Uid != null)
							.Select(y => new
							{
								y.Uid,
								UidText = serviceCardRepository
									.Where(z => z.Uid == y.Uid)
									.Select(z => z.UidText)
									.FirstOrDefault() ?? y.Uid.ToString() ?? ""
							})
						),
					x.Lines.FirstOrDefault().Title,
					x.Date,
					PayedAmount = x.Payments.Where(y => y.State == PaymentState.Active).Sum(y => (decimal?)y.Amount) ?? 0,
					HasShipment = x.ShipmentId != null,
					TemplateId = x.Concession.TicketTemplate.Concessions.Count() == 1 ? x.Concession.TicketTemplate.Id : (int?)null, // Si hay más de uno significa que no es propio y no se puede modificar
					HasText = (x.TextUrl != ""),
					EventName = x.Event.Name,
					ConcessionName = x.TaxName
				});

			var items3 = items2
				.Select(x => new
				{
					x.Id,
					x.Amount,
					x.State,
					x.Type,
					UidsText = x.Uids
						.Select(y => y.UidText),
					x.Title,
					x.Date,
					x.PayedAmount,
					x.HasShipment,
					x.TemplateId,
					x.HasText,
					x.EventName,
					x.ConcessionName,
					PaymentUserName = x.Uids
						.SelectMany(y => serviceCardRepository
							.Where(z => z.Uid == y.Uid)
							.Select(z => (z.OwnerUser.Name + " " + z.OwnerUser.LastName).Trim())
							.Where(z => z != "")
						)
						.FirstOrDefault() ?? ""
				});

			var items4 = items3
				.ToList();

			var items5 = items4
				.Select(x => new
				{
					x.Id,
					x.Amount,
					x.State,
					x.Type,
					UidsText = x.UidsText
						.Where(y => y != null && y != "")
						.Select(y => y)
						.JoinString(","),
					x.Title,
					x.Date,
					x.PayedAmount,
					x.HasShipment,
					x.TemplateId,
					x.HasText,
					x.EventName,
					x.ConcessionName,
					x.PaymentUserName
				});

            if (!arguments.Filter.IsNullOrEmpty())
                items5 = items5
                   .Where(x =>
                        x.Id.ToString().Contains(arguments.Filter) ||
                        //x.Uid.ToString().Contains(arguments.Filter) ||
                        x.UidsText.Contains(arguments.Filter) ||
                        x.Title.Contains(arguments.Filter) ||
                        x.EventName.Contains(arguments.Filter) ||
                        x.ConcessionName.Contains(arguments.Filter)
                    );

            var result = items5
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
                        //Uid = x.Uid,
                        UidsText = x.UidsText ?? "",
                        PayedAmount = x.PayedAmount,
                        State = x.State,
                        Title = x.Title,
                        HasShipment = x.HasShipment,
                        TemplateId = x.TemplateId,
                        HasText = x.HasText,
                        EventName = x.EventName,
                        ConcessionName = x.ConcessionName,
						PaymentUserName = x.PaymentUserName
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
