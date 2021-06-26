using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Application.Dto.Payments.Results;
using PayIn.Common;
using PayIn.Domain.Security;
using PayIn.Web.Security;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Payments.Controllers
{
	[HideSwagger]
	[RoutePrefix("Mobile/Notice")]
	public class MobileNoticeController : ApiController
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

		#region GET /v1/public
		[HttpGet]
        [Route("v1/public")]
        public async Task<ResultBase<MobileNoticeGetAllPublicResult>> GetAll(
            [FromUri] MobileNoticeGetAllPublicArguments arguments,
            [Injection] IQueryBaseHandler<MobileNoticeGetAllPublicArguments, MobileNoticeGetAllPublicResult> handler
        )
        {
			arguments.Language = GetHeaderLanguage();
			var result = await handler.ExecuteAsync(arguments);
            return result;
        }
        #endregion GET /v1/public

        #region GET /v1/Event/{id:int}
        [HttpGet]
		[Route("v1/Event/{id:int}")]
        [XpAuthorize(
            ClientIds = AccountClientId.AndroidNative,
            Roles = AccountRoles.User
        )]
        public async Task<ResultBase<MobileNoticeGetAllByEventResult>> GetAll(
			[FromUri] MobileNoticeGetAllByEventArguments arguments,
			[Injection] IQueryBaseHandler<MobileNoticeGetAllByEventArguments, MobileNoticeGetAllByEventResult> handler
		)
		{
			//arguments.Language = GetHeaderLanguage();
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
        #endregion GET /v1/Event/{id:int}

        #region GET /v1/Event/GetNotice/{id:int}
        [HttpGet]
		[Route("v1/Event/GetNotice/{id:int}")]
        [XpAuthorize(
            ClientIds = AccountClientId.AndroidNative + "," + AccountClientId.AndroidFallesNative + "," + AccountClientId.AndroidVilamarxantNative + "," + AccountClientId.AndroidFinestratNative,
            Roles = AccountRoles.User
        )]
        public async Task<ResultBase<MobileNoticeGetByEventResult>> Get(
			int id,
			[FromUri] MobileNoticeGetByEventArguments arguments,
			[Injection] IQueryBaseHandler<MobileNoticeGetByEventArguments, MobileNoticeGetByEventResult> handler
		)
		{
			//arguments.Language = GetHeaderLanguage();
			arguments.Id = id;
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
        #endregion GET /v1/Event/GetNotice/{id:int}

        #region GET /v1/{id:int}
        [HttpGet]
        [Route("v1/{id:int}")]
        public async Task<ResultBase<MobileNoticeGetResult>> Get(
            int id,
            [FromUri] MobileNoticeGetArguments arguments,
            [Injection] IQueryBaseHandler<MobileNoticeGetArguments, MobileNoticeGetResult> handler
        )
        {
            arguments.Id = id;
			arguments.Language = GetHeaderLanguage();
			var result = await handler.ExecuteAsync(arguments);
            return result;
        }
		#endregion GET /v1/{id:int}

		#region GET /v1/Edicts
		[HttpGet]
		[Route("v1/Edicts")]
		public async Task<ResultBase<MobileNoticeGetEdictsResult>> GetEdicts(
			[FromUri] MobileNoticeGetEdictsArguments arguments,
			[Injection] IQueryBaseHandler<MobileNoticeGetEdictsArguments, MobileNoticeGetEdictsResult> handler
		)
		{
			arguments.Language = GetHeaderLanguage();
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1/Edicts

		#region GET /v1/Pages
		[HttpGet]
		[Route("v1/Pages")]
		public async Task<ResultBase<MobileNoticeGetPagesResult>> GetPages(
			[FromUri] MobileNoticeGetPagesArguments arguments,
			[Injection] IQueryBaseHandler<MobileNoticeGetPagesArguments, MobileNoticeGetPagesResult> handler
		)
		{
			arguments.Language = GetHeaderLanguage();
			var result = await handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion GET /v1/Pages
	}
}
