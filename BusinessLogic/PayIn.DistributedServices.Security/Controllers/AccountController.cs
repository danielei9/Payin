using Microsoft.Owin.Security;
using PayIn.Application.Dto.Arguments.SystemCardMember;
using PayIn.Application.Dto.Results.Account;
using PayIn.Application.Dto.Security.Arguments;
using PayIn.Application.Dto.Security.Results;
using PayIn.Domain.Security;
using PayIn.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Xp.Application.Dto;
using Xp.DistributedServices.ModelBinder;

namespace PayIn.DistributedServices.Security.Controllers
{
	//[HideSwagger]
	[ApiExplorerSettings(IgnoreApi = true)]
	[RoutePrefix("api/Account")]
	public class AccountController : ApiController
	{
		private readonly SecurityRepository repository = null;

		#region Authentication
		private IAuthenticationManager Authentication
		{
			get { return Request.GetOwinContext().Authentication; }
		}
		#endregion Authentication

		#region Constructors
		public AccountController()
		{
			repository = new SecurityRepository();
		}
		#endregion Constructors

		#region GET api/Account/Current
		//[Authorize]
		[HttpGet]
		[Route("Current")]
		public async Task<ResultBase<AccountGetCurrentResult>> Current()
		{
			var result = await repository.GetUserIdentityAsync();
			var res = new AccountGetCurrentResult
			{				
				Name = result.Name,
				Mobile = result.Mobile,
				Sex = result.Sex,
				TaxNumber = result.TaxNumber,
				TaxName = result.TaxName,
				TaxAddress = result.TaxAddress,
				Birthday = result.Birthday,
				PhotoUrl = result.PhotoUrl + (result.PhotoUrl == "" ? "" : "?now=" + DateTime.Now.ToString())
			};

			var list = new List<AccountGetCurrentResult>();
			list.Add(res);

			return new ResultBase<AccountGetCurrentResult> { Data = list };
		}
		#endregion GET api/Account/Current

		#region POST api/Account
		[AllowAnonymous]
		[HttpPost]
		[Route("")]
		public async Task Post(
			AccountRegisterArguments userModel
		)
		{
			await repository.CreateUserAsync(userModel, AccountRoles.User);			
		}
		#endregion POST api/Account	

		#region PUT api/Account/ConfirmEmail
		[AllowAnonymous]
		[HttpPut]
		[Route("ConfirmEmail")]
		public async Task ConfirmEmail(
			AccountConfirmArguments arguments
		)
		{
			await repository.ConfirmEmail(arguments);
		}
		#endregion PUT api/Account/ConfirmEmail

		#region PUT api/Account/ConfirmEmailAndData
		[AllowAnonymous]
		[HttpPut]
		[Route("ConfirmEmailAndData")]
		public async Task ConfirmEmailAndData(
			AccountConfirmEmailAndDataArguments arguments
		)
		{
			await repository.ConfirmEmailAndData(arguments);
		}
		#endregion PUT api/Account/ConfirmEmailAndData
		
		#region PUT api/Account/ConfirmCompanyEmailAndData
		[AllowAnonymous]
		[HttpPut]
		[Route("ConfirmCompanyEmailAndData")]
		public async Task ConfirmCompanyEmailAndData(
			AccountConfirmCompanyEMailAndDataArguments arguments,
			[Injection] IServiceBaseHandler<SystemCardMemberAddInvitedCompanyArguments> handler
		)
		{
			AccountConfirmEmailAndDataArguments userArguments = new AccountConfirmEmailAndDataArguments
			{
				userId = arguments.userId,
				code = arguments.code,
				password = arguments.password,
				mobile = arguments.mobile
			};

			await repository.ConfirmEmailAndData(userArguments);

			//var user = UserManager.FindById(arguments.userId);
			SystemCardMemberAddInvitedCompanyArguments addInvitedCompanyArguments = new SystemCardMemberAddInvitedCompanyArguments
			(
				login: arguments.userName,
				name: arguments.name,
				mobile: arguments.mobile,
				bankAccountNumber: arguments.bankAccountNumber,
				address: arguments.address,
				observations: arguments.observations,
				taxNumber: arguments.taxNumber,
				taxName: arguments.taxName,
				taxAddress: arguments.taxAddress
			);


			// Llamar al controller de crear supplier, serviceconcession y paymentconcession
			//var result = await handler.ExecuteAsync(addInvitedCompanyArguments);
			await handler.ExecuteAsync(addInvitedCompanyArguments);

		}
		#endregion PUT api/Account/ConfirmCompanyEmailAndData

		#region POST api/Account/ForgotPassword
		[AllowAnonymous]
		[HttpPost]
		[Route("ForgotPassword")]
		public async Task<IHttpActionResult> ForgotPassword(AccountForgotPasswordArguments arguments)
		{
			await repository.ForgotPassword(arguments);

			return Ok();
		}
		#endregion POST api/Account/ForgotPassword

