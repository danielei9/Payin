using PayIn.Application.Dto.Arguments;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers.Api
{
	/// <summary>
	/// Gestiona los usuarios de Tarjetas de Fidelización
	/// </summary>
	[RoutePrefix("public/serviceuser")]
	[XpAuthorize(
		ClientIds = AccountClientId.PaymentApi,
		Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker
	)]
	public class PublicServiceUserController : ApiController
	{
		#region POST v1
		/// <summary>
		/// Añade un nuevo miembro a la Tarjeta de Fidelización
		/// </summary>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("v1")]
		public async Task<dynamic> Post(
			ServiceUserCreateArguments arguments,
			[Injection] IServiceBaseHandler<ServiceUserCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST v1
	}
}
