using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers.Api
{
	[HideSwagger]
	[RoutePrefix("Api/Event")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
	)]
	public class ApiEventController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.Superadministrator
		)]
		public async Task<ResultBase<ApiEventGetAllResult>> GetAll(
			[FromUri] ApiEventGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ApiEventGetAllArguments, ApiEventGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
        #endregion GET /

        #region GET /{id:int}
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ResultBase<EventGetResult>> Get(
            int id,
			[FromUri] EventGetArguments arguments,
			[Injection] IQueryBaseHandler<EventGetArguments, EventGetResult> handler)
		{
            arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{id:int}

		#region GET Visibility /{id:int}
		[HttpGet]
		[Route("Visibility/{id:int}")]
		public async Task<ResultBase<EventGetVisibilityResult>> GetVisibility(
			int id,
			[FromUri] EventGetVisibilityArguments arguments,
			[Injection] IQueryBaseHandler<EventGetVisibilityArguments, EventGetVisibilityResult> handler)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET Visibility /{id:int}

		#region GET /RetrieveSelector/{filter?}
		[HttpGet]
		[Route("RetrieveSelector/{filter?}")]
		public async Task<ResultBase<EventGetSelectorResult>> RetrieveSelector(
			string filter,
			[FromUri] EventGetSelectorArguments argument,
			[Injection] IQueryBaseHandler<EventGetSelectorArguments, EventGetSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(argument);
			return result;
		}
        #endregion GET /RetrieveSelector/{filter?}

        #region GET /Create
        [HttpGet]
        [Route("Create")]
        public async Task<ResultBase<EventGetCreateResult>> Create(
            [FromUri] EventGetCreateArguments argument,
            [Injection] IQueryBaseHandler<EventGetCreateArguments, EventGetCreateResult> handler
        )
        {
            var result = await handler.ExecuteAsync(argument);
            return result;
        }
        #endregion GET /RetrieveSelector

        #region PUT /{id:int}
        [HttpPut]
		[Route("{id:int}")]
		[XpAuthorize(Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment)]
		public async Task<dynamic> Put(
			int id,
			EventUpdateArguments arguments,
			[Injection] IServiceBaseHandler<EventUpdateArguments> handler)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /{id:int}

		#region POST /
		[HttpPost]
		[Route]
		public async Task<dynamic> Post(
			EventCreateArguments command,
			[Injection] IServiceBaseHandler<EventCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
		#endregion POST /

		#region PUT /ImageCrop/{id:int}
		[HttpPut]
		[Route("ImageCrop/{id:int}")]
		public async Task<dynamic> EventImage(
			int id,
			EventUpdatePhotoArguments arguments,
			[Injection] IServiceBaseHandler<EventUpdatePhotoArguments> handler)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /ImageCrop/{id:int}

		#region PUT /ImageMenuCrop/{id:int}
		[HttpPut]
		[Route("ImageMenuCrop/{id:int}")]
		public async Task<dynamic> EventImage(
			int id,
			EventUpdatePhotoMenuArguments arguments,
			[Injection] IServiceBaseHandler<EventUpdatePhotoMenuArguments> handler)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /ImageMenuCrop/{id:int}

		#region DELETE /{id:int}
		[HttpDelete]
		[Route("{id:int}")]
		[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> Delete(
			int id,
			[FromUri] EventDeleteArguments command,
			[Injection] IServiceBaseHandler<EventDeleteArguments> handler
			)
			{
                command.Id = id;
				var result = await handler.ExecuteAsync(command);
				return result;
			}
        #endregion DELETE /{id:int}

        #region PUT /Suspend/{id:int}
        [HttpPut]
        [Route("Suspend/{id:int}")]
        public async Task<dynamic> Suspend(
            int id,
            [FromUri]EventSuspendArguments arguments,
            [Injection] IServiceBaseHandler<EventSuspendArguments> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
		#endregion PUT /Suspend/{id:int}

		#region PUT /Visibility/{id:int}
		[HttpPut]
		[Route("Visibility/{id:int}")]
		public async Task<dynamic> Visibility(
			int id,
			[FromUri]EventVisibilityArguments arguments,
			[Injection] IServiceBaseHandler<EventVisibilityArguments> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion PUT /Visibility/{id:int}

		#region PUT /Unsuspend/{id:int}
		[HttpPut]
        [Route("Unsuspend/{id:int}")]
        [XpAuthorize(
            ClientIds = AccountClientId.Web,
            Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
        )]
        public async Task<dynamic> Unsuspend(
            int id,
            [FromUri] EventUnsuspendArguments arguments,
            [Injection] IServiceBaseHandler<EventUnsuspendArguments> handler
            )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
		#endregion PUT /Unsuspend/{id:int}

		#region PUT /IsVisible/{id:int}
		[HttpPut]
		[Route("IsVisible/{id:int}")]
		[XpAuthorize(Roles = AccountRoles.Superadministrator + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker)]
		public async Task<dynamic> IsVisible(
			int id,
			EventIsVisibleArguments arguments,
			[Injection] IServiceBaseHandler<EventIsVisibleArguments> handler)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
        #endregion PUT /IsVisible/{id:int}

        #region POST /ImageGallery/{id:int}
        [HttpPut]
        [Route("AddImage/{id:int}")]
        public async Task<dynamic> AddEventGalleryImage(
            int id,
            EventAddImageGalleryArguments arguments,
            [Injection] IServiceBaseHandler<EventAddImageGalleryArguments> handler)
        {
            arguments.Id = id;
            var item = await handler.ExecuteAsync(arguments);
            return new { item.Id };
        }
        #endregion POST /ImageGallery/{id:int}

        #region PUT /DeleteImageGallery/{id:int}
        [HttpPut]
        [Route("DeleteImage/{id:int}")]
        [XpAuthorize(
            ClientIds = AccountClientId.Web,
            Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
        )]
        public async Task<dynamic> DeleteImageGallery(
            int id,
            [FromUri] EventDeleteImageGalleryArguments arguments,
            [Injection] IServiceBaseHandler<EventDeleteImageGalleryArguments> handler
            )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
		#endregion PUT /DeleteImageGallery/{id:int}

		#region GET /ProfileSelector/{filter?}
		[HttpGet]
		[Route("ProfileRetrieveSelector/{filter?}")]
		public async Task<ResultBase<EventProfileGetSelectorResult>> Selector(
			string filter,
			[FromUri] EventProfileGetSelectorArguments arguments,
			[Injection] IQueryBaseHandler<EventProfileGetSelectorArguments, EventProfileGetSelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
        #endregion GET /ProfileSelector/{filter?}

        #region POST /MapImageCrop/{id:int}
        [HttpPut]
        [Route("MapImageCrop/{id:int}")]
        public async Task<dynamic> EventMapImage(
            int id,
            EventUpdateMapPhotoArguments arguments,
            [Injection] IServiceBaseHandler<EventUpdateMapPhotoArguments> handler)
        {
            arguments.Id = id;
            var item = await handler.ExecuteAsync(arguments);
            return new { item.Id };
        }
        #endregion POST /MapImageCrop/{id:int}
    }
}
