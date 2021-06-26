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
	[RoutePrefix("Api/Exhibitor")]
    [XpAuthorize(
           ClientIds = AccountClientId.Web,
           Roles = AccountRoles.Superadministrator + "," + AccountRoles.Operator + "," + AccountRoles.Commerce + "," + AccountRoles.CommercePayment
    )]
    public class ExhibitorController : ApiController
	{
        #region GET /
        [HttpGet]
        [Route]
        [XpAuthorize(
            ClientIds = AccountClientId.Web,
            Roles = AccountRoles.Commerce + "," + AccountRoles.CommercePayment + "," + AccountRoles.Superadministrator
        )]
        public async Task<ResultBase<ExhibitorGetAllResult>> GetAll(
            [FromUri] ExhibitorGetAllArguments arguments,
            [Injection] IQueryBaseHandler<ExhibitorGetAllArguments, ExhibitorGetAllResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /

        #region PUT /Suspend/{id:int}
        [HttpPut]
        [Route("Suspend/{id:int}")]
        public async Task<dynamic> Suspend(
            int id,
            ExhibitorSuspendArguments arguments,
            [Injection] IServiceBaseHandler<ExhibitorSuspendArguments> handler
        )
        {
            arguments.Id = id;
            var item = await handler.ExecuteAsync(arguments);
            return new { item.Id };
        }
        #endregion PUT /Suspend/{id:int}

        #region PUT /Unsuspend/{id:int}
        [HttpPut]
        [Route("Unsuspend/{id:int}")]
        public async Task<dynamic> Unsuspend(
            int id,
            ExhibitorUnsuspendArguments arguments,
            [Injection] IServiceBaseHandler<ExhibitorUnsuspendArguments> handler
        )
        {
            arguments.Id = id;
            var item = await handler.ExecuteAsync(arguments);
            return new { item.Id };
        }
        #endregion PUT /Unsuspend/{id:int}

        #region DELETE /{id:int}
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<dynamic> Delete(
            [FromUri] ExhibitorDeleteArguments arguments,
            [Injection] IServiceBaseHandler<ExhibitorDeleteArguments> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion DELETE /{id:int}

        #region PUT /{id:int}
        [HttpPut]
        [Route("{id:int}")]
        public async Task<dynamic> Put(
            int id,
            ExhibitorUpdateArguments arguments,
            [Injection] IServiceBaseHandler<ExhibitorUpdateArguments> handler)
        {
            arguments.OldId = id;
            var item = await handler.ExecuteAsync(arguments);
            return new { item.Id };
        }
        #endregion PUT /{id:int}

        #region GET /{id:int}
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ResultBase<ExhibitorGetResult>> Get(
            [FromUri] ExhibitorGetArguments query,
            [Injection] IQueryBaseHandler<ExhibitorGetArguments, ExhibitorGetResult> handler
        )
        {
            var result = await handler.ExecuteAsync(query);
            return result;
        }
        #endregion  GET /{id:int}

        #region POST /
        [HttpPost]
        [Route("{eventId:int}")]
        public async Task<dynamic> Post(
             int eventId,
            ExhibitorCreateArguments arguments,
            [Injection] IServiceBaseHandler<ExhibitorCreateArguments> handler
        )
        {
            //arguments.EventId = eventId;
            var item = await handler.ExecuteAsync(arguments);
            return new { item.Id };
        }
        #endregion POST /

        #region GET /RetrieveSelector/{filter?}
        [HttpGet]
        [Route("RetrieveSelector/{filter?}")]
        public async Task<ResultBase<ExhibitorGetSelectorResult>> RetrieveSelector(
            string filter,
            [FromUri] ExhibitorGetSelectorArguments arguments,
            [Injection] IQueryBaseHandler<ExhibitorGetSelectorArguments, ExhibitorGetSelectorResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /RetrieveSelector/{filter?}
    }
}
