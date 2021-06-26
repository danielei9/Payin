using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
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
	[RoutePrefix("Mobile/Contact")]
	[XpAuthorize(
		ClientIds = AccountClientId.AndroidNative,
		Roles = AccountRoles.User
	)]
	public class MobileContactController : ApiController
	{
		#region POST /v1/Exhibitor
		[HttpPost]
		[Route("v1/Exhibitor")]
		public async Task<dynamic> PostExhibitor(
			MobileContactScanExhibitorArguments arguments,
			[Injection] IServiceBaseHandler<MobileContactScanExhibitorArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			if (item != null)
				return new { item.Id };

			return null;
		}
		#endregion POST /v1/Exhibitor

		#region POST /v1/Visitor
		[HttpPost]
		[Route("v1/Visitor")]
		public async Task<dynamic> PostVisitor(
			MobileContactScanVisitorArguments arguments,
			[Injection] IServiceBaseHandler<MobileContactScanVisitorArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /v1/Visitor

		#region GET /v1
		[HttpGet]
		[Route("v1")]
		public async Task<ResultBase<MobileContactGetAllResult>> GetAllContact(
			[FromUri] MobileContactGetAllArguments arguments,
			[Injection] IQueryBaseHandler<MobileContactGetAllArguments, MobileContactGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1

		#region DELETE v1/{id:int}
		[HttpDelete]
		[Route("v1/{id:int}")]
		public async Task<dynamic> Delete(
				int id,
				[FromUri] MobileContactDeleteArguments arguments,
				[Injection] IServiceBaseHandler<MobileContactDeleteArguments> handler
		)
		{
			arguments.Id = id;

			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE v1/{id:int}

		#region PUT /Lock/{id:int}
		[HttpPut]
		[Route("v1/Lock/{id:int}")]
		public async Task<dynamic> Lock(
			int id,
			[FromUri] MobileContactLockArguments arguments,
			[Injection] IServiceBaseHandler<MobileContactLockArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion PUT /Lock/{id:int}

		#region PUT /Unlock/{id:int}
		[HttpPut]
		[Route("v1/Unlock/{id:int}")]
		public async Task<dynamic> Unlock(
			int id,
			[FromUri] MobileContactUnlockArguments arguments,
			[Injection] IServiceBaseHandler<MobileContactUnlockArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion PUT /Unlock/{id:int}
	}
}