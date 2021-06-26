using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.Application.Results;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers.Public
{
	/// <summary>
	/// Gestión de eventos
	/// </summary>
	/// <remarks>
	/// En evento se diferencian varios tipos de fechas:
	/// - eventStart y eventEnd se refiere a la fecha-hora de inicio y fin del evento
	/// - checkInStart y checkInEnd se refiere a la fecha-hora de inicio y fin del check, o lo que es lo mismo al periodo apertura de puertas
	/// </remarks>
	[RoutePrefix("public/event")]
	[XpAuthorize(
		ClientIds = AccountClientId.PaymentApi,
		Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
	)]
	public class EventController : ApiController
	{
		#region GET v1
		/// <summary>
		/// Obtener listado de eventos
		/// </summary>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("v1")]
		public async Task<ResultBase<EventGetAllResult>> GetAll(
			[FromUri] EventGetAllArguments arguments,
			[Injection] IQueryBaseHandler<EventGetAllArguments, EventGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET v1

		#region GET v1/{id:int}
		/// <summary>
		/// Obtener la información de un evento
		/// </summary>
		/// <param name="id"></param>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("v1/{id:int}")]
		public async Task<ResultBase<EventGetResult>> Get(
			int id,
			[FromUri] EventGetArguments arguments,
			[Injection] IQueryBaseHandler<EventGetArguments, EventGetResult> handler)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET v1/{id:int}

		#region PUT v1/{id:int}
		/// <summary>
		/// Modificar un evento
		/// </summary>
		/// <param name="id"></param>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpPut]
		[Route("v1/{id:int}")]
		public async Task<IdResult> Put(
			int id,
			EventUpdateArguments arguments,
			[Injection] IServiceBaseHandler<EventUpdateArguments> handler)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new IdResult { Id = item.Id };
		}
		#endregion PUT v1/{id:int}

		#region POST v1
		/// <summary>
		/// Crear un evento
		/// </summary>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("v1")]
		public async Task<IdResult> Post(
			EventCreateArguments arguments,
			[Injection] IServiceBaseHandler<EventCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new IdResult { Id = item.Id };
		}
		#endregion POST v1/

		#region POST v1/ImageCrop/{id:int}
		/// <summary>
		/// Subir una foto
		/// </summary>
		/// <param name="id"></param>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpPut]
		[Route("v1/imagecrop/{id:int}")]
		public async Task<IdResult> EventImage(
			int id,
			EventUpdatePhotoArguments arguments,
			[Injection] IServiceBaseHandler<EventUpdatePhotoArguments> handler)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new IdResult { Id = item.Id };
		}
		#endregion POST v1/ImageCrop/{id:int}

		#region DELETE v1/{id:int}
		/// <summary>
		/// Eliminar evento
		/// </summary>
		/// <param name="id"></param>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpDelete]
		[Route("v1/{id:int}")]
		public async Task Delete(
			int id,
			[FromUri] EventDeleteArguments arguments,
			[Injection] IServiceBaseHandler<EventDeleteArguments> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
		}
		#endregion DELETE v1/{id:int}
	}
}
