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
	/// Gestiona las Campañas de Fidelización
	/// </summary>
	[RoutePrefix("public/campaign")]
	[XpAuthorize(
		ClientIds = AccountClientId.PaymentApi,
		Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker
	)]
	public class CampaignController : ApiController
    {
        #region GET v1
        /// <summary>
        /// Listado de campañas
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("v1")]
        public async Task<ResultBase<PublicCampaignGetAllResult>> RetrieveSelector(
            [FromUri] PublicCampaignGetAllArguments arguments,
            [Injection] IQueryBaseHandler<PublicCampaignGetAllArguments, PublicCampaignGetAllResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET v1

        #region GET v1/WithLines
        /// <summary>
        /// Listado de campañas activas a dia de hoy con sus líneas
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/WithLines")]
        public async Task<ResultBase<PublicCampaignGetAllWithLinesResult>> RetrieveSelector(
            [FromUri] PublicCampaignGetAllWithLinesArguments arguments,
            [Injection] IQueryBaseHandler<PublicCampaignGetAllWithLinesArguments, PublicCampaignGetAllWithLinesResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET v1/WithLines

        #region GET v1/user (SOBRA)
        /// <summary>
        /// Listado de campañas
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        [HttpGet]
		[Route("v1/user")]
		public async Task<ResultBase<PublicCampaignGetByUserResult>> RetrieveSelector(
			[FromUri] PublicCampaignGetByUserArguments arguments,
			[Injection] IQueryBaseHandler<PublicCampaignGetByUserArguments, PublicCampaignGetByUserResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
        #endregion GET v1/user (SOBRA)

        #region GET v1/qr (SOBRA)
        /// <summary>
        /// Devolver el QR de una campaña
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("v1/qr")]
        public async Task<ResultBase<PublicCampaignGetQrResult>> RetrieveQr(
            [FromUri] PublicCampaignGetQrArguments arguments,
            [Injection] IQueryBaseHandler<PublicCampaignGetQrArguments, PublicCampaignGetQrResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET v1/qr (SOBRA)
    }
}
