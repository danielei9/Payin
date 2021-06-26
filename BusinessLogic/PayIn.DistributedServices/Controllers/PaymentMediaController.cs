using PayIn.Application.Dto.Arguments.PaymentMedia;
using PayIn.Application.Dto.Queries;
using PayIn.Application.Dto.Results;
using PayIn.Application.Dto.Results.PaymentMedia;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers
{
	public class PaymentMediaController : ApiController
	{
		//#region GET /
		//public async Task<ResultBase<PaymentMediaGetAllResult>> Get(
		//	[FromUri] PaymentMediaGetAllQuery command,
		//	[Injection] IQueryBaseHandler<PaymentMediaGetAllQuery, PaymentMediaGetAllResult> handler
		//)
		//{
		//				var result = await handler.ExecuteAsync(command);
		//				return result;
		//}
		//#endregion GET /

		//#region POST /
		//public async Task<dynamic> Post(
		//	PaymentMediaCreateArguments command,
		//	[Injection] IServiceBaseHandler<PaymentMediaCreateArguments> handler
		//)
		//{
		//				var item = await handler.ExecuteAsync(command);
		//				return new { Id = item.Id };
		//}
		//#endregion POST /

		//#region Delete /
		//public async Task<dynamic> Delete(
		//	int id,
		//	[FromUri] PaymentMediaDeleteArguments arguments,
		//	[Injection] IServiceBaseHandler<PaymentMediaDeleteArguments> handler
		//)
		//{
		//				arguments.Id = id;

		//				var item = await handler.ExecuteAsync(arguments);
		//				return null;
		//}
		//#endregion Delete /

		//#region GET /RetrieveSelector
		//[HttpGet]
		//public async Task<ResultBase<PaymentMediaGetSelectorResult>> RetrieveSelector(
		//	[FromUri] PaymentMediaGetSelectorArguments command,
		//	[Injection] IQueryBaseHandler<PaymentMediaGetSelectorArguments, PaymentMediaGetSelectorResult> handler
		//)
		//{
		//				var result = await handler.ExecuteAsync(command);
		//				return result;
		//}
		//#endregion GET /RetrieveSelector
	}
}
