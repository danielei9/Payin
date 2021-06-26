using PayIn.Application.Dto.Payments.Arguments.Shipment;
using PayIn.Application.Dto.Payments.Results.Shipment;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Payments.Controllers.Api
{
	[HideSwagger]
	[RoutePrefix("Api/Shipment")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker + "," + AccountRoles.User
	)]
	public class ShipmentController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route("")]
		public async Task<ResultBase<ShipmentGetAllResult>> GetAll(
			[FromUri] ShipmentGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ShipmentGetAllArguments, ShipmentGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region GET /
		[HttpGet]	
		[Route("Receipt")]
		[Authorize(Roles = AccountRoles.User)]
		public async Task<ResultBase<ShipmentReceiptGetAllResult>> GetAll(
			[FromUri] ShipmentReceiptGetAllArguments arguments,
			[Injection] IQueryBaseHandler<ShipmentReceiptGetAllArguments, ShipmentReceiptGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<ShipmentGetResult>> Get(
			[FromUri] ShipmentGetArguments query,
			[Injection] IQueryBaseHandler<ShipmentGetArguments, ShipmentGetResult> handler
		)
		{
			var result = await handler.ExecuteAsync(query);
			return result;
		}
		#endregion  GET /{id:int}

		#region GET /Details
		[HttpGet]
		[Route("Details/{id:int}")]
		public async Task<ResultBase<ShipmentTicketGetAllResult>> GetDetails(
			[FromUri] ShipmentTicketGetAllArguments query,
			[Injection] IQueryBaseHandler<ShipmentTicketGetAllArguments, ShipmentTicketGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(query);
			return result;
		}
		#endregion  GET /Details

		#region GET /AddUser
		[HttpGet]
		[Route("AddUser/{id:int}")]
		public async Task<ResultBase<ShipmentGetAddUserResult>> GetAddUser(
			[FromUri] ShipmentGetAddUserArguments query,
			[Injection] IQueryBaseHandler<ShipmentGetAddUserArguments, ShipmentGetAddUserResult> handler
		)
		{
			var result = await handler.ExecuteAsync(query);
			return result;
		}
		#endregion  GET /AddUser

		#region PUT /{id:int}
		[HttpPut]
		[Route("{id:int}")]
		public async Task<dynamic> Put(
			ShipmentUpdateArguments command,
			[Injection] IServiceBaseHandler<ShipmentUpdateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };
		}
		#endregion PUT /{id:int}
		

		#region POST /Tickets
		[HttpPost]
		[Route("Tickets/{shipmentId:int}")]
		public async Task<dynamic> Post(
			ShipmentTicketsCreateArguments arguments,
			[Injection] IServiceBaseHandler<ShipmentTicketsCreateArguments> handler
		)
		{
			var items = await handler.ExecuteAsync(arguments);
			return null;
		}
		#endregion POST /Tickets

		#region POST /
		[HttpPost]
		[Route("")]
		public async Task<dynamic> Post(
			ShipmentCreateArguments arguments,
			[Injection] IServiceBaseHandler<ShipmentCreateArguments> handler
		)
		{
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion POST /

		#region Delete /{id:int}
		[HttpDelete]
		[Route("{id:int}")]
		public async Task<dynamic> Post(
			[FromUri] ShipmentDeleteArguments arguments,
			[Injection] IServiceBaseHandler<ShipmentDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion Delete /{id:int}

		#region Delete /Ticket/{ticketId:int}
		[HttpDelete]
		[Route("Ticket/{id:int}")]
		public async Task<dynamic> DeleteTicket(
			[FromUri] ShipmentTicketDeleteArguments arguments,
			[Injection] IServiceBaseHandler<ShipmentTicketDeleteArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion Delete /Ticket/{ticketId:int}
	}
}
