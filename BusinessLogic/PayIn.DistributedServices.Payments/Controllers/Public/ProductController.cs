using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;
using System.Collections.Generic;

namespace PayIn.DistributedServices.Payments.Controllers.Api
{
	/// <summary>
	/// Gestor de productos
	/// </summary>
	[RoutePrefix("Public/Product")]
	[XpAuthorize(
		ClientIds = AccountClientId.PaymentApi,
		Roles = 
            AccountRoles.Superadministrator + "," +
            AccountRoles.Operator + "," +
            AccountRoles.PaymentWorker + "," +
            AccountRoles.Commerce + "," +
            AccountRoles.CommercePayment
	)]
	public class ProductController : ApiController
	{
		#region POST /v1/Syncronize
		/// <summary>
		/// Sincronizar la lista de productos
		/// </summary>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpPost]
        [Route("v1/Syncronize")]
        public async Task<dynamic> Syncronize(
			ProductSyncronizeArguments arguments,
            [Injection] IServiceBaseHandler<ProductSyncronizeArguments> handler = null
        )
        {
            await handler.ExecuteAsync(arguments);
            return null;
        }
		#endregion POST /v1/Syncronize
	}
}
