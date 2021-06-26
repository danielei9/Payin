using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
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
	[RoutePrefix("Mobile/ServiceIncidence")]
	[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidFallesNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative,
			Roles = AccountRoles.User
		)]
	public class MobileServiceIncidenceController : ApiController
	{
		#region GET v1/{filter?}
		[HttpGet]
		[Route("v1")]
		//[XpAuthorize(
		//	ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.Web//,
		//	//Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.User
		//)]
		public async Task<ResultBase<MobileServiceIncidenceGetAllResult>> GetAll(
			[FromUri] MobileServiceIncidenceGetAllArguments arguments,
			[Injection] IQueryBaseHandler<MobileServiceIncidenceGetAllArguments, MobileServiceIncidenceGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET v1/{filter?}

		#region GET v1/{id:int}
		[HttpGet]
		[Route("v1/{id:int}")]
		//[XpAuthorize(
		//	ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.Web,
		//	Roles = AccountRoles.User //AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.User
		//)]
		public async Task<ResultBase<MobileServiceIncidenceGetResult>> Get(
			int id,
			[FromUri] MobileServiceIncidenceGetArguments arguments,
			[Injection] IQueryBaseHandler<MobileServiceIncidenceGetArguments, MobileServiceIncidenceGetResult> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET v1/{id:int}

		#region POST v1/
		[HttpPost]
		[Route("v1")]
		public async Task<dynamic> Post(
			MobileServiceIncidenceCreateArguments arguments,
			[Injection] IServiceBaseHandler<MobileServiceIncidenceCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST v1/

		#region PUT v1/
		[HttpPut]
		[Route("v1/{id:int}")]
		public async Task<dynamic> Put(
			int id,
			ServiceIncidenceUpdateArguments arguments,
			[Injection] IServiceBaseHandler<ServiceIncidenceUpdateArguments> handler
		)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT v1/
	}
}
