using System.Threading.Tasks;
using System.Web.Http;
using PayIn.Web.Security;
using PayIn.Domain.Security;
using Xp.Application.Dto;
using Xp.DistributedServices.ModelBinder;
using PayIn.Application.Dto.Arguments;

namespace PayIn.DistributedServices.Controllers.Api
{
	/// <summary>
	/// Gestión de los sistemas de tarjetas de fidelización
	/// </summary>
	[RoutePrefix("public/systemcard")]
	[XpAuthorize(
		ClientIds = AccountClientId.PaymentApi,
		Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment

	)]
	public class PublicSystemCardController : ApiController
	{
		#region GET v1/selector
		/// <summary>
		/// Listado de sistemas de tarjetas de fidelización
		/// </summary>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("v1/selector")]
		public async Task<ResultBase<SelectorResult>> RetrieveSelector(
			[FromUri] ApiSystemCardGetSelectorArguments arguments,
			[Injection] IQueryBaseHandler<ApiSystemCardGetSelectorArguments, SelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET v1/selector
	}
}
