using PayIn.Application.Dto.Payments.Arguments.Promotion;
using PayIn.Application.Dto.Payments.Results.Promotion;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Payments.Controllers.Promotion
{
	[HideSwagger]
	[RoutePrefix("Api/Promotion")]
	public class PromotionController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route("")]
		public async Task<ResultBase<PromotionGetAllResult>> GetAll(
			[FromUri] PromotionGetAllArguments arguments,
			[Injection] IQueryBaseHandler<PromotionGetAllArguments, PromotionGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region GET /
		[HttpGet]
		[Route("GetAllConcession")]
		public async Task<ResultBase<PromotionGetAllConcessionResult>> GetAll2(
			[FromUri] PromotionGetAllConcessionArguments arguments,
			[Injection] IQueryBaseHandler<PromotionGetAllConcessionArguments, PromotionGetAllConcessionResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region GET /ViewCode/{id:int}
		[HttpGet]
		[Route("ViewCode/{id:int}")]
		public async Task<ResultBase<PromotionGetCodeResult>> Get(
			[FromUri] PromotionGetCodeArguments arguments,
			[Injection] IQueryBaseHandler<PromotionGetCodeArguments, PromotionGetCodeResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /ViewCode/{id:int}

		#region GET /AddCode/{id:int}
		[HttpPost]
		[Route("AddCode/{id:int}")]
		public async Task<dynamic> AddCode(
				PromotionAddCodeArguments command,
				[Injection] IServiceBaseHandler<PromotionAddCodeArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { id = item };
		}
		#endregion GET /ViewCode/{id:int}

		#region POST /
		[HttpPost]
		[Route("Create")]
		public async Task<dynamic> Post(
				PromotionCreateArguments command,
				[Injection] IServiceBaseHandler<PromotionCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
		#endregion POST /

		#region Delete /{id:int}
		[HttpDelete]
		[Route("{id:int}")]
		public async Task<dynamic> Post(
			[FromUri] PromotionDeleteArguments arguments,
			[Injection] IServiceBaseHandler<PromotionDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion Delete /{id:int}

		#region Unlinkcode /{id:int}
		[HttpDelete]
		[Route("UnlinkCode/{id:int}")]
		public async Task<dynamic> Delete(
			[FromUri] PromotionUnlinkCodeArguments arguments,
			[Injection] IServiceBaseHandler<PromotionUnlinkCodeArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion Unlinkcode /{id:int}

		#region GET /
		[HttpGet]
		[Route("Check")]
		public async Task<ResultBase<PromotionCheckResult>> Check(
			[FromUri] PromotionAsignArguments arguments,
			[Injection] IQueryBaseHandler<PromotionAsignArguments, PromotionCheckResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region PUT /{id:int}
		[HttpPut]
		[Route("{id:int}")]
		public async Task<dynamic> Put(
			PromotionUpdateArguments command,
			[Injection] IServiceBaseHandler<PromotionUpdateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
		#endregion PUT /{id:int}

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<PromotionGetResult>> Get(
			[FromUri] PromotionGetArguments query,
			[Injection] IQueryBaseHandler<PromotionGetArguments, PromotionGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(query);
			return result;
		}
		#endregion  GET /{id:int}


	}
}
