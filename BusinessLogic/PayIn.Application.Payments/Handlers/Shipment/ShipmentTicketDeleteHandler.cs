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
	public class ShipmentTicketDeleteHandler :
		IServiceBaseHandler<ShipmentTicketDeleteArguments>
	{
		private readonly IEntityRepository<Ticket> Repository;


		#region Constructors
		public ShipmentTicketDeleteHandler(IEntityRepository<Ticket> repository)
		{
			
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
			

		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ShipmentTicketDeleteArguments>.ExecuteAsync(ShipmentTicketDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);

			item.State = TicketStateType.Cancelled;
			

			return null;
		}
		#endregion ExecuteAsync
	}
}
