using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Transport.Controllers
{
    [RoutePrefix("Public/TransportOperation")]
    [XpAuthorize(
        ClientIds = AccountClientId.PaymentApi,
        Roles = AccountRoles.PaymentWorker + "," + AccountRoles.CommercePayment
    )]
    public class TransportOperationController : ApiController
    {
        #region POST /v1/Refund/{int:id}
        /// <summary>
        /// Devolver una operación ya cobrada externamente
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/Refund/{id:int}")]
        public async Task<dynamic> Refund(
            int id,
            [FromBody] PublicTransportOperationRefundArguments arguments,
            [Injection] IServiceBaseHandler<PublicTransportOperationRefundArguments> handler
        )
        {
            arguments.Id = id;
            var item = await handler.ExecuteAsync(arguments);
            return new { item.Id };
        }
        #endregion POST /v1/Refund/{int:id}
    }
}
