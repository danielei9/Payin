using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Payments.Controllers
{
	/// <summary>
	/// Gestión de empresas de pagos
	/// </summary>
	[RoutePrefix("public/paymentconcession")]
	[XpAuthorize(
		ClientIds = AccountClientId.PaymentApi,
		Roles = AccountRoles.PaymentWorker + "," + AccountRoles.CommercePayment
	)]
	public class PaymentConcessionController : ApiController
	{
     
        #region GET /Selector
        /// <summary>
        /// Obtener el listado de empresas para un selector
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        [HttpGet]
		[Route("v1/selector")]
		public async Task<ResultBase<SelectorResult>> RetrieveSelector(
			[FromUri] PaymentConcessionGetSelectorArguments arguments,
			[Injection] IQueryBaseHandler<PaymentConcessionGetSelectorArguments, SelectorResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /Selector
	}
}
