using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class TicketDeleteHandler :
		IServiceBaseHandler<TicketDeleteArguments>
	{
		[Dependency] public IEntityRepository<Ticket> Repository { get; set; }

		#region TicketDelete
		public async Task<dynamic> ExecuteAsync(TicketDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);
			item.State = TicketStateType.Cancelled;

			return null;
		}
		#endregion TicketDelete
	}
}
