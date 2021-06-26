using PayIn.Application.Dto.Payments.Arguments.TicketTemplate;
using PayIn.Application.Dto.Payments.Results.TicketTemplate;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Payments.Controllers
{
	[HideSwagger]
	[RoutePrefix("Api/TicketTemplate")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Superadministrator
	)]
	public class TpvTicketTemplateController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route("")]
		public async Task<ResultBase<TicketTemplateGetAllResult>> GetAll(
			[FromUri] TicketTemplateGetAllArguments arguments,
			[Injection] IQueryBaseHandler<TicketTemplateGetAllArguments, TicketTemplateGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<TicketTemplateGetResult>> Get(
			[FromUri] TicketTemplateGetArguments arguments,
			[Injection] IQueryBaseHandler<TicketTemplateGetArguments, TicketTemplateGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{id:int}

		#region GET /Selector/{filter?}
		[HttpGet]
		[Route("Selector/{filter?}")]
		public async Task<ResultBase<TicketTemplateGetSelectorResult>> Selector(
			string filter,
			[FromUri] TicketTemplateGetSelectorArguments arguments,
			[Injection] IQueryBaseHandler<TicketTemplateGetSelectorArguments, TicketTemplateGetSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Selector/{filter?}

		#region GET /Details
		[HttpGet]
		[Route("Details/{id:int}")]
		public async Task<ResultBase<TicketTemplateGetDetailsResult>> GetDetails(
			[FromUri] TicketTemplateGetDetailsArguments arguments,
			[Injection] IQueryBaseHandler<TicketTemplateGetDetailsArguments, TicketTemplateGetDetailsResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion  GET /Details

		#region PUT /Check/{id:int}
		[HttpPut]
		[Route("Check/{id:int}")]
		public async Task<ResultBase<TicketTemplateCheckResult>> Check(
			int id,
			[FromBody] TicketTemplateCheckArguments arguments,
			[Injection] IQueryBaseHandler<TicketTemplateCheckArguments, TicketTemplateCheckResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion PUT /Check/{id:int}

		#region POST /
		[HttpPost]
		[Route] 
		public async Task<dynamic> Post(
			[FromBody]  TicketTemplateCreateArguments arguments,
			[Injection] IServiceBaseHandler<TicketTemplateCreateArguments> handler
		) 
		{			
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
        #endregion POST /

        #region PUT /{id:int}
        [HttpPut]
		[Route("{id:int}")]
		public async Task<dynamic> Update(
			int id,
			[FromBody] TicketTemplateUpdateArguments arguments,
			[Injection] IServiceBaseHandler<TicketTemplateUpdateArguments> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion PUT /{id:int}

		#region Delete /{id:int}
		[HttpDelete]
		[Route("{id:int}")]
		public async Task<dynamic> Delete(
			[FromUri] TicketTemplateDeleteArguments arguments,
			[Injection] IServiceBaseHandler<TicketTemplateDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion Delete /{id:int}
	}
}
