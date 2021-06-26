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
	[RoutePrefix("Api/Notice")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
	)]
	public class ApiNoticeController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route]
		public async Task<ResultBase<ApiNoticeGetAllResult>> GetAll(
			[FromUri] ApiNoticeGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ApiNoticeGetAllArguments, ApiNoticeGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
        #endregion GET /

        #region GET /Pages
        [HttpGet]
        [Route("Pages")]
        public async Task<ResultBase<ApiNoticeGetPagesResult>> GetPages(
            [FromUri] ApiNoticeGetPagesArguments arguments,
            [Injection] IQueryBaseHandler<ApiNoticeGetPagesArguments, ApiNoticeGetPagesResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /Pages

        #region GET /Edicts
        [HttpGet]
        [Route("Edicts")]
        public async Task<ResultBase<ApiNoticeGetEdictsResult>> GetEdicts(
            [FromUri] ApiNoticeGetEdictsArguments arguments,
            [Injection] IQueryBaseHandler<ApiNoticeGetEdictsArguments, ApiNoticeGetEdictsResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /Edicts

        #region GET /{id:int}
        [HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<NoticeGetResult>> Get(
			int id,
			[FromUri] NoticeGetArguments arguments,
			[Injection] IQueryBaseHandler<NoticeGetArguments, NoticeGetResult> handler)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{id:int}

		#region PUT /{id:int}
		[HttpPut]
		[Route("{id:int}")]
		public async Task<dynamic> Put(
			int id,
			[FromBody] ApiNoticeUpdateArguments arguments,
			[Injection] IServiceBaseHandler<ApiNoticeUpdateArguments> handler)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /{id:int}

		#region PUT /Page/{id:int}
		[HttpPut]
		[Route("Page/{id:int}")]
		public async Task<dynamic> PutPage(
			int id,
			[FromBody] ApiNoticeUpdatePageArguments arguments,
			[Injection] IServiceBaseHandler<ApiNoticeUpdatePageArguments> handler)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /Page/{id:int}

		#region PUT /Edict/{id:int}
		[HttpPut]
		[Route("Edict/{id:int}")]
		public async Task<dynamic> PutEdict(
			int id,
			[FromBody] ApiNoticeUpdateEdictArguments arguments,
			[Injection] IServiceBaseHandler<ApiNoticeUpdateEdictArguments> handler)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /Edict/{id:int}

		#region POST /
		[HttpPost]
		[Route]
		public async Task<dynamic> Post(
			ApiNoticeCreateArguments arguments,
			[Injection] IServiceBaseHandler<ApiNoticeCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /

		#region POST /Route
		[HttpPost]
		[Route("Route")]
		public async Task<dynamic> PostRoute(
			ApiNoticeCreatePageArguments arguments,
			[Injection] IServiceBaseHandler<ApiNoticeCreatePageArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /Page

		#region POST /Page
		[HttpPost]
        [Route("Page")]
        public async Task<dynamic> PostPage(
            ApiNoticeCreatePageArguments arguments,
            [Injection] IServiceBaseHandler<ApiNoticeCreatePageArguments> handler
        )
        {
            var item = await handler.ExecuteAsync(arguments);
            return new { item.Id };
        }
        #endregion POST /Page

        #region POST /Edict
        [HttpPost]
        [Route("Edict")]
        public async Task<dynamic> Post(
            ApiNoticeCreateEdictArguments arguments,
            [Injection] IServiceBaseHandler<ApiNoticeCreateEdictArguments> handler
        )
        {
            var item = await handler.ExecuteAsync(arguments);
            return new { item.Id };
        }
        #endregion POST /Edict

        #region POST /ImageCrop/{id:int}
        [HttpPut]
		[Route("ImageCrop/{id:int}")]
		public async Task<dynamic> NoticeImage(
			int id,
			NoticeUpdatePhotoArguments arguments,
			[Injection] IServiceBaseHandler<NoticeUpdatePhotoArguments> handler)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /ImageCrop/{id:int}

		#region DELETE /{id:int}
		[HttpDelete]
		[Route("{id:int}")]
		public async Task<dynamic> Delete(
			int id,
			[FromUri] NoticeDeleteArguments arguments,
			[Injection] IServiceBaseHandler<NoticeDeleteArguments> handler
			)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /{id:int}

		#region PUT /IsVisible/{id:int}
		[HttpPut]
		[Route("IsVisible/{id:int}")]
		public async Task<dynamic> IsVisible(
			int id,
			NoticeIsVisibleArguments arguments,
			[Injection] IServiceBaseHandler<NoticeIsVisibleArguments> handler)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion PUT /IsVisible/{id:int}

		#region GET Visibility /{id:int}
		[HttpGet]
		[Route("Visibility/{id:int}")]
		public async Task<ResultBase<NoticeGetVisibilityResult>> GetVisibility(
			int id,
			[FromUri] NoticeGetVisibilityArguments arguments,
			[Injection] IQueryBaseHandler<NoticeGetVisibilityArguments, NoticeGetVisibilityResult> handler)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET Visibility /{id:int}

		#region PUT /Visibility/{id:int}
		[HttpPut]
		[Route("Visibility/{id:int}")]
		public async Task<dynamic> Visibility(
			int id,
			[FromUri]NoticeVisibilityArguments arguments,
			[Injection] IServiceBaseHandler<NoticeVisibilityArguments> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion PUT /Visibility/{id:int}

		#region GET /RetrieveSelector/{filter?}
		[HttpGet]
		[Route("RetrieveSelector/{filter?}")]
		public async Task<ResultBase<NoticeGetSelectorResult>> RetrieveSelector(
			string filter,
			[FromUri] NoticeGetSelectorArguments argument,
			[Injection] IQueryBaseHandler<NoticeGetSelectorArguments, NoticeGetSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(argument);
			return result;
		}
		#endregion GET /RetrieveSelector/{filter?}
	}
}
