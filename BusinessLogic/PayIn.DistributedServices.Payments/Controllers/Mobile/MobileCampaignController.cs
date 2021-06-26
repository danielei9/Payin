using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData.Query;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Payments.Controllers
{
	[HideSwagger]
	[RoutePrefix("Mobile/Campaign")]
	[XpAuthorize(
		ClientIds = AccountClientId.AndroidNative,
		Roles = AccountRoles.User
	)]
	public class MobileCampaignController : ApiController
	{
		#region GET /v1/{id:int}
		[HttpGet]
		[Route("v1/{id:int}")]
		public async Task<ResultBase<MobileCampaignGetResult>> Get(
			int id,
			[FromUri] MobileCampaignGetArguments arguments,
			[Injection] IQueryBaseHandler<MobileCampaignGetArguments, MobileCampaignGetResult> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1/{id:int}		

		#region Get /v1
		[HttpGet]
		[Route("v1")]
		public async Task<ResultBase<MobileCampaignGetAllResult>> Get(
			[FromUri] MobileCampaignGetAllArguments arguments,
			ODataQueryOptions<MobileCampaignGetAllResult> options,
			[Injection] IQueryBaseHandler<MobileCampaignGetAllArguments, MobileCampaignGetAllResult> handler
		)
		{
			arguments.Skip = options.Skip == null ? 0 : options.Skip.Value;
			arguments.Top = Math.Min(options.Top == null ? 100 : options.Top.Value, 100);

			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion Get /v1
	}
}
