using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Arguments.Main;
using PayIn.Application.Dto.Results;
using PayIn.Application.Dto.Results.Main;
using PayIn.Common;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData.Query;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers
{
    [HideSwagger]
	[RoutePrefix("Mobile/Main")]
    public class MobileMainController : ApiController
	{
		private LanguageEnum GetHeaderLanguage()
		{
			System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;
			LanguageEnum language = LanguageEnum.Spanish;
			if (headers.Contains("Accept-Language"))
			{
				var acceptLanguage = headers.AcceptLanguage.ToString().ToUpper();
				if (acceptLanguage == "ES-VA" || acceptLanguage == "VA-ES")
					language = LanguageEnum.Valencian;
				else if (acceptLanguage == "EN-EN" || acceptLanguage == "EN-US")
					language = LanguageEnum.English;
			}

			return language;
		}

		#region GET /v1
		[HttpGet]
		[Route("v1")]
        [XpAuthorize(
            ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.CashlessProApp + "," + AccountClientId.FallasProApp + "," + AccountClientId.FiraValenciaApp,
            Roles = AccountRoles.User
        )]
        public async Task<ResultBase<MainMobileGetAllResult>> Get(
			[FromUri] MainMobileGetAllArguments arguments,
			ODataQueryOptions<MainMobileGetAllResult> options,
			[Injection] IQueryBaseHandler<MainMobileGetAllArguments, MainMobileGetAllResult> handler
		)
		{
			arguments.Skip = options.Skip == null ? 0 : options.Skip.Value;
			arguments.Top = Math.Min(options.Top == null ? 100 : options.Top.Value, 100);

			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
        #endregion GET /v1

        #region GET /v2
        [HttpGet]
        [Route("v2")]
        [XpAuthorize(
            ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.CashlessProApp + "," + AccountClientId.FallasProApp + "," + AccountClientId.FiraValenciaApp,
            Roles = AccountRoles.User
        )]
        public async Task<ResultBase<MainMobileGetAllV2Result>> Get(
            [FromUri] MainMobileGetAllV2Arguments arguments,
            ODataQueryOptions<MainMobileGetAllV2Result> options,
            [Injection] IQueryBaseHandler<MainMobileGetAllV2Arguments, MainMobileGetAllV2Result> handler
        )
        {
            arguments.Skip = options.Skip == null ? 0 : options.Skip.Value;
            arguments.Top = Math.Min(options.Top == null ? 100 : options.Top.Value, 100);

            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
		#endregion GET /v2

		#region GET /v3
		[HttpGet]
		[Route("v3")]
        [XpAuthorize(
            ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.CashlessProApp + "," + AccountClientId.FallasProApp + "," + AccountClientId.FiraValenciaApp,
            Roles = AccountRoles.User
        )]
        public async Task<ResultBase<MainMobileGetAllV3Result>> Get3(
			[FromUri] MainMobileGetAllV3Arguments arguments,
			ODataQueryOptions<MainMobileGetAllV3Result> options,
			[Injection] IQueryBaseHandler<MainMobileGetAllV3Arguments, MainMobileGetAllV3Result> handler
		)
		{
			arguments.Skip = options.Skip == null ? 0 : options.Skip.Value;
			arguments.Top = Math.Min(options.Top == null ? 100 : options.Top.Value, 100);

			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1

		#region GET /v4
		[HttpGet]
		[Route("v4")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidFallesNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative + "," + AccountClientId.CashlessProApp + "," + AccountClientId.FallasProApp + "," + AccountClientId.FiraValenciaApp,
			Roles = AccountRoles.User
		)]
		public async Task<ResultBase<MobileMainGetAllV4Result>> Get4(
			[FromUri] MobileMainGetAllV4Arguments arguments,
			[Injection] IQueryBaseHandler<MobileMainGetAllV4Arguments, MobileMainGetAllV4Result> handler
		)
		{
			arguments.Language = GetHeaderLanguage();
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v4

		#region GET /v4/Anonymous
		[HttpGet]
		[Route("v4/Anonymous")]
		public async Task<ResultBase<MobileMainGetAllV4Result>> Get4Anonymous(
			[FromUri] MobileMainGetAllV4Arguments arguments,
			[Injection] IQueryBaseHandler<MobileMainGetAllV4Arguments, MobileMainGetAllV4Result> handler
		)
		{
			arguments.Language = GetHeaderLanguage();
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v4/Anonymous

		#region GET /v1/version
		[HttpGet]
		[Route("v1/version")]
		[AllowAnonymous]
		public async Task<MainMobileGetVersionResult> GetVersion(
			[FromUri] MobileMainGetVersionArguments arguments,
			[Injection] IQueryBaseHandler<MobileMainGetVersionArguments, MainMobileGetVersionResult> handler
		)
		{
			var result = (await handler.ExecuteAsync(arguments)).Data;
			return result.FirstOrDefault();
		}
		#endregion GET /v1/version

		#region GET /v1/GetEntailments
		[HttpGet]
		[Route("v1/GetEntailments")]
        [XpAuthorize(
            ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.CashlessProApp + "," + AccountClientId.FallasProApp + "," + AccountClientId.FiraValenciaApp,
            Roles = AccountRoles.User
        )]
        public async Task<ResultBase<MainMobileGetEntailmentsResult>> Get(
			[FromUri] MainMobileGetEntailmentsArguments arguments,
			[Injection] IQueryBaseHandler<MainMobileGetEntailmentsArguments, MainMobileGetEntailmentsResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1/GetEntailments

		#region POST /v1/Synchronize
		[HttpPost]
		[Route("v1/Synchronize")]
        [XpAuthorize(
            ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.CashlessProApp + "," + AccountClientId.FallasProApp + "," + AccountClientId.FiraValenciaApp,
            Roles = AccountRoles.PaymentWorker + "," + AccountRoles.CommercePayment
        )]
        public async Task<dynamic> Synchronize(
			[FromBody] MobileMainSynchronizeArguments arguments,
			[Injection] IServiceBaseHandler<MobileMainSynchronizeArguments> handler
		)
		{
			arguments.Language = GetHeaderLanguage();
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST /v1/Synchronize

		#region POST /v1/Synchronize/Anonymous
		[HttpPost]
		[Route("v1/Synchronize/Anonymous")]
		public async Task<dynamic> SynchronizeAnonymous(
			[FromBody] MobileMainSynchronizeArguments arguments,
			[Injection] IServiceBaseHandler<MobileMainSynchronizeArguments> handler
		)
		{
			arguments.Language = GetHeaderLanguage();
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST /v1/Synchronize/Anonymous
	}
}