		#region POST api/Account/ConfirmForgotPassword
		[AllowAnonymous]
		[HttpPost]
		[Route("ConfirmForgotPassword")]
		public async Task<IHttpActionResult> ConfirmForgotPassword(AccountConfirmForgotPasswordArguments arguments)
		{
			await repository.ConfirmForgotPassword(arguments);

			return Ok();
		}
		#endregion POST api/Account/ConfirmForgotPassword

		//#region POST api/Account/ConfirmForgotPasswordTurismeVilamarxant
		//[AllowAnonymous]
		//[HttpPost]
		//[Route("ConfirmForgotPasswordTurismeVilamarxant")]
		//public async Task<IHttpActionResult> ConfirmForgotPasswordTurismeVilamarxant(AccountConfirmForgotPasswordArguments arguments)
		//{
		//	await repository.ConfirmForgotPasswordTurismeVilamarxant(arguments);

		//	return Ok();
		//}
		//#endregion POST api/Account/ConfirmForgotPasswordTurismeVilamarxant

		#region POST api/Account/UpdatePassword
		[AllowAnonymous]
		[HttpPost]
		[Route("UpdatePassword")]
		public async Task UpdatePassword(AccountChangePasswordArguments arguments)
		{
			await repository.UpdatePassword(arguments);
		}
		#endregion POST api/Account/UpdatePassword

		#region PUT api/Account
		[AllowAnonymous]
		[HttpPut]
		[Route("")]
		public async Task Update(AccountUpdateArguments arguments)
		{
			await repository.UpdateProfileAsync(arguments);
		}
		#endregion PUT api/Account

		#region PUT api/Account/ImageCrop
		[AllowAnonymous]
		[HttpPut]
		[Route("ImageCrop")]
		public async Task UserAvatar(AccountUpdatePhotoArguments arguments)
		{
			await repository.UpdateImageUrlAsync(arguments.Avatar);
		}
		#endregion PUT api/Account/ImageCrop

		#region DELETE api/Account/DeleteImage
		[AllowAnonymous]
		[HttpDelete]
		[Route("DeleteImage")]
		public async Task DeleteImage()
		{
			await repository.DeleteImageAsync();
		}
		#endregion DELETE api/Account/DeleteImage

		#region GET api/Account/GetUsers
		[Authorize(Roles = AccountRoles.Superadministrator)]
		[HttpGet]
		[Route("GetUsers")]
		public async Task<ResultBase<AccountGetUsersResult>> GetUsers()
		{
			var result = await repository.GetUsers();
			//var list = new List<AccountGetUsersResult>();

			if (result != null)
			{
			//	list.AddRange(result);
				return result;
			}

			return null;			
		}
		#endregion GET api/Account/GetUsers
		
		#region PUT api/Account/OverwritePassword/{id}
		[Authorize(Roles = AccountRoles.Superadministrator)]
		[HttpPut]
		[Route("OverwritePassword/{id}")]
		public async Task OverwritePassword(  
			[FromUri] string id,	
			[FromBody] AccountOverwritePasswordArguments arguments
		)
		{			
			arguments.Id = id;
			await repository.OverwritePassword(arguments);
		}
		#endregion PUT api/Account/OverwritePassword/{id}

		#region PUT api/Account/UnlockUser/{id}
		[Authorize(Roles = AccountRoles.Superadministrator)]
		[HttpPut]
		[Route("UnlockUser/{id}")]
		public async Task UnlockUser(
			[FromUri] string id,
			[FromBody] AccountUnlockUserArguments arguments
		)
		{
			arguments.Id = id;
			await repository.UnlockUser(arguments);
		}
		#endregion PUT api/Account/UnlockUser/{id}

		#region POST api/Account/OverridePassword
		[Authorize(Roles = AccountRoles.Superadministrator)]
		[HttpPost]
		[Route("OverridePassword")]
		public async Task OverridePassword(AccountOverridePasswordArguments arguments)
		{
			await repository.OverridePassword(arguments);
		}
		#endregion POST api/Account/OverridePassword

		#region PUT api/Account/Logout
		[AllowAnonymous]
		[HttpPut]
		[Route("Logout")]
		public async Task<IHttpActionResult> Logout(AccountLogoutArguments arguments)
		{
			var result = await repository.DeleteRefreshTokenAsync();
			if (result)
				return Ok();

			return BadRequest("Token Id does not exist");
		}
		#endregion PUT api/Account/Logout

		#region Dispose
		protected override void Dispose(bool disposing)
		{
			if (disposing)
				if (repository != null)
					repository.Dispose();

			base.Dispose(disposing);
		}
		#endregion Dispose
	}
}
