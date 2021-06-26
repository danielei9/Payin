using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Arguments.Purse;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Application.Dto.Payments.Results.Purse;
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
	[RoutePrefix("Api/Purse")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker
	)]
	public class PurseController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route("")]
		public async Task<ResultBase<PurseGetAllResult>> GetAll(
			[FromUri] PurseGetAllArguments arguments,
			[Injection] IQueryBaseHandler<PurseGetAllArguments, PurseGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /
		
		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<PurseGetResult>> Get(
			[FromUri] PurseGetArguments query,
			[Injection] IQueryBaseHandler<PurseGetArguments, PurseGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(query);
			return result;
		}
		#endregion  GET /{id:int}

		#region GET /Users/{id:int}
		[HttpGet]
		[Route("Users/{id:int}")]
		public async Task<ResultBase<PurseGetUsersResult>> Get2(
			[FromUri] PurseGetUsersArguments query,
			[Injection] IQueryBaseHandler<PurseGetUsersArguments, PurseGetUsersResult> handler
		)
		{
			var result = await handler.ExecuteAsync(query);
			return result;
		}
		#endregion  GET /Users/{id:int}

		#region GET /BySystemCardRetrieveSelector
		[HttpGet]
		[Route("BySystemCardRetrieveSelector")]
		public async Task<ResultBase<MobileEntranceTypeGetPursesResult>> GetPursesBySystemCard(
			[FromUri] MobileEntranceTypeGetPursesArguments arguments,
			[Injection] IQueryBaseHandler<MobileEntranceTypeGetPursesArguments, MobileEntranceTypeGetPursesResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion  GET /BySystemCardRetrieveSelector

		#region PUT /{id:int}
		[HttpPut]
		[Route("{id:int}")]
		public async Task<dynamic> Put(
			PurseUpdateArguments command,
			[Injection] IServiceBaseHandler<PurseUpdateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
		#endregion PUT /{id:int}
		
		#region POST /
		[HttpPost]
		[Route("")]
		public async Task<dynamic> Post(
			PurseCreateArguments arguments,
			[Injection] IServiceBaseHandler<PurseCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
        #endregion POST /

        #region CreateImage /{id:int}
        [HttpPut]
        [Route("CreateImage/{id:int}")]
        public async Task<dynamic> CreateImage(
            PurseImageCreateArguments arguments,
            [Injection] IServiceBaseHandler<PurseImageCreateArguments> handler
        )
        {
            var item = await handler.ExecuteAsync(arguments);
            return new { item.Id };
        }
        #endregion CreateImage /{id:int}

        #region POST /ImageCrop		
        [HttpPut]
		[Route("ImageCrop/{id:int}")]
		public async Task <dynamic>PurseImage(PurseUpdatePhotoArguments arguments, [Injection] IServiceBaseHandler<PurseUpdatePhotoArguments>handler)
		{			
			 var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST api/Account/ImageCrop
		
		#region Delete /{id:int}
		[HttpDelete]
		[Route("{id:int}")]
		public async Task<dynamic> Post(
			[FromUri] PurseDeleteArguments arguments,
			[Injection] IServiceBaseHandler<PurseDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion Delete /{id:int}

		#region POST DeleteImage		
		[HttpDelete]
		[Route("DeleteImage/{id:int}")]
		public async Task<dynamic> DeleteImage([FromUri] PurseImageDeleteArguments arguments,
			[Injection] IServiceBaseHandler<PurseImageDeleteArguments> handler)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST api/Account/DeleteImage

	}
}
