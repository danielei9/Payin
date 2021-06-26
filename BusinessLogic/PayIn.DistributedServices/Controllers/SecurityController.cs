using PayIn.Application.Dto.Security.Arguments;
using System.Threading.Tasks;
using System.Web.Http;
using Xp.Common.Application;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Controllers
{
	public class SecurityController : ApiController
	{
		//#region POST Administrator
		//[HttpPost]
		//public async Task<dynamic> Administrator(
		//	AccountCreateAdministratorArguments arguments,
		//	[Injection] IServiceBaseHandler<AccountCreateAdministratorArguments> handler
		//)
		//{
		//	var result = handler.ExecuteAsync(arguments);
		//	return result;
		//}
		//#endregion POST Administrator

		//#region POST Commerce
		//[HttpPost]
		//public async Task<dynamic> Commerce(
		//	AccountCreateCommerceArguments arguments,
		//	[Injection] IServiceBaseHandler<AccountCreateCommerceArguments> handler
		//)
		//{
		//	var result = handler.ExecuteAsync(arguments);
		//	return result;
		//}
		//#endregion POST Commerce

		#region POST Superadministrator
		[HttpPost]
		[AllowAnonymous]
		public async Task<dynamic> Superadministrator(
			AccountCreateSuperadministratorArguments arguments,
			[Injection] IServiceBaseHandler<AccountCreateSuperadministratorArguments> handler
		)
		{
			var result = handler.ExecuteAsync(arguments);
			return result;
		}
		#endregion POST Superadministrator

		//#region POST User
		//[HttpPost]
		//public async Task<dynamic> User(
		//	AccountCreateUserArguments arguments,
		//	[Injection] IServiceBaseHandler<AccountCreateUserArguments> handler
		//)
		//{
		//	var result = handler.ExecuteAsync(arguments);
		//	return result;
		//}
		//#endregion POST User
	}
}
