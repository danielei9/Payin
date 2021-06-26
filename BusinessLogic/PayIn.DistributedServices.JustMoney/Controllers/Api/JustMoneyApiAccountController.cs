using System.Web.Http;
using Xp.DistributedServices.Filters;
using PayIn.Infrastructure.Security;
using PayIn.Application.Dto.JustMoney.Security.Arguments;
using PayIn.Application.Dto.Security.Arguments;
using System.Threading.Tasks;

namespace PayIn.DistributedServices.JustMoney.Controllers.Api
{
	[HideSwagger]
	[RoutePrefix("JustMoney/Api/Account")]
	public class JustMoneyApiAccountController : ApiController
	{
        private readonly SecurityRepository repository = null;

		#region Constructors
		public JustMoneyApiAccountController()
		{
			repository = new SecurityRepository();
		}
		#endregion Constructors

		#region POST api/Account/ForgotPassword
		[AllowAnonymous]
		[HttpPost]
		[Route("ForgotPassword")]
		public async Task<IHttpActionResult> ForgotPassword(JustMoneyApiAccountForgotPasswordArguments arguments)
		{
			var newArguments = new AccountForgotPasswordArguments
			{
				Email = arguments.Email
			};

			await repository.ForgotPassword(newArguments);

			return Ok();
		}
		#endregion POST api/Account/ForgotPassword

		#region POST JustMoney/Api/Account/ConfirmForgotPassword
		[AllowAnonymous]
		[HttpPost]
		[Route("ConfirmForgotPassword")]
		public async Task<IHttpActionResult> ConfirmForgotPassword(JustMoneyApiAccountConfirmForgotPasswordArguments arguments)
		{
			var newArguments = new AccountConfirmForgotPasswordArguments
			{
				Code = arguments.Code,
				UserId = arguments.UserId,
				Password = arguments.Password,
				ConfirmPassword = arguments.ConfirmPassword
			};

			await repository.ConfirmForgotPassword(newArguments);
				return Ok();
		}
		#endregion POST JustMoney/Api/Account/ConfirmForgotPassword

	}
}
