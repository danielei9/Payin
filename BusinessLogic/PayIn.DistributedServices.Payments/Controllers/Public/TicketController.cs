using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Payments.Controllers
{
    /// <summary>
    /// Gestión de tickets
    /// </summary>
    [RoutePrefix("public/ticket")]
	[XpAuthorize(
		ClientIds = AccountClientId.PaymentApi,
		Roles = AccountRoles.PaymentWorker + "," + AccountRoles.CommercePayment
    )]
	public class TicketController : ApiController
	{
		#region POST /v1
		/// <summary>
		/// Crear un ticket
		/// </summary>
		/// <remarks>
		/// Este método se utiliza para enviarle toda la información desde el móvil de forma que el usuario pueda modificar los precios, ...
		/// </remarks>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("v1")]
		public async Task<dynamic> Create(
			[FromBody] PublicTicketCreateAndGetArguments arguments,
			[Injection] IServiceBaseHandler<PublicTicketCreateAndGetArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST /v1

		#region POST /v1/Payed
		/// <summary>
		/// Crear un ticket ya pagado externamente
		/// </summary>
		/// <remarks>
		/// Este método se utiliza para crear un ticket que cuyo saldo no es recibido por payin sino por la empresa creadora.
		/// </remarks>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("v1/Payed")]
		public async Task<dynamic> CreatePayedAndGet(
			[FromBody] PublicTicketCreatePayedAndGetArguments arguments,
			[Injection] IServiceBaseHandler<PublicTicketCreatePayedAndGetArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST /v1/Payed

		#region POST /v1/List
		/// <summary>
		/// Crear un conjunto de tickets
		/// </summary>
		/// <remarks>
		/// Este método se utiliza para enviarle toda la información de varios tickets para almacernarlos
		/// </remarks>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpPost]
        [Route("v1/list")]
        public async Task<dynamic> CreateList(
            [FromBody] PublicTicketCreateListArguments arguments,
            [Injection] IServiceBaseHandler<PublicTicketCreateListArguments> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion POST /v1/List

        #region POST /v1/entrances
        /// <summary>
        /// Crear un ticket para comprar entradas
        /// </summary>
        /// <remarks>
        /// Este método se utiliza para comprar entradas, el sistema ya se encarga de calcular los precios, descuentos y gastos de gestión.
        /// </remarks>
        /// <param name="arguments"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        [HttpPost]
		[Route("v1/entrances")]
		public async Task<dynamic> CreateEntrances(
			[FromBody] PublicTicketCreateEntrancesAndGetArguments arguments,
			[Injection] IServiceBaseHandler<PublicTicketCreateEntrancesAndGetArguments> handler
		)
        {
            var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST /v1/entrances

		#region POST /v1/payandcreatewebcardbyuser
		/// <summary>
		/// Pagar un ticket creando tarjeta simultaneamente
		/// </summary>
		/// <remarks>
		/// Al mismo tiempo que se paga el ticket se devuelve el texto de creación de tarjeta para el IFrame
		/// </remarks>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("v1/payandcreatewebcardbyuser")]
		public async Task<dynamic> CreateEntrances(
			[FromBody] PublicTicketPayAndCreateWebCardByUserArguments arguments,
			[Injection] IServiceBaseHandler<PublicTicketPayAndCreateWebCardByUserArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST /v1/payandcreatewebcardbyuser

		#region POST /v1/products
		/// <summary>
		/// Crear un ticket para comprar productos
		/// </summary>
		/// <remarks>
		/// Este método se utiliza para comprar productos, el sistema ya se encarga de calcular los precios y descuentos.
		/// </remarks>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("v1/products")]
		public async Task<dynamic> CreateProducts(
			[FromBody] PublicTicketCreateProductsAndGetArguments arguments,
			[Injection] IServiceBaseHandler<PublicTicketCreateProductsAndGetArguments> handler
		)
        {
            var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST /v1/products

		#region POST /v1/payuser
		/// <summary>
		/// Pagar un ticket por parte de una empresa intermediaria
		/// </summary>
		/// <param name="arguments"></param>
		/// <param name="handler"></param>
		/// <param name="id">Id del ticket</param>
		/// <returns></returns>
		[HttpPost]
		[Route("v1/payuser/{id:int}")]
		public async Task<dynamic> Pay(
			PublicTicketPayUserArguments arguments,
			[Injection] IServiceBaseHandler<PublicTicketPayUserArguments> handler,
			int id
		)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
        #endregion POST /v1/payuser
    }
}
