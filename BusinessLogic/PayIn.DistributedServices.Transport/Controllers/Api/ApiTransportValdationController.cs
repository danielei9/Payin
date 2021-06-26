using PayIn.Application.Dto.Transport.Arguments.TransportValidation;
using PayIn.Application.Dto.Transport.Results.TransportValidation;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Transport.Controllers.Api
{
	[HideSwagger]
	[RoutePrefix("Api/TransportValidation")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Superadministrator
	)]
	public class TransportValidationController : ApiController
	{
		#region GET /Validation
		[HttpGet]
		[Route("")]
		public async Task<ResultBase<TransportValidationGetAllResult>> GetAll(
			[FromUri] TransportValidationGetAllArguments arguments,
			[Injection] IQueryBaseHandler<TransportValidationGetAllArguments, TransportValidationGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Validation
	}
}