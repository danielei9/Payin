using PayIn.Application.Dto.JustMoney.Arguments;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Application.Dto;
using Xp.DistributedServices.Filters;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.JustMoney.Controllers.Api
{
	[HideSwagger]
	[RoutePrefix("JustMoney/Api/Pfs")]
	public class JustMoneyApiPfsController : ApiController
	{
		#region GET /
		[HttpGet]
		[Route]
		public async Task<dynamic> Get()
		{
			return "Hola";
		}
		#endregion GET /

		#region POST /Confirm
		[HttpPost]
		[Route("Confirm")]
		public async Task Confirm(
			[FromBody] JustMoneyApiPfsConfirmArguments arguments,
			[Injection] IServiceBaseHandler<JustMoneyApiPfsConfirmArguments> handler
		)
		{
			await handler.ExecuteAsync(arguments);
		}
		#endregion POST /Confirm
	}
}
