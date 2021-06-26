using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Attributes;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	[XpLog("Ticket", "Delete")]
	public class TicketMobileDeleteHandler :
		IServiceBaseHandler<TicketMobileDeleteArguments>
	{
		private readonly IEntityRepository<Ticket> _Repository;

		#region Constructors
		public TicketMobileDeleteHandler(
			IEntityRepository<PayIn.Domain.Payments.Ticket> repository
				)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;

		}
		#endregion Constructors

		#region TicketDelete
		async Task<dynamic> IServiceBaseHandler<TicketMobileDeleteArguments>.ExecuteAsync(TicketMobileDeleteArguments arguments)
		{
			var item = await _Repository.GetAsync(arguments.Id);

			item.State = TicketStateType.Cancelled;

			return null;
		}
		#endregion TicketDelete
	}
}
