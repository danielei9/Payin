using PayIn.Application.Dto.Payments.Arguments.Purse;
using PayIn.Common.Security;
using PayIn.Domain.Payments;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure.Repositories;

namespace PayIn.Application.Public.Handlers
{
	public class PurseUpdatePhotoHandler :
		IServiceBaseHandler<PurseUpdatePhotoArguments>
	{
		private readonly IEntityRepository<Purse> Repository;

		#region Constructors
		public PurseUpdatePhotoHandler(
			IEntityRepository<Purse> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(PurseUpdatePhotoArguments arguments)
		{
			var now = DateTime.Now;
			var purse = await Repository.GetAsync(arguments.Id);
			var azureBlob = new AzureBlobRepository();
			byte[] b1 = System.Text.Encoding.UTF8.GetBytes (arguments.Image);
            var name = arguments.Id + "_" + Guid.NewGuid();

            var oldImage = purse.Image;
			purse.Image = azureBlob.SaveImage(SecurityResources.FotoPurseShortUrl.FormatString(name), b1);

			if (oldImage != null && oldImage != "")
			{
				var route = Regex.Split(oldImage, "[/?]");
				var fileName = route[route.Length - 2] + "/" + route[route.Length - 1];
				azureBlob.DeleteFile(oldImage);
			}
			return purse;
		}
        #endregion ExecuteAsync
    }
}
