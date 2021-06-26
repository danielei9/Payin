using PayIn.Application.Dto.Payments.Arguments.Shipment;
using PayIn.Common;
using PayIn.Common.Resources;
using PayIn.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
	public class ShipmentDeleteHandler :
		IServiceBaseHandler<ShipmentDeleteArguments>
	{
		private readonly IEntityRepository<Shipment> Repository;
		private readonly IEntityRepository<Ticket> TicketRepository;


		#region Constructors
		public ShipmentDeleteHandler(IEntityRepository<Shipment> repository, IEntityRepository<Ticket> ticketrepository)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (ticketrepository == null) throw new ArgumentNullException("ticketrepository");

			Repository = repository;
			TicketRepository = ticketrepository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ShipmentDeleteArguments>.ExecuteAsync(ShipmentDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);

			foreach (var ticket in item.Tickets) {
				ticket.State = TicketStateType.Cancelled;
			}

			var now = DateTime.Now;

			if (item.Since <= now)
				throw new Exception(ShipmentResources.DeleteException);

			if(item.Since > now)
			item.State = Common.ShipmentState.Deleted;

			return null;
		}
		#endregion ExecuteAsync
	}
}
