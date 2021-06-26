using PayIn.Application.Dto.Arguments;
using PayIn.Domain.Public;
using PayIn.Common.Resources;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
using Xp.Infrastructure.Repositories;

namespace PayIn.Application.Handlers
{
	public class SystemCardUpdatePhotoHandler :
		IServiceBaseHandler<SystemCardUpdatePhotoArguments>
	{
		private readonly IEntityRepository<SystemCard> Repository;

		#region Constructors
		public SystemCardUpdatePhotoHandler(
			IEntityRepository<SystemCard> repository
		)
		{
			if (repository == null) throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		public async Task<dynamic> ExecuteAsync(SystemCardUpdatePhotoArguments arguments)
		{
			if (arguments.Id <= 0)
				throw new ArgumentException("Id");

			var now = DateTime.Now;
			var oldImage = "";
			var azureBlob = new AzureBlobRepository();
			byte[] b1 = System.Text.Encoding.UTF8.GetBytes (arguments.PhotoUrl);
			var name = "_" + Guid.NewGuid(); ;
			var systemCard = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id)
				.FirstOrDefault();

			oldImage = systemCard.PhotoUrl;
			name = arguments.Id.ToString() + name;

			systemCard.PhotoUrl = azureBlob.SaveImage(SystemCardResources.PhotoShortUrl.FormatString(name), b1);

			if (oldImage != null && oldImage != "")
			{
				azureBlob.DeleteFile(oldImage);
			}

			return systemCard;
		}
        #endregion ExecuteAsync
    }
}
