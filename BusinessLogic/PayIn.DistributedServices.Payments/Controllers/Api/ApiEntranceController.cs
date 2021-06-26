using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
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
	[RoutePrefix("Api/Entrance")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.User + "," + AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
	)]
	public class ApiEntranceController : ApiController
	{
		#region GET {id:int}
		[HttpGet]
        [Route("{id:int}")]
        [XpAuthorize(
            ClientIds = AccountClientId.Web,
            Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.Superadministrator
        )]
        public async Task<ResultBase<ApiEntranceGetAllResult>> GetAll(
            [FromUri] ApiEntranceGetAllArguments arguments,
            [Injection] IQueryBaseHandler<ApiEntranceGetAllArguments, ApiEntranceGetAllResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
		#endregion GET {id:int}

		#region GET EntranceTicket/{id:int
		[HttpGet]
		[Route("EntranceTicket/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.Superadministrator
		)]
		public async Task<ResultBase<EntranceTicketGetResult>> EntranceTicket(
			int id,
			[FromUri] EntranceTicketGetArguments arguments,
			[Injection] IQueryBaseHandler<EntranceTicketGetArguments, EntranceTicketGetResult> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET EntranceTicket/{id:int}
		
		#region POST /
		[HttpPost]
		[Route("")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.User
		)]
		public async Task<dynamic> Create(
			[FromBody] ApiEntranceCreateArguments arguments,
			[Injection] IServiceBaseHandler<ApiEntranceCreateArguments> handler
		)
		{
			await handler.ExecuteAsync(arguments);
			return null;
		}
		#endregion POST /

		#region POST Invite/{id:int}
		[HttpPost]
		[Route("Invite/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> Invite(
			ApiEntranceInviteArguments argumets,
			[Injection] IServiceBaseHandler<ApiEntranceInviteArguments> handler
		)
		{
			await handler.ExecuteAsync(argumets);
			return null;
		}
        #endregion POST Invite/{id:int}

        #region GET MailWeb
        [HttpGet]
        [Route("Mail/{id:int}")]
        [XpAuthorize(
            ClientIds = AccountClientId.Web,
            Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.Superadministrator
        )]
        public async Task<ResultBase<EntranceGenerateMailResult>> GetMail(
            [FromUri] EntranceGenerateMailArguments arguments,
            [Injection] IQueryBaseHandler<EntranceGenerateMailArguments, EntranceGenerateMailResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
		#endregion GET MailWeb

		#region PUT /Suspend/{id:int}
		[HttpPut]
		[Route("Suspend/{id:int}")]
		public async Task<dynamic> Suspend(
			int id,
			[FromUri] EntranceSuspendArguments arguments,
			[Injection] IServiceBaseHandler<EntranceSuspendArguments> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return new { Id = result.Id };
		}
		#endregion PUT /Suspend/{id:int}

		#region PUT /Unsuspend/{id:int}
		[HttpPut]
		[Route("Unsuspend/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.Web,
			Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment
		)]
		public async Task<dynamic> Unsuspend(
			int id,
			[FromUri] EntranceUnsuspendArguments arguments,
			[Injection] IServiceBaseHandler<EntranceUnsuspendArguments> handler
			)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return new { Id = result.Id};
		}
		#endregion PUT /Unsuspend/{id:int}

		#region PUT /ChangeCard/{id:int}
		[HttpPut]
        [Route("ChangeCard/{id:int}")]
        public async Task<dynamic> Link(
			int id,
			ApiEntranceChangeCardArguments arguments,
            [Injection] IServiceBaseHandler<ApiEntranceChangeCardArguments> handler
        )
        {
			arguments.Id = id;
            var item = await handler.ExecuteAsync(arguments);
            return new { item.Id };
        }
		#endregion PUT /ChangeCard/{id:int}

	}
}
