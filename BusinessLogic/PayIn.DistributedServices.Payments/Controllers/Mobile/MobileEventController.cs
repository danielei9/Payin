using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Common;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Payments.Controllers
{
	[HideSwagger]
	[RoutePrefix("Mobile/Event")]
	public class MobileEventController : ApiController
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

		#region GET /v1/{id:int}
		[HttpGet]
		[Route("v1/{id:int}")]
		public async Task<ResultBase<MobileEventGetResult>> Get(
			[FromUri] MobileEventGetArguments arguments,
			[Injection] IQueryBaseHandler<MobileEventGetArguments, MobileEventGetResult> handler
		)
		{
			arguments.Language = GetHeaderLanguage();
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
        #endregion GET /v1/{id:int}

        #region GET /v1/Map/{id:int}
        [HttpGet]
        [Route("v1/Map/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative,
			Roles = AccountRoles.User
		)]
		public async Task<ResultBase<MobileEventGetMapResult>> GetMap(
            [FromUri] MobileEventGetMapArguments arguments,
            [Injection] IQueryBaseHandler<MobileEventGetMapArguments, MobileEventGetMapResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /v1/Map/{id:int}

        #region GET /v1/CheckView/{id:int}
        [HttpGet]
        [Route("v1/CheckView/{id:int}")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative,
			Roles = AccountRoles.User
		)]
		public async Task<ResultBase<MobileEventGetCheckViewResult>> GetCheckView(
            [FromUri] MobileEventGetCheckViewArguments arguments,
            [Injection] IQueryBaseHandler<MobileEventGetCheckViewArguments, MobileEventGetCheckViewResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /v1/CheckView/{id:int}

        #region GET /v1
        [HttpGet]
		[Route("v1")]
		public async Task<ResultBase<MobileEventGetAllResult>> GetAll(
			[FromUri] MobileEventGetAllArguments arguments,
			[Injection] IQueryBaseHandler<MobileEventGetAllArguments, MobileEventGetAllResult> handler
		)
		{
			arguments.Language = GetHeaderLanguage();
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
        #endregion GET /v1

        #region GET /v1/Calendar
        [HttpGet]
        [Route("v1/Calendar")]
        [XpAuthorize(
            ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidFallesNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative,
            Roles = AccountRoles.User
        )]
        public async Task<ResultBase<MobileEventGetCalendarResult>> GetCalendar(
            [FromUri] MobileEventGetCalendarArguments arguments,
            [Injection] IQueryBaseHandler<MobileEventGetCalendarArguments, MobileEventGetCalendarResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /v1/Calendar

        #region GET /v1/CheckView
        [HttpGet]
		[Route("v1/CheckView")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative,
			Roles = AccountRoles.User
		)]
		public async Task<ResultBase<MobileEventGetAllCheckViewResult>> GetAllCheckView(
			[FromUri] MobileEventGetAllCheckViewArguments arguments,
			[Injection] IQueryBaseHandler<MobileEventGetAllCheckViewArguments, MobileEventGetAllCheckViewResult> handler
		)
		{
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
        #endregion GET /v1/CheckView
        
        #region GET /v1/SelectorOpened/{filter}
        [HttpGet]
        [Route("v1/SelectorOpened/{filter}")]
		[XpAuthorize(
			ClientIds = AccountClientId.AndroidNative,
			Roles = AccountRoles.User
		)]
		public async Task<ResultBase<SelectorResult>> SelectorOpened(
            [FromUri] MobileEventGetSelectorOpenedArguments arguments,
            [Injection] IQueryBaseHandler<MobileEventGetSelectorOpenedArguments, SelectorResult> handler
        )
        {
            var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /v1/SelectorOpened/{filter}
    }
}
