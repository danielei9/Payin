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
	[RoutePrefix("Api/EntranceTypeForm")]
	[XpAuthorize(
	ClientIds = AccountClientId.Web,
	Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
	)]

	public class ApiEntranceTypeFormController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route]
        [XpAuthorize(
            ClientIds = AccountClientId.Web,
            Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker + "," + AccountRoles.Superadministrator
        )]
        public async Task<ResultBase<EntranceTypeFormGetAllResult>> GetAll(
			[FromUri] EntranceTypeFormGetAllArguments arguments,
			[Injection] IQueryBaseHandler<EntranceTypeFormGetAllArguments, EntranceTypeFormGetAllResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /

		#region GET /{id:int}
		[HttpGet]
		[Route("{id:int}")]
		public async Task<ResultBase<EntranceTypeFormGetResult>> Get(
			int id,
			[FromUri] EntranceTypeFormGetArguments arguments,
			[Injection] IQueryBaseHandler<EntranceTypeFormGetArguments, EntranceTypeFormGetResult> handler)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /{id:int}

		#region PUT /{id:int}
		[HttpPut]
		[Route("{id:int}")]
		[XpAuthorize(Roles = AccountRoles.Superadministrator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment)]
		public async Task<dynamic> Put(
			int id,
			EntranceTypeFormUpdateArguments arguments,
			[Injection] IServiceBaseHandler<EntranceTypeFormUpdateArguments> handler)
		{
			arguments.Id = id;
			var item = await handler.ExecuteAsync(arguments);
			return new { item.Id };
		}
		#endregion PUT /{id:int}

		#region POST /
		[HttpPost]
        [Route]
        [XpAuthorize(
            ClientIds = AccountClientId.Web,
            Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker + "," + AccountRoles.Superadministrator
        )]
        public async Task<dynamic> Post(
            EntranceTypeFormCreateArguments arguments,
            [Injection] IServiceBaseHandler<EntranceTypeFormCreateArguments> handler
        )
        {
            var item = await handler.ExecuteAsync(arguments);
            return new { item.Id };
        }
        #endregion POST /

        #region DELETE /{id:int}
        [HttpDelete]
        [Route("{id:int}")]
        [XpAuthorize(
            ClientIds = AccountClientId.Web,
            Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.PaymentWorker + "," + AccountRoles.Superadministrator
        )]
        public async Task<dynamic> Delete(
            int id,
            [FromUri] EntranceTypeFormDeleteArguments command,
            [Injection] IServiceBaseHandler<EntranceTypeFormDeleteArguments> handler
            )
        {
            var result = await handler.ExecuteAsync(command);
            return result;
        }
        #endregion DELETE /{id:int}
    }
}
