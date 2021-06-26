using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Application.Dto.Payments.Results.CampaignCode;
using PayIn.Domain.Payments;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Payments.Controllers
{
	/// <summary>
	/// Gestiona los Códigos de las campañas de fidelización
	/// </summary>
	[RoutePrefix("public/campaigncode")]
	[XpAuthorize(
		ClientIds = AccountClientId.PaymentApi,
		Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker
	)]
	public class CampaignCodeController : ApiController
    {
        #region POST v1
        /// <summary>
        /// Crear código de una campaña para un usaurio
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1")]
        public async Task<PublicCampaignCodeCreateResult> Create(
            PublicCampaignCodeCreateArguments arguments,
            [Injection] IServiceBaseHandler<PublicCampaignCodeCreateArguments> handler
        )
        {
            var item = (await handler.ExecuteAsync(arguments)) as CampaignCode;

            var result = new PublicCampaignCodeCreateResult
            {
                Id = item.Id,
                CodeText = item.Code.ToString(),
                CampaignId = item.CampaignId,
                Code = string.Format("pay[in]/campaign:{{\"code\":{0},\"id\":{1}}}", item.Code, item.CampaignId)
            };
            return result;
        }
        #endregion GET v1
    }
}
