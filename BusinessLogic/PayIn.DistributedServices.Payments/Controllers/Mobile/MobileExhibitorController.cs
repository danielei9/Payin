using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Payments.Controllers
{
	[HideSwagger]
	[RoutePrefix("Mobile/Exhibitor")]
	[XpAuthorize(
		ClientIds = AccountClientId.AndroidNative,
		Roles = AccountRoles.User
	)]
	public class MobileExhibitorController : ApiController
	{
        #region GET /v1/{id:int}
        [HttpGet]
        [Route("v1/{id:int}")]
        [XpAuthorize(
            ClientIds = AccountClientId.AndroidNative,
            Roles = AccountRoles.User
        )]
        public async Task<ResultBase<ExhibitorMobileGetResult>> Get(
            int id,
            [FromUri] ExhibitorMobileGetArguments arguments,
            [Injection] IQueryBaseHandler<ExhibitorMobileGetArguments, ExhibitorMobileGetResult> handler
        )
        {
            arguments.Id = id;
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /v1/{id:int}

        #region GET /v1
        [HttpGet]
        [Route("v1")]
        public async Task<ResultBase<MobileExhibitorGetAllResult>> GetAll(
            [FromUri] MobileExhibitorGetAllArguments arguments,
            [Injection] IQueryBaseHandler<MobileExhibitorGetAllArguments, MobileExhibitorGetAllResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
		#endregion GET /v1

		#region GET /v1/AllVisitors
		[HttpGet]
		[Route("v1/AllVisitors")]
		public async Task<ResultBase<MobileExhibitorGetAllVisitorsResult>> AllVisitors(
			[FromUri] MobileExhibitorGetAllVisitorsArguments arguments,
			[Injection] IQueryBaseHandler<MobileExhibitorGetAllVisitorsArguments, MobileExhibitorGetAllVisitorsResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1/AllVisitors

		#region GET /v1/GetVisitor/{id:int}
		[HttpGet]
		[Route("v1/GetVisitor/{id:int}")]
		public async Task<ResultBase<MobileExhibitorGetVisitorResult>> GetVisitor(
			int id,
			[FromUri] MobileExhibitorGetVisitorArguments arguments,
			[Injection] IQueryBaseHandler<MobileExhibitorGetVisitorArguments, MobileExhibitorGetVisitorResult> handler
		)
		{
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1/GetVisitor/{id:int}
	}
}
