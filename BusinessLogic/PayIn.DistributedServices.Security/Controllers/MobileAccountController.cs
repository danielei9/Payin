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
	[RoutePrefix("Mobile/Account")]
	public class MobileAccountController : ApiController
	{
		private readonly SecurityRepository repository = null;

		#region Constructors
		public MobileAccountController()
		{
			repository = new SecurityRepository();
		}
		#endregion Constructors


		#region PUT Mobile/Account/ImageCrop
		[AllowAnonymous]
		[HttpPut]
		[Route("ImageCrop")]
		public async Task UserAvatar(MobileAccountUpdatePhotoArguments arguments)
		{
			await repository.UpdateImageUrlAsync_FromString(arguments.Avatar);
		}
		#endregion PUT Mobile/Account/ImageCrop

		#region DELETE Mobile/Account/DeleteImage
		[AllowAnonymous]
		[HttpDelete]
		[Route("DeleteImage")]
		public async Task DeleteImage()
		{
			await repository.DeleteImageAsync();
		}
		#endregion DELETE Mobile/Account/DeleteImage
		
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
