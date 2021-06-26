using PayIn.Application.Dto.Payments.Arguments.Shipment;
using PayIn.Application.Dto.Payments.Results.Shipment;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ShipmentGetAddUserHandler :
		IQueryBaseHandler<ShipmentGetAddUserArguments, ShipmentGetAddUserResult>
	{
		private readonly IEntityRepository<PaymentUser> Repository;
		private readonly ISessionData SessionData;

		#region Constructors
		public ShipmentGetAddUserHandler(IEntityRepository<PaymentUser> repository, ISessionData sessionData)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			if (sessionData == null) throw new ArgumentNullException("sessionData");

			Repository = repository;
			SessionData = sessionData;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<ResultBase<ShipmentGetAddUserResult>> IQueryBaseHandler<ShipmentGetAddUserArguments, ShipmentGetAddUserResult>.ExecuteAsync(ShipmentGetAddUserArguments arguments)
		{
			var result = (await Repository.GetAsync())
				.Where(x => 
					((!x.Tickets
						.Select(y => y.ShipmentId)
						.Contains(arguments.Id) && 
							x.Concession.Concession.Supplier.Login == SessionData.Login && 
							x.State == Common.PaymentUserState.Active) || 
							x.Tickets.Where(y => y.State == Common.TicketStateType.Cancelled)
							.Select(z => z.ShipmentId)
							.Contains(arguments.Id) &&
							x.Concession.Concession.Supplier.Login == SessionData.Login &&
							x.State == Common.PaymentUserState.Active))
					
				.Select(x => new ShipmentGetAddUserResultBase.User
				{
					Id = x.Id,
					Login = x.Login,					
					Name = x.Name
				});

			return new ShipmentGetAddUserResultBase { Users = result };
		}
		#endregion ExecuteAsync
	}
}
