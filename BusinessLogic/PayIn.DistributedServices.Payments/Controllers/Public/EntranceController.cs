using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Payments.Controllers.Mobile
{
	/// <summary>
	/// Gestor de entradas
	/// </summary>
	[RoutePrefix("public/entrance")]
	[XpAuthorize(
		ClientIds = AccountClientId.PaymentApi,
		Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
	)]
	public class EntranceController : ApiController
	{
		#region GET v1/user
		/// <summary>
		/// Obtener listado de entradas de un usaurio
		/// </summary>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("v1/user")]
		public async Task<ResultBase<MobileEntranceGetAllResult>> Get(
			[FromUri] PublicEntranceGetByUserArguments arguments,
			[Injection] IQueryBaseHandler<PublicEntranceGetByUserArguments, MobileEntranceGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET v1/user
	}
}