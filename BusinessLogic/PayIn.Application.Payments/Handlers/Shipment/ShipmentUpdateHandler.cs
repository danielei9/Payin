using PayIn.Application.Dto.Payments.Arguments.Shipment;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ShipmentUpdateHandler :
		IServiceBaseHandler<ShipmentUpdateArguments>
	{
		private readonly IEntityRepository<Shipment> Repository;		
		private readonly IUnitOfWork UnitOfWork;
		private readonly IEntityRepository<Ticket> TicketRepository;

		#region Constructors
		public ShipmentUpdateHandler(
			IUnitOfWork unitOfWork,
			IEntityRepository<Shipment> repository,
			IEntityRepository<Ticket> ticketRepository
		)
		{
			if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");
			if (repository == null)	throw new ArgumentNullException("repository");
			if (ticketRepository == null) throw new ArgumentNullException("ticketRepository");
			
			UnitOfWork = unitOfWork;
			Repository = repository;
			TicketRepository = ticketRepository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ShipmentUpdateArguments>.ExecuteAsync(ShipmentUpdateArguments arguments)
		{
			var now = DateTime.Now;
			var item = await Repository.GetAsync(arguments.Id, "Tickets.Lines");			

			if (item.Until < now)
				throw new Exception(ShipmentResources.ShipmentExpiredException);
			else if (item.Since < now && item.Until <= now)
			{
				foreach (var ticket in item.Tickets)
				{
					foreach (var line in ticket.Lines)
					{
						line.Amount = arguments.Amount;
						line.Title = arguments.Name;
					}

					ticket.Until = arguments.Until.Value.ToUTC();
					ticket.Reference = arguments.Name;
					ticket.Amount = ticket.Lines.Sum(x => (decimal?) x.Amount) ?? 0;
				}

				item.Until = arguments.Until;
				item.Name = arguments.Name;
				item.Amount = arguments.Amount;
				return item;
			}
			else if (item.Until > arguments.Until)
				throw new Exception(ShipmentResources.ShipmentChangeUntilException);
			else 
			{
				foreach (var ticket in item.Tickets)
				{
					foreach (var line in ticket.Lines)
					{
						line.Amount = arguments.Amount;
						line.Title = arguments.Name;
					}					
					
					ticket.Until = arguments.Until.Value.ToUTC();
					ticket.Reference = arguments.Name;
					ticket.Amount = ticket.Lines.Sum(x => (decimal?)x.Amount) ?? 0;
				}
				
				item.Until = arguments.Until.Value.ToUTC();
				item.Name = arguments.Name;
				item.Amount = arguments.Amount;
				return item;
			}
		}
		#endregion ExecuteAsync
	}
}
