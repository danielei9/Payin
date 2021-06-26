using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers.Api
{
	[HideSwagger]
	[RoutePrefix("Api/ControlFormOption")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
	)]
	public class ControlFormOptionController : ApiController
	{
		#region GET
		/// <summary>
		/// Obtener listado de opciones en formularios de tipo multiopcion
		/// </summary>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpGet]
		[Route]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.Superadministrator
		)]
		public async Task<ResultBase<ControlFormOptionGetAllResult>> GetAll(
			[FromUri] ControlFormOptionGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ControlFormOptionGetAllArguments, ControlFormOptionGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET

		#region GET {id:int}
		/// <summary>
		/// Obtener la información de una opcion de un formulario multiopcion
		/// </summary>
		/// <param name="id"></param>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<ControlFormOptionGetResult>> Get(
			int id,
			[FromUri] ControlFormOptionGetArguments arguments,
			[Injection] IQueryBaseHandler<ControlFormOptionGetArguments, ControlFormOptionGetResult> handler)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET {id:int}

		#region POST {id:int}
		/// <summary>
		/// Crear una opcion
		/// </summary>
		/// <param name="argumentId"></param>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("{argumentId:int}")]
		public async Task<dynamic> Post(
			int argumentId,
			ControlFormOptionCreateArguments arguments,
			[Injection] IServiceBaseHandler<ControlFormOptionCreateArguments> handler
		)
		{
			arguments.ArgumentId = argumentId;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST {id:int}

		#region PUT {id:int}
		/// <summary>
		/// Modificar una opcion
		/// </summary>
		/// <param name="id"></param>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpPut]
		[Route("{id:int}")]
		[XpAuthorize(Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment)]
		public async Task<dynamic> Put(
			int id,
			ControlFormOptionUpdateArguments arguments,
			[Injection] IServiceBaseHandler<ControlFormOptionUpdateArguments> handler)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT {id:int}

		#region DELETE v1/{id:int}
		/// <summary>
		/// Eliminar opcion
		/// </summary>
		/// <param name="id"></param>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpDelete]
		[Route("{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> Delete(
			int id,
			[FromUri] ControlFormOptionDeleteArguments arguments,
			[Injection] IServiceBaseHandler<ControlFormOptionDeleteArguments> handler
			)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE v1/{id:int}
	}
}
