using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Payments.Controllers.Api
{
	[HideSwagger]
	[RoutePrefix("Api/Campaign")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker + "," + AccountRoles.User
	)]
	public class ApiCampaignController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route("")]
		public async Task<ResultBase<ApiCampaignGetAllResult>> GetAll(
			[FromUri] ApiCampaignGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ApiCampaignGetAllArguments, ApiCampaignGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region GET /AddOwner
		[HttpGet]
		[Route("AddOwners")]
		public async Task<ResultBase<ApiCampaignGetAddOwnersResult>> GetAddOwners(
			[FromUri] ApiCampaignGetAddOwnersArguments query,
			[Injection] IQueryBaseHandler<ApiCampaignGetAddOwnersArguments, ApiCampaignGetAddOwnersResult> handler
		)
		{
			var result = await handler.ExecuteAsync(query);
			return result;
		}
		#endregion  GET /AddOwner

		#region GET /AddOwner/{id:int}
		[HttpGet]
		[Route("AddOwners/{id:int}")]
		public async Task<ResultBase<ApiCampaignGetAddOwnersResult>> GetAddOwners2(
			[FromUri] ApiCampaignGetAddOwnersArguments query,
			[Injection] IQueryBaseHandler<ApiCampaignGetAddOwnersArguments, ApiCampaignGetAddOwnersResult> handler
		)
		{
			var result = await handler.ExecuteAsync(query);
			return result;
		}
		#endregion  GET /AddOwner/{id:int}

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<ApiCampaignGetResult>> Get(
			[FromUri] ApiCampaignGetArguments query,
			[Injection] IQueryBaseHandler<ApiCampaignGetArguments, ApiCampaignGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(query);
			return result;
		}
		#endregion GET /{id:int}

		#region GET /Create
		[HttpGet]
		[Route("Create")]
		public async Task<ResultBase<ApiCampaignGetCreateResult>> Create(
			[FromUri] ApiCampaignGetCreateArguments argument,
			[Injection] IQueryBaseHandler<ApiCampaignGetCreateArguments, ApiCampaignGetCreateResult> handler
		)
		{
			var result = await handler.ExecuteAsync(argument);
			return result;
		}
		#endregion GET /Create

		#region POST /
		[HttpPost]
		[Route("")]		
		public async Task<dynamic> Post(
			ApiCampaignCreateArguments arguments,
			[Injection] IServiceBaseHandler<ApiCampaignCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /

		#region PUT /{id:int}
		[HttpPut]
		[Route("{id:int}")]		
		public async Task<dynamic> Put(
			ApiCampaignUpdateArguments command,
			[Injection] IServiceBaseHandler<ApiCampaignUpdateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
		#endregion PUT /{id:int}

		#region PUT /Suspend/{id:int}
		[HttpPut]
		[Route("Suspend/{id:int}")]
		public async Task<dynamic> Suspend(
			int id,
			ApiCampaignSuspendArguments arguments,
			[Injection] IServiceBaseHandler<ApiCampaignSuspendArguments> handler
		)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /Suspend/{id:int}

		#region PUT /Unsuspend/{id:int}
		[HttpPut]
		[Route("Unsuspend/{id:int}")]
		public async Task<dynamic> Unsuspend(
			int id,
			ApiCampaignUnsuspendArguments arguments,
			[Injection] IServiceBaseHandler<ApiCampaignUnsuspendArguments> handler
		)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /Unsuspend/{id:int}

		#region POST /ImageCrop		
		[HttpPut]
		[Route("ImageCrop/{id:int}")]
		public async Task<dynamic> CampaignImage(
			int id,
			ApiCampaignUpdatePhotoArguments arguments,
			[Injection] IServiceBaseHandler<ApiCampaignUpdatePhotoArguments> handler)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /ImageCrop

		#region DELETE /{id:int}
		[HttpDelete]
		[Route("{id:int}")]		
		public async Task<dynamic> Delete(
			[FromUri] ApiCampaignDeleteArguments arguments,
			[Injection] IServiceBaseHandler<ApiCampaignDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /{id:int}

		#region GET /Event/{campaignId:int}
		[HttpGet]
        [Route("Event/{campaignId:int}")]
        public async Task<ResultBase<ApiCampaignGetEventResult>> Event(
			int campaignId,
            [FromUri] ApiCampaignGetEventArguments arguments,
            [Injection] IQueryBaseHandler<ApiCampaignGetEventArguments, ApiCampaignGetEventResult> handler
        )
        {
			arguments.CampaignId = campaignId;
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
		#endregion GET /Event/{campaignId:int}

		#region POST /AddEvent/{id:int}
		[HttpPost]
        [Route("AddEvent/{id:int}")]
        public async Task<dynamic> AddEvent(
            int id,
            CampaignAddEventArguments arguments,
            [Injection] IServiceBaseHandler<CampaignAddEventArguments> handler = null
        )
        {
            arguments.Id = id;
            var item = await handler.ExecuteAsync(arguments);
            return new { item.Id };
        }
        #endregion POST /AddEvent/{id:int}

        #region GET /EventRetrieveSelector/{filter?}
        [HttpGet]
        [Route("EventRetrieveSelector/{filter?}")]
        public async Task<ResultBase<CampaignEventGetSelectorResult>> Selector(
            string filter,
            [FromUri] CampaignEventGetSelectorArguments arguments,
            [Injection] IQueryBaseHandler<CampaignEventGetSelectorArguments, CampaignEventGetSelectorResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /EventRetrieveSelector/{filter?}

        #region DELETE /RemoveEvent/{id:int}
        [HttpPut]
        [Route("RemoveEvent/{id:int}")]
        public async Task<dynamic> Remove(
			[FromBody] CampaignRemoveEventArguments arguments,
            [Injection] IServiceBaseHandler<CampaignRemoveEventArguments> handler,
            int id
		)
        {
            arguments.EventId = id;
            var result = await handler.ExecuteAsync(arguments);
            return null;
        }
        #endregion DELETE /RemoveEvent/{id:int}
    }
}
