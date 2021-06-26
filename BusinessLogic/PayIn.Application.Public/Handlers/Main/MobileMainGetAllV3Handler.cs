using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments.Main;
using PayIn.Application.Dto.Results.Main;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers.Main
{
	public class MobileMainGetAllV3Handler : 
		IQueryBaseHandler<MainMobileGetAllV3Arguments, MainMobileGetAllV3Result>
	{
		[Dependency] public ISessionData                           SessionData                   { get; set; }	
		[Dependency] public IEntityRepository<Ticket>              TicketRepository              { get; set; }
		
		#region ExecuteAsync
		public async Task<ResultBase<MainMobileGetAllV3Result>> ExecuteAsync(MainMobileGetAllV3Arguments arguments)
        {
			var tickets = (await TicketRepository.GetAsync("Payments","Concession.Concession"))
				.Where(x => x.Payments.Where(y => y.UserLogin == SessionData.Login).Count() >= 1 && x.State != TicketStateType.Cancelled)
				.OrderByDescending(x=> x.Date)
				.Take(5);

			var result = tickets
				.Select(x => new
				{
					Id = x.Id,
					Date = x.Date,
					Amount = x.Amount,
					Reference = x.Reference,
					Name = x.Concession.Concession.Name,
					Type = x.Type
				})
				.ToList()
				.Select(x => new MainMobileGetAllV3Result
				{
					Id  = x.Id,
					Day = x.Date.Day,
					Month = x.Date.Month,
					Amount = x.Amount,
					Reference = x.Reference,
					ConcessionName = x.Name,
					Type = x.Type
				});

			return new ResultBase<MainMobileGetAllV3Result> { Data = result };
		}
		#endregion ExecuteAsync
	}
}
