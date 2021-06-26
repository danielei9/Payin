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
	/// Gestión de validaciones de entrada y salida
	/// </summary>
	[RoutePrefix("public/entrancecheck")]
	[XpAuthorize(
		ClientIds = AccountClientId.PaymentApi,
		Roles = AccountRoles.PaymentWorker + "," + AccountRoles.CommercePayment
	)]
	public class EntranceCheckController : ApiController
	{
		#region POST /v1/qr
		/// <summary>
		/// Validar entrada o salida por QR del evento mediante una entrada gestionada por pay[in]
		/// </summary>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("v1/qr")]
		public async Task<MobileEntranceCheckCreateResult> PostQr(
			[FromBody] MobileEntranceCheckCreateQrArguments arguments,
			[Injection] IServiceBaseHandler<MobileEntranceCheckCreateQrArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result as MobileEntranceCheckCreateResult;
		}
		#endregion POST /v1/qr

		#region POST /v1/text
		/// <summary>
		/// Validar entrada o salida por el texto de debajo del QR  del evento mediante una entrada gestionada por pay[in]
		/// </summary>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("v1/text")]
		public async Task<MobileEntranceCheckCreateResult> PostText(
			[FromBody] MobileEntranceCheckCreateTextArguments arguments,
			[Injection] IServiceBaseHandler<MobileEntranceCheckCreateTextArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result as MobileEntranceCheckCreateResult;
		}
		#endregion POST /v1/text

		#region POST /v1/userqr
		/// <summary>
		/// Validar entrada o salida por QR del evento mediante una entrada que se autogenera en pay[in].
		/// </summary>
		/// <remarks>
		/// Se utiliza para la validación cuando existen entradas que no están introducidas en Pay[in]
		/// y por tanto cuando se validan y no existen se introducen en el sistema
		/// </remarks>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("v1/userqr")]
		public async Task<MobileEntranceCheckCreateResult> PostUserQr(
			[FromBody] PublicEntranceCheckCreateUserQrArguments arguments,
			[Injection] IServiceBaseHandler<PublicEntranceCheckCreateUserQrArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result as MobileEntranceCheckCreateResult;
		}
		#endregion POST /v1/qr
    }
}
