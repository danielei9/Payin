using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers.Api
{
	/// <summary>
	/// Gestión de tipos de entrada
	/// </summary>
	[RoutePrefix("public/entrancetype")]
	[XpAuthorize(
		ClientIds = AccountClientId.PaymentApi,
		Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker
	)]
	public class EntranceTypeController : ApiController
	{
		#region GET v1
		/// <summary>
		/// Obtener listado de tipos de entrada
		/// </summary>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("v1")]
		[XpAuthorize(
			ClientIds = AccountClientId.PaymentApi,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.Superadministrator
		)]
		public async Task<ResultBase<EntranceTypeGetAllResult>> GetAll(
			[FromUri] EntranceTypeGetAllArguments arguments,
			[Injection] IQueryBaseHandler<EntranceTypeGetAllArguments, EntranceTypeGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET v1

		#region GET v1/{id:int}
		/// <summary>
		/// Obtener la información de un tipo de entrada
		/// </summary>
		/// <param name="id"></param>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("v1/{id:int}")]
		public async Task<ResultBase<EntranceTypeGetResult>> Get(
			int id,
			[FromUri] EntranceTypeGetArguments arguments,
			[Injection] IQueryBaseHandler<EntranceTypeGetArguments, EntranceTypeGetResult> handler)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET v1/{id:int}

		#region PUT v1/{id:int}
		/// <summary>
		/// Modificar un tipo de entrada
		/// </summary>
		/// <param name="id"></param>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpPut]
		[Route("v1/{id:int}")]
		[XpAuthorize(Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment)]
		public async Task<dynamic> Put(
			int id,
			EntranceTypeUpdateArguments arguments,
			[Injection] IServiceBaseHandler<EntranceTypeUpdateArguments> handler)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT v1/{id:int}

		#region POST v1/{id:int}
		/// <summary>
		/// Crear un tipo de entrada
		/// </summary>
		/// <param name="eventId"></param>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("v1/{eventId:int}")]
		public async Task<dynamic> Post(
			int eventId,
			EntranceTypeCreateArguments arguments,
			[Injection] IServiceBaseHandler<EntranceTypeCreateArguments> handler
		)
		{
			arguments.EventId = eventId;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST v1/{id:int}

		#region POST v1/imagecrop/{id:int}
		/// <summary>
		/// Subir una foto del tipo de entrada
		/// </summary>
		/// <param name="id"></param>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpPut]
		[Route("v1/imagecrop/{id:int}")]
		public async Task<dynamic> EntranceTypeImage(
			int id,
			EntranceTypeUpdatePhotoArguments arguments,
			[Injection] IServiceBaseHandler<EntranceTypeUpdatePhotoArguments> handler)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST v1/imagecrop/{id:int}

		#region POST /v1/Syncronize
		/// <summary>
		/// Sincronizar los tipos de entradas
		/// </summary>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("v1/Syncronize")]
		public async Task<dynamic> Syncronize(
			EntranceTypeSyncronizeArguments arguments,
			[Injection] IServiceBaseHandler<EntranceTypeSyncronizeArguments> handler = null
		)
		{
			await handler.ExecuteAsync(arguments);
			return null;
		}
		#endregion POST /v1/Syncronize

		#region DELETE v1/{id:int}
		/// <summary>
		/// Eliminar tipo de entrada
		/// </summary>
		/// <param name="id"></param>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpDelete]
		[Route("v1/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.PaymentApi,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> Delete(
			int id,
			[FromUri] EntranceTypeDeleteArguments arguments,
			[Injection] IServiceBaseHandler<EntranceTypeDeleteArguments> handler
			)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE v1/{id:int}

		#region PUT v1/relocate/{id:int}
		/// <summary>
		/// Eliminar tipo de entrada pasando las entradas a otro tipo
		/// </summary>
		/// <param name="id"></param>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpPut]
        [Route("v1/relocate/{id:int}")]
        [XpAuthorize(
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
        public async Task<dynamic> Relocate(
            int id,
            EntranceTypeRelocateArguments arguments,
            [Injection] IServiceBaseHandler<EntranceTypeRelocateArguments> handler)
        {
            arguments.OldId = id;
            var item = await handler.ExecuteAsync(arguments);
            return null;
        }
        #endregion PUT v1/relocate/{id:int}

        #region PUT v1/isvisible/{id:int}
        /// <summary>
        /// Ocutar o mostrar un tipo de entrada
        /// </summary>
        /// <param name="id"></param>
        /// <param name="arguments"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        [HttpPut]
		[Route("v1/isvisible/{id:int}")]
		public async Task<dynamic> IsVisible(
			int id,
			EntranceTypeIsVisibleArguments arguments,
			[Injection] IServiceBaseHandler<EntranceTypeIsVisibleArguments> handler)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion PUT v1/isvisible/{id:int}

		#region GET Recharge/{id:int}
		[HttpGet]
		[Route("v1/{id:int}")]
		public async Task<ResultBase<EntranceTypeGetResult>> GetRecharge(
			int id,
			[FromUri] EntranceTypeGetArguments arguments,
			[Injection] IQueryBaseHandler<EntranceTypeGetArguments, EntranceTypeGetResult> handler)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET Recharge/{id:int}
	}
}
