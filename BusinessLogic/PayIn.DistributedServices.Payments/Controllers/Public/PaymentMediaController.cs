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
	/// Gestión de medios de pago (tarjetas, monederos, ...)
	/// </summary>
	[RoutePrefix("public/paymentmedia")]
	[XpAuthorize(
		ClientIds = AccountClientId.PaymentApi,
		Roles = AccountRoles.PaymentWorker + "," + AccountRoles.CommercePayment
	)]
	public class PaymentMediaController : ApiController
	{
        #region GET v1/user
		/// <summary>
		/// Obtener listado de medios de pago de un usuario
		/// </summary>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
        [HttpGet]
		[Route("v1/user")]
		public async Task<ResultBase<MobilePaymentMediaGetAllResult>> GetAllByUser(
			[FromUri] PublicPaymentMediaGetByUserArguments arguments,
			[Injection] IQueryBaseHandler<PublicPaymentMediaGetByUserArguments, MobilePaymentMediaGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET v1/user

		#region POST /v1/WebCardUser
		/// <summary>
		/// Crear una tarjeta a través de un IFrame a nombre de un usuario
		/// </summary>
		/// <remarks>
		/// Este método solo es válido para empresas que tienen permitido el pago no seguro.
		/// </remarks>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns>Devuelve un objeto con un campo Request. Este campo se debe separar por & y ejecutarlos como inputs en un POST cuyo tarjet sea un IFrame</returns>
		[HttpPost]
		[Route("v1/webcarduser")]
		public async Task<dynamic> CreateWebCardUser(
			PublicPaymentMediaCreateWebCardByUserArguments arguments,
			[Injection] IServiceBaseHandler<PublicPaymentMediaCreateWebCardByUserArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST /v1/WebCardUser
	}
}

