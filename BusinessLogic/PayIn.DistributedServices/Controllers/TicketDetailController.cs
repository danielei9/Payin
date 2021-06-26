//using PayIn.Application.Dto.Arguments.TicketDetail;
//using PayIn.Application.Dto.Results.TicketDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers
{
	public class TicketDetailController : ApiController
	{
		//#region GET /
		//[HttpGet]
		//public async Task<ResultBase<TicketDetailGetResult>> Get(
		//	[FromUri] TicketDetailGetArguments arguments,
		//	[Injection] IQueryBaseHandler<TicketDetailGetArguments, TicketDetailGetResult> handler
		//)
		//{
		//	var result = await handler.ExecuteAsync(arguments);
		//	return result;
		//}
		//#endregion GET /

		//#region PUT /
		//[HttpPut]
		//public async Task<dynamic> Put(
		//		int id,
		//		TicketDetailUpdateArguments arguments,
		//		[Injection] IServiceBaseHandler<TicketDetailUpdateArguments> handler
		//)
		//{
		//	arguments.Id = id;
		//	var item = await handler.ExecuteAsync(arguments);
		//	return new { Id = item.Id };
		//}
		//#endregion PUT /
	}
}
