using PayIn.Application.Dto.Arguments.SystemCardMember;
using PayIn.Application.Dto.Results.SystemCardMember;
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
	[RoutePrefix("Api/SystemCardMember")]
	[XpAuthorize(
		ClientIds = AccountClientId.Web,
		Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment

	)]
	public class SystemCardMemberController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route]
		public async Task<ResultBase<SystemCardMemberGetAllResult>> GetAll(
			[FromUri] SystemCardMemberGetAllArguments arguments,
			[Injection] IQueryBaseHandler<SystemCardMemberGetAllArguments, SystemCardMemberGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<SystemCardMemberGetResult>> Get(
			[FromUri] SystemCardMemberGetArguments arguments,
			[Injection] IQueryBaseHandler<SystemCardMemberGetArguments, SystemCardMemberGetResult> handler)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{id:int}

		#region POST /
		[HttpPost]
		[Route]
		[XpAuthorize(Roles = AccountRoles.Superadministrator + "," + AccountRoles.SystemCardOwner)]
		public async Task<dynamic> Create(
			SystemCardMemberCreateArguments arguments,
			[Injection] IServiceBaseHandler<SystemCardMemberCreateArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return new
			{
				Id = result.Id
			};
		}
		#endregion POST /

		#region DELETE /{id:int}
		[HttpDelete]
		[Route("{id:int}")]
		public async Task<dynamic> Delete(
			[FromUri] SystemCardMemberDeleteArguments arguments,
			[Injection] IServiceBaseHandler<SystemCardMemberDeleteArguments> handler
			)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion DELETE /{id:int}

		#region PUT /{id:int}
		[HttpPut]
		[Route("{id:int}")]
		[XpAuthorize(Roles = AccountRoles.SystemCardOwner)]
		public async Task<dynamic> Put(
			int id,
			SystemCardMemberUpdateArguments command,
		[Injection] IServiceBaseHandler<SystemCardMemberUpdateArguments> handler
		)
		{
			command.Id = id;
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };

		}
		#endregion PUT /{id:int}

		#region PUT /Lock/{id:int}
		[HttpPut]
		[Route("Lock/{id:int}")]
		[XpAuthorize(Roles = AccountRoles.SystemCardOwner)]
		public async Task<dynamic> Lock(
			int id,
			SystemCardMemberLockArguments command,
		[Injection] IServiceBaseHandler<SystemCardMemberLockArguments> handler
		)
		{
			command.Id = id;
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };

		}
		#endregion PUT /Lock/{id:int}

		#region PUT /Unlock/{id:int}
		[HttpPut]
		[Route("Unlock/{id:int}")]
		[XpAuthorize(Roles = AccountRoles.SystemCardOwner)]
		public async Task<dynamic> Unlock(
			int id,
			SystemCardMemberUnlockArguments command,
		[Injection] IServiceBaseHandler<SystemCardMemberUnlockArguments> handler
		)
		{
			command.Id = id;
			var item = await handler.ExecuteAsync(command);
			return new { item.Id };

		}
		#endregion PUT /{id:int}		
		
		#region POST /AddInvitedCompany
		[HttpPost]
		[Route("AddInvitedCompany")]
		[XpAuthorize(ClientIds = AccountClientId.Web)]
		public async Task<dynamic> AddInvitedCompany(
			SystemCardMemberAddInvitedCompanyArguments arguments,
			[Injection] IServiceBaseHandler<SystemCardMemberAddInvitedCompanyArguments> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return new
			{
				Id = result.Id
			};
		}
		#endregion POST /AddInvitedCompany
		
	}
}
