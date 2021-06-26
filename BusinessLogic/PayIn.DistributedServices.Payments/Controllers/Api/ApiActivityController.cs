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
	[RoutePrefix("Api/Activity")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
	)]
	public class ApiActivityController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route]
		public async Task<ResultBase<ApiActivityGetAllResult>> GetAll(
			[FromUri] ApiActivityGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ApiActivityGetAllArguments, ApiActivityGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<ActivityGetResult>> Get(
			int id,
			[FromUri] ActivityGetArguments arguments,
			[Injection] IQueryBaseHandler<ActivityGetArguments, ActivityGetResult> handler)
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
			ActivityUpdateArguments arguments,
			[Injection] IServiceBaseHandler<ActivityUpdateArguments> handler)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /{id:int}

		#region POST /
		[HttpPost]
		[Route("{eventId:int}")]
		public async Task<dynamic> Post(
			int eventId,
			ActivityCreateArguments arguments,
			[Injection] IServiceBaseHandler<ActivityCreateArguments> handler
		)
		{
			arguments.EventId = eventId;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /

		#region DELETE /{id:int}
		[HttpDelete]
		[Route("{id:int}")]
		public async Task<dynamic> Delete(
			int id,
			[FromUri] ActivityDeleteArguments arguments,
			[Injection] IServiceBaseHandler<ActivityDeleteArguments> handler
			)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /{id:int}
	}
}
